using Salon.Domain.ServiceOrders.Entities;
using Salon.Domain.ServiceOrders.Repositories;
using Salon.Infra.DbContext;

namespace Salon.Infra.Repositories
{
    public class ServiceOrderRepository : Repository<ServiceOrder>, IServiceOrderRepository
    {
        public ServiceOrderRepository(
            IMongoDbContext dbContext,
            bool showRemoved = false)
            : base(dbContext, showRemoved)
        { }
    }
}
