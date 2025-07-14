using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Exceptions;
using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Rules
{
    public class AuthRules : BaseRules
    {
        public Task UserShouldNotBeExist(User? user)
        {
            if (user is not null) throw new UserAlreadyExistException();
            return Task.CompletedTask;
        }

        public Task EmailOrPasswordShouldNotBeInvalid(User user, bool checkpassword)
        {
            if (user == null || !checkpassword) throw new EmailOrPasswordShouldNotBeInvalidException();
            return Task.CompletedTask;
        }

        public Task EmailAddressShouldBeValid(User? user)
        {
            if (user == null)
                throw new EmailAddressShouldBeValidException();

            return Task.CompletedTask;
        }

        public Task<int> GetValidatedUserId(string stringuserId)
        {
            if (!int.TryParse(stringuserId, out int userId)) throw new GetValidatedUserIdException();
            return Task.FromResult(userId);
        }

        public Task UserNameMustBeUnique(string FullName, IList<User> userList)
        {
            foreach(var user in userList)
            {
                if (user.FullName.Equals(FullName, StringComparison.OrdinalIgnoreCase))
                    throw new UserNameMustBeUniqueException();
            }
            return Task.CompletedTask;
        }

    }
}
