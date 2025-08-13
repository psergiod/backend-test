using Salon.Domain.Users.Repositories;

namespace Salon.Application.Users.Validators
{
    public class UpdateUserCommandValidator : UserCommandValidator
    {
        public UpdateUserCommandValidator(IUserRepository userRepository)
            : base(userRepository)
        {
            ValidateLogin();
        }
    }
}
