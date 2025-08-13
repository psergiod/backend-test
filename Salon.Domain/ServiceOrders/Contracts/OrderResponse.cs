using System;
using System.Collections.Generic;
using System.Text;

namespace Salon.Domain.ServiceOrders.Contracts
{
    public class OrderResponse
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public List<ItemOrderDto> Items { get; set; } = new List<ItemOrderDto>();
        public DateTime Date { get; set; }
        public int PaymentMethod { get; set; }
        public string Obs { get; set; }
    }
}
