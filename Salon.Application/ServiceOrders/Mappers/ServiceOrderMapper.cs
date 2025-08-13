using MongoDB.Bson;
using Salon.Application.ServiceOrders.Interfaces;
using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.ServiceOrders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Salon.Application.ServiceOrders.Mappers
{
    public class ServiceOrderMapper : IServiceOrderMapper
    {
        public ServiceOrder MapCommandToEntity(ServiceOrderCommand command)
        {
            var entity = new ServiceOrder();
            entity.Id = string.IsNullOrEmpty(command.Id) ? ObjectId.Empty : ObjectId.Parse(command.Id);
            entity.InformClient(ObjectId.Parse(command.ClientId));
            entity.InformDate(command.Date);
            entity.InformPaymentMethod(command.PaymentMethod);
            entity.Obs = command.Obs;
            command.Items.ForEach(x => entity.AddItem(new ItemOrder { ItemId = ObjectId.Parse(x.Id), Value = x.Value }));

            return entity;
        }

        public Expression<Func<ServiceOrder, OrderResponse>> MapResponse()
        {
            return entity => new OrderResponse
            {
                Id = entity.Id.ToString(),
                ClientId = entity.ClientId.ToString(),
                Date = entity.Date,
                Items = entity.Services.Select(x => new ItemOrderDto { Id = x.ItemId.ToString(), Value = x.Value }).ToList(),
                Obs = entity.Obs,
                PaymentMethod = (int)entity.PaymentMethod
            };
        }
    }
}
