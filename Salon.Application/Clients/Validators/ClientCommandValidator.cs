using FluentValidation;
using MongoDB.Bson;
using Salon.Domain.Clients.Contracts;
using Salon.Domain.Clients.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Salon.Application.Clients.Validators
{
    public class ClientCommandValidator : AbstractValidator<ClientCommand>
    {
        private const string EMAIL_INVALID = "{0} is not a valid Email!";
        private const string EMAIL_ALREADY_IN_USE = "Email {0} Already in use!";
        private const string INVALID_ID = "Id is invalid!";
        private readonly IClientRepository _clientRepository;

        public ClientCommandValidator(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;

            ValidateEmail();
        }

        public void ValidateEmail()
        {
            RuleFor(x => x.Email)
                .Must(email => IsEmailValid(email))
                .WithSeverity(Severity.Error)
                .WithMessage(x => string.Format(EMAIL_INVALID, x.Email));

            RuleFor(x => x)
                .MustAsync(async (x, cancelation) => await IsEmailAvailable(x))
                .WithSeverity(Severity.Error)
                .WithMessage(x => string.Format(EMAIL_ALREADY_IN_USE, x.Email));
        }

        public void ValidateId()
        {
            RuleFor(x => x)
                .MustAsync(async (x, cancelation) => await IsValidId(x.Id))
                .WithSeverity(Severity.Error)
                .WithMessage(INVALID_ID);
        }

        private async Task<bool> IsValidId(string id)
        {
            return ObjectId.TryParse(id, out var idParsed) && (await _clientRepository.ExistAsync(idParsed));
        }

        private bool IsEmailValid(string email)
        {
            var emailValidation = new EmailAddressAttribute();
            return emailValidation.IsValid(email);
        }

        private async Task<bool> IsEmailAvailable(ClientCommand command)
        {
            if (!string.IsNullOrEmpty(command.Id))
            {
                var canParse = ObjectId.TryParse(command.Id, out var parsedId);
                if (!canParse)
                    return true;

                var user = await _clientRepository.GetClientByEmailAndIdAsync(command.Email, parsedId);

                if (user == null) return true;

                return false;
            }

            return (await _clientRepository.GetClientByEmailAsync(command.Email)) == null;
        }
    }
}
