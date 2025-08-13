using FluentValidation;
using MongoDB.Bson;
using Salon.Domain.Users.Contracts;
using Salon.Domain.Users.Repositories;
using System.Threading.Tasks;

namespace Salon.Application.Users.Validators
{
    public class UserCommandValidator : AbstractValidator<UserCommand>
    {
        private const string USER_ALREADY_EXIST = @"Login already exist!";
        private const string LOGIN_CANT_BE_EMPTY = "Login can't be empty!";
        private const string PASSWORD_TOO_SHORT = "Password must be bigger than 5 characters!";
        private readonly IUserRepository _userRepository;

        public UserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            ValidatePassword();
            ValidateLogin();
        }

        public void ValidateLogin()
        {
            RuleFor(x => x.Login)
               .NotEmpty()
               .WithMessage(LOGIN_CANT_BE_EMPTY);

            RuleFor(x => x)
                .MustAsync(async (x, cancelation) => await IsLoginValid(x))
                .WithSeverity(Severity.Error)
                .WithMessage(USER_ALREADY_EXIST);
        }

        public void ValidatePassword()
        {
            RuleFor(x => x)
                .Custom((command, context) =>
                {
                    if (command.Password.Length <= 5)
                        context.AddFailure(PASSWORD_TOO_SHORT);
                });
        }

        private async Task<bool> IsLoginValid(UserCommand command)
        {
            if (!string.IsNullOrEmpty(command.Id))
            {
                var canParse = ObjectId.TryParse(command.Id, out var parsedId);
                if (!canParse)
                    return true;

                var user = await _userRepository.GetUserByLoginAsync(command.Login, parsedId);

                if (user == null) return true;

                return false;
            }

            return (await _userRepository.GetUserByLoginAsync(command.Login)) == null;
        }
    }
}
