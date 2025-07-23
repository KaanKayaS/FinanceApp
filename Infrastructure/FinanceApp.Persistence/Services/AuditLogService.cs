using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IUnitOfWork unitOfWork;

        public AuditLogService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task LogActionAsync(
            int? userId,
            string userName,
            string actionName,
            string requestData,
            string? responseData,
            int durationMs,
            bool success,
            string? errorMessage = null,
            string? ipAddress = null,
            string? userAgent = null)
        {
            var auditLog = new AuditLog
            {
                UserId = userId,
                UserName = userName,
                ActionName = actionName,
                RequestData = requestData,
                ResponseData = responseData,
                DurationMs = durationMs,
                Success = success,
                ErrorMessage = errorMessage,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                ExecutedAt = DateTime.UtcNow.AddHours(3)
            };

            await unitOfWork.GetWriteRepository<AuditLog>().AddAsync(auditLog);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByUserAsync(int userId, int pageNumber = 1, int pageSize = 50)
        {
            return await unitOfWork.GetReadRepository<AuditLog>()
                .GetAllAsync(
                    predicate: a => a.UserId == userId,
                    orderBy: q => q.OrderByDescending(a => a.ExecutedAt),
                    include: q => q.Include(a => a.User),
                    enableTracking: false
                );
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByActionAsync(string actionName, int pageNumber = 1, int pageSize = 50)
        {
            return await unitOfWork.GetReadRepository<AuditLog>()
                .GetAllAsync(
                    predicate: a => a.ActionName == actionName,
                    orderBy: q => q.OrderByDescending(a => a.ExecutedAt),
                    include: q => q.Include(a => a.User),
                    enableTracking: false
                );
        }

        public async Task<IEnumerable<AuditLog>> GetFailedActionsAsync(DateTime? from = null, DateTime? to = null)
        {
            return await unitOfWork.GetReadRepository<AuditLog>()
                .GetAllAsync(
                    predicate: a => !a.Success && 
                                   (from == null || a.ExecutedAt >= from) &&
                                   (to == null || a.ExecutedAt <= to),
                    orderBy: q => q.OrderByDescending(a => a.ExecutedAt),
                    include: q => q.Include(a => a.User),
                    enableTracking: false
                );
        }

        public async Task<AuditLog?> GetAuditLogByIdAsync(int id)
        {
            return await unitOfWork.GetReadRepository<AuditLog>()
                .GetAsync(
                    predicate: a => a.Id == id,
                    include: q => q.Include(a => a.User),
                    enableTracking: false
                );
        }
    }
} 