using FluentValidation;
using MongoDB.Bson;
using Salon.Application.Clients.Interfaces;
using Salon.Application.Helpers;
using Salon.Domain.Base;
using Salon.Domain.Clients.Contracts;
using Salon.Domain.Clients.Repositories;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Salon.Application.Clients.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IValidator<ClientCommand> _newValidator;
        private readonly IValidator<UpdateClientCommand> _updateClientValidator;
        private readonly IClientMapper _clientMapper;
        private readonly IClientServiceBuilder _clientServiceBuilder;

        public ClientService(
            IClientRepository clientRepository,
            IValidator<ClientCommand> newValidator,
            IValidator<UpdateClientCommand> updateClientValidator,
            IClientMapper clientMapper,
            IClientServiceBuilder clientServiceBuilder)
        {
            _clientRepository = clientRepository;
            _newValidator = newValidator;
            _updateClientValidator = updateClientValidator;
            _clientMapper = clientMapper;
            _clientServiceBuilder = clientServiceBuilder;
        }

        public async Task<Result> CreateClient(ClientCommand clientCommand)
        {
            var validation = await _newValidator.ValidateAsync(clientCommand);

            if (!validation.IsValid)
                return ResultHelper.GetErrorResult(validation.Errors);

            var client = _clientMapper.MapCommandToEntity(clientCommand);

            await _clientRepository.InsertAsync(client);

            return new Result(HttpStatusCode.Created);
        }

        public async Task<Result> DeleteClient(ObjectId id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            client.Remove();
            await _clientRepository.UpdateAsync(client);

            return new Result(HttpStatusCode.OK);
        }

        public async Task<Result> GetAllClients(int? amount = null)
        {
            var clients = (await _clientRepository.GetByExpressionAsync(_clientServiceBuilder.Build(), _clientMapper.MapResponse(), amount)).ToList();
            return new Result(clients, HttpStatusCode.OK);
        }

        public async Task<Result> GetClientById(ObjectId id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            return new Result(client != null ? _clientMapper.MapEntityToResponse(client) : null, HttpStatusCode.OK);
        }

        public async Task<Result> UpdateClient(UpdateClientCommand clientCommand)
        {
            var validation = await _updateClientValidator.ValidateAsync(clientCommand);

            if (!validation.IsValid)
                return ResultHelper.GetErrorResult(validation.Errors);

            var client = _clientMapper.MapCommandToEntity(clientCommand);

            await _clientRepository.UpdateAsync(client);

            return new Result(HttpStatusCode.OK);
        }
    }
}
