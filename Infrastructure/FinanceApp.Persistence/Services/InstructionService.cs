using AutoMapper;
using FinanceApp.Application.Features.Commands.InstructionsCommands;
using FinanceApp.Application.Features.Exceptions;
using FinanceApp.Application.Features.Results.InstructionsResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class InstructionService : IInstructionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly InstructionRules instructionRules;
        private readonly IMapper mapper;

        public InstructionService(IUnitOfWork unitOfWork, InstructionRules instructionRules, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.instructionRules = instructionRules;
            this.mapper = mapper;
        }

        public async Task CreateInstructionAsync(CreateInstructionCommand request, int userId)
        {
            var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(x => x.UserId == userId);

            if (request.MonthlyInstruction)
            {
                var groupId = Guid.NewGuid();  
                var newInstructions = new List<Instructions>();

                for (int i = 0; i < request.InstructionTime; i++)
                {
                    var scheduledDate = request.ScheduledDate.AddMonths(i);

                    var exists = instructions.Any(x =>
                        x.Title.Equals(request.Title, StringComparison.OrdinalIgnoreCase) &&
                        x.ScheduledDate.Date == scheduledDate.Date);

                    if (exists)
                        throw new InstructionNameNotMustBeSameException();

                    newInstructions.Add(new Instructions
                    {
                        Title = request.Title,
                        Amount = request.Amount,
                        ScheduledDate = scheduledDate,
                        IsPaid = false,
                        Description = request.Description,
                        UserId = userId,
                        GroupId = groupId,
                    });
                }

                await unitOfWork.GetWriteRepository<Instructions>().AddRangeAsync(newInstructions);
            }
            else
            {
                var exists = instructions.Any(x =>
                    x.Title.Equals(request.Title, StringComparison.OrdinalIgnoreCase) &&
                    x.ScheduledDate.Date == request.ScheduledDate.Date);

                if (exists)
                    throw new InstructionNameNotMustBeSameException();

                await unitOfWork.GetWriteRepository<Instructions>().AddAsync(new Instructions
                {
                    Title = request.Title,
                    Amount = request.Amount,
                    ScheduledDate = request.ScheduledDate,
                    IsPaid = false,
                    Description = request.Description,
                    UserId = userId,
                });
            }

            await unitOfWork.SaveAsync();
        }

        public async Task<IList<GetAllInstructionsByUserQueryResult>> GetAllUnpaidInstructionsByUserAsync(int userId)
        {
            var instructions = await unitOfWork.GetReadRepository<Instructions>()
                       .GetAllAsync(x => !x.IsPaid && x.UserId == userId);

            return mapper.Map<IList<GetAllInstructionsByUserQueryResult>>(instructions);
        }

        public async Task<GetInstructionCountByUserQueryResult> GetInstructionSummaryByUserIdAsync(int userId)
        {
            int totalInstruction = await unitOfWork.GetReadRepository<Instructions>().CountAsync(x => x.UserId == userId);
            int waitingInstruction = await unitOfWork.GetReadRepository<Instructions>().CountAsync(x => !x.IsPaid && x.UserId == userId);
            int paidInstruction = await unitOfWork.GetReadRepository<Instructions>().CountAsync(x => x.IsPaid && x.UserId == userId);
            var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(x => !x.IsPaid && x.UserId == userId);
            decimal totalAmount = instructions.Sum(x => x.Amount);

            return new GetInstructionCountByUserQueryResult
            {
                TotalInstruction = totalInstruction,
                WaitingInstruction = waitingInstruction,
                PaidInstruction = paidInstruction,
                TotalAmount = totalAmount
            };
        }

        public async Task RemoveInstructionAsync(int instructionId, int userId)
        {
            var instruction = await unitOfWork.GetReadRepository<Instructions>().GetAsync(x => x.Id == instructionId);
            await instructionRules.InstructionsNotFound(instruction);
            await instructionRules.IsThisYourInstruction(instruction, userId);


            if (instruction.GroupId != null) 
            {
                var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(x => x.GroupId == instruction.GroupId);
                await unitOfWork.GetWriteRepository<Instructions>().HardDeleteRangeAsync(instructions);
                await unitOfWork.SaveAsync();
            }
            else
            {
                await unitOfWork.GetWriteRepository<Instructions>().HardDeleteAsync(instruction);
                await unitOfWork.SaveAsync();
            }          
        }

        public async Task<bool> SetInstructionPaidTrueAsync(int instructionId, int userId)
        {
            var instruction = await unitOfWork.GetReadRepository<Instructions>().GetAsync(x => x.Id == instructionId);
            await instructionRules.InstructionsNotFound(instruction);
            await instructionRules.IsThisYourInstruction(instruction, userId);

            instruction.IsPaid = true;
            await unitOfWork.GetWriteRepository<Instructions>().UpdateAsync(instruction);
            await unitOfWork.SaveAsync();

            return instruction.IsPaid;
        }

        public async Task UpdateInstructionAsync(UpdateInstructionCommand request, int userId)
        {
            var instruction = await unitOfWork.GetReadRepository<Instructions>().GetAsync(x => x.Id == request.Id);
            await instructionRules.InstructionsNotFound(instruction);
            await instructionRules.IsThisYourInstruction(instruction, userId);

            if(instruction.GroupId != null)
            {
                var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(x => x.GroupId == instruction.GroupId);

                var sortedInstructions = instructions.OrderBy(x => x.ScheduledDate).ToList();

                for (int i = 0; i < sortedInstructions.Count; i++)
                {
                    var item = sortedInstructions[i];

                    item.Title = request.Title;
                    item.Description = request.Description;
                    item.Amount = request.Amount;
                    item.ScheduledDate = request.ScheduledDate.AddMonths(i);

                    await unitOfWork.GetWriteRepository<Instructions>().UpdateAsync(item);
                }

                await unitOfWork.SaveAsync();
            }
            else
            {
                instruction.Title = request.Title;
                instruction.Description = request.Description;
                instruction.ScheduledDate = request.ScheduledDate;
                instruction.Amount = request.Amount;

                await unitOfWork.GetWriteRepository<Instructions>().UpdateAsync(instruction);
                await unitOfWork.SaveAsync();
            }
           
        }
    }
}
