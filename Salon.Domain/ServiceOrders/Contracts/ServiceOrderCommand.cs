using Salon.Domain.Models.Enums;
using System;
using System.Collections.Generic;

namespace Salon.Domain.ServiceOrders.Contracts
{
    public class ServiceOrderCommand
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public DateTime Date { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Obs { get; set; }
        public List<ItemOrderDto> Items { get; set; }
    }
}
