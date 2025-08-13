using Salon.Domain.Clients.Repositories;

namespace Salon.Application.Clients.Validators
{
    public class UpdateClientCommandValidator : ClientCommandValidator
    {
        public UpdateClientCommandValidator(IClientRepository clientRepository)
            : base(clientRepository)
        {
            ValidateId();
        }
    }
}
