using MongoDB.Bson;
using Salon.Domain.Models.Enums;
using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.ServiceOrders.Entities;
using System;
using System.Collections.Generic;

namespace Salon.Application.Tests.ServiceOrders.Fakes
{
    public static class ServiceOrderFake
    {
        public static ObjectId IdServiceOrder1 = ObjectId.Parse("649205a22524c80b79e0d1b1");
        public static ObjectId IdServiceOrder2 = ObjectId.Parse("649205a22524c80b79e0d1b2");
        public static ObjectId ClientId1 = ObjectId.Parse("649205a22524c80b79e0d1c1");
        public static ObjectId ClientId2 = ObjectId.Parse("649205a22524c80b79e0d1c2");

        public static ServiceOrder GetServiceOrderEntity()
        {
            return new ServiceOrder()
                .InformClient(ClientId1)
                .InformDate(DateTime.UtcNow.Date)
                .InformPaymentMethod(PaymentMethod.Credit)
                .AddItem(new ItemOrder(ItemFake.GetItemBrazilianBlowout(), 1));
        }

        public static ServiceOrder GetServiceOrderEntityWithId()
        {
            var order = GetServiceOrderEntity();
            order.Id = IdServiceOrder1;
            return order;
        }

        public static ServiceOrderCommand GetServiceOrderCommand()
        {
            return new ServiceOrderCommand
            {
                ClientId = ClientId1.ToString(),
                Date = DateTime.UtcNow.Date,
                PaymentMethod = PaymentMethod.Credit,
                Obs = "Test observation",
                Items = new List<ItemOrderDto>
                {
                    new ItemOrderDto
                    {
                        Id = ItemFake.GetItemBrazilianBlowout().Id.ToString(),
                        Amount = 1
                    }
                }
            };
        }

        public static ServiceOrderCommand GetInvalidServiceOrderCommand()
        {
            return new ServiceOrderCommand
            {
                ClientId = "", // Invalid empty client ID
                Date = DateTime.MinValue,
                PaymentMethod = PaymentMethod.Credit,
                Items = new List<ItemOrderDto>()
            };
        }

        public static ServiceOrder GetServiceOrderForClient2()
        {
            var order = new ServiceOrder()
                .InformClient(ClientId2)
                .InformDate(DateTime.UtcNow.Date.AddDays(-1))
                .InformPaymentMethod(PaymentMethod.Debit)
                .AddItem(new ItemOrder(ItemFake.GetItemBrazilianBlowout(), 2));
            order.Id = IdServiceOrder2;
            return order;
        }
    }
}