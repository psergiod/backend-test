using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.ServiceOrders.Entities;
using System;
using System.Linq.Expressions;

namespace Salon.Application.ServiceOrders.Interfaces
{
    public interface IServiceOrderMapper
    {
        ServiceOrder MapCommandToEntity(ServiceOrderCommand command);
        Expression<Func<ServiceOrder, OrderResponse>> MapResponse();
    }
}