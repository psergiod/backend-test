using MongoDB.Bson;
using Salon.Application.Clients.Interfaces;
using Salon.Domain.Clients.Contracts;
using Salon.Domain.Clients.Entities;
using System;
using System.Linq.Expressions;

namespace Salon.Application.Clients.Mappers
{
    public class ClientMapper : IClientMapper
    {
        public Client MapCommandToEntity(ClientCommand command)
            => new Client(command.Name)
            { Id = !string.IsNullOrEmpty(command.Id) ? ObjectId.Parse(command.Id) : ObjectId.Empty }
            .InformBirthDate(command.BirthDate)
            .InformContactNumbers(command.ContactNumbers)
            .InformEmail(command.Email)
            .InformGender(command.Gender);

        public Expression<Func<Client, ClientResponse>> MapResponse()
        {
            return entity => new ClientResponse
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                BirthDate = entity.BirthDate,
                ContactNumbers = entity.ContactNumbers,
                Email = entity.Email,
                Gender = entity.Gender
            };
        }

        public ClientResponse MapEntityToResponse(Client client)
            => new ClientResponse
            {
                Id = client.Id.ToString(),
                Name = client.Name,
                BirthDate = client.BirthDate,
                ContactNumbers = client.ContactNumbers,
                Email = client.Email,
                Gender = client.Gender
            };
    };
}

