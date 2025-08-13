using Salon.Application.Tests.Clients.Fakes;
using Salon.Application.Tests.DataLoad;
using Salon.Domain.Clients.Repositories;
using System.Linq;
using System.Threading.Tasks;
using ClientEntity = Salon.Domain.Clients.Entities.Client;

namespace Salon.Application.Tests.Clients.Loads
{
    public class ClientDataLoad : IDataLoad<ClientEntity>
    {
        private readonly IClientRepository _repository;

        public ClientDataLoad(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task LoadAsync()
        {
            await _repository.InsertAsync(ClientFakes.GetClientMike());
            await _repository.InsertAsync(ClientFakes.GetClientBob());
            await _repository.InsertAsync(ClientFakes.GetClientTony());
        }

        public async Task<bool> IsLoadedAsync()
        {
            return (await _repository.GetAllAsync()).Any();
        }
    }
}
