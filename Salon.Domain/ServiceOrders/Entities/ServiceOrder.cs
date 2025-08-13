using MongoDB.Bson;
using Salon.Domain.Models;
using Salon.Domain.Models.Enums;
using System;
using System.Collections.Generic;

namespace Salon.Domain.ServiceOrders.Entities
{
    public class ServiceOrder : Entity<ObjectId>
    {
        public ObjectId ClientId { get; private set; }
        public List<ItemOrder> Services { get; private set; } = new List<ItemOrder>();
        public DateTime Date { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public string Obs { get; set; }

        public ServiceOrder AddItem(ItemOrder item)
        {
            Services.Add(item);
            return this;
        }

        public ServiceOrder InformClient(ObjectId clientId)
        {
            this.ClientId = clientId;
            return this;
        }

        public ServiceOrder InformDate(DateTime date)
        {
            this.Date = date;
            return this;
        }

        public ServiceOrder InformPaymentMethod(PaymentMethod paymentMethod)
        {
            this.PaymentMethod = paymentMethod;
            return this;
        }
    }
}
