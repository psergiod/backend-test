using FluentValidation;
using MongoDB.Bson;
using Salon.Domain.Base;
using Salon.Domain.Clients.Repositories;
using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.ServiceOrders.Entities;
using Salon.Domain.ServiceOrders.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Salon.Application.ServiceOrders.Validators
{
    public class ServiceOrderCommandValidator : AbstractValidator<ServiceOrderCommand>
    {
        private const string INVALID_CLIENTID = "Invalid Client!";
        private const string INVALID_DATE = "Can't create Order from Future";
        private const string FIELD_EMPTY = "Field {0} can't be empty!";
        private const string INVALID_ITEM = "There is an Invalid Item!";

        private readonly IClientRepository _clientRepository;
        private readonly IRepository<Item> _itemRepository;
        public ServiceOrderCommandValidator(
            IServiceOrderRepository serviceOrderRepository,
            IClientRepository clientRepository,
            IRepository<Item> itemRepository)
        {
            _clientRepository = clientRepository;
            _itemRepository = itemRepository;

            ValidateClient();
            ValidateDate();
            ValidateItems();
        }

        public void ValidateClient()
        {
            RuleFor(command => command.ClientId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithMessage(x => string.Format(FIELD_EMPTY, nameof(x.ClientId)));

            RuleFor(command => command)
                .MustAsync(async (x, cancelation) => await IsValidClient(x.ClientId))
                .WithSeverity(Severity.Error)
                .WithMessage(INVALID_CLIENTID);
        }

        public void ValidateDate()
        {
            RuleFor(command => command.Date)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithMessage(x => string.Format(FIELD_EMPTY, nameof(x.Date)));

            RuleFor(command => command.Date)
                .GreaterThan(DateTime.Now)
                .WithSeverity(Severity.Error)
                .WithMessage(INVALID_DATE);
        }

        public void ValidateItems()
        {
            RuleFor(command => command.Items)
                .NotNull()
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithMessage(x => string.Format(FIELD_EMPTY, nameof(x.Items)));

            RuleFor(command => command.Items)
                .MustAsync(async (x, cancelation) => await IsItemsValid(x))
                .WithSeverity(Severity.Error)
                .WithMessage(INVALID_ITEM);
        }

        private async Task<bool> IsItemsValid(List<ItemOrderDto> items)
        {
            foreach (var item in items)
            {
                if (!ObjectId.TryParse(item.Id, out var idParsed) || !await _itemRepository.ExistAsync(idParsed))
                    return false;
            }

            return true;
        }

        private async Task<bool> IsValidClient(string clientId)
        {
            if (!string.IsNullOrEmpty(clientId) && ObjectId.TryParse(clientId, out var idParsed))
            {
                var client = await _clientRepository.GetByIdAsync(idParsed);

                return client != null;
            }

            return false;
        }
    }
}
