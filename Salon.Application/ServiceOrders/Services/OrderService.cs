using FluentValidation;
using MongoDB.Bson;
using Salon.Application.Helpers;
using Salon.Application.ServiceOrders.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.ServiceOrders.Repositories;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Salon.Application.ServiceOrders.Services
{
    public class OrderService : IOrderService
    {
        private const string INVALID_ID = "Order Invalid";

        private readonly IServiceOrderRepository _repository;
        private readonly IValidator<ServiceOrderCommand> _newValidator;
        private readonly IServiceOrderMapper _serviceOrderMapper;
        private readonly IOrderServiceBuilder _orderServiceBuilder;

        public OrderService(
            IServiceOrderRepository repository,
            IValidator<ServiceOrderCommand> newValidator,
            IServiceOrderMapper serviceOrderMapper,
            IOrderServiceBuilder orderServiceBuilder)
        {
            _repository = repository;
            _newValidator = newValidator;
            _serviceOrderMapper = serviceOrderMapper;
            _orderServiceBuilder = orderServiceBuilder;
        }

        public async Task<Result> CreateOrder(ServiceOrderCommand command)
        {
            var validation = await _newValidator.ValidateAsync(command);

            if (!validation.IsValid)
                return ResultHelper.GetErrorResult(validation.Errors);

            var entity = _serviceOrderMapper.MapCommandToEntity(command);

            await _repository.InsertAsync(entity);

            return new Result(HttpStatusCode.Created);
        }

        public async Task<Result> DeleteOrder(ObjectId id)
        {
            var order = await _repository.GetByIdAsync(id);

            if (order == null)
            {
                return ResultHelper.GetErrorResult(INVALID_ID);
            }

            order.Enable();

            await _repository.UpdateAsync(order);

            return new Result(HttpStatusCode.OK);
        }

        public async Task<Result> GetAllOrders() => new Result(await _repository.GetByExpressionAsync(_orderServiceBuilder.Build(), _serviceOrderMapper.MapResponse()), HttpStatusCode.OK);

        public async Task<Result> GetOrderByIdAsync(ObjectId id)
        {
            var builder = _orderServiceBuilder.FilterById(id).Build();
            return new Result(await _repository.GetByFirstOrDefaultExpressionAsync(builder, _serviceOrderMapper.MapResponse()), HttpStatusCode.OK);
        }

        public async Task<Result> GetOrdersByClientId(ObjectId clientId)
        {
            var builder = _orderServiceBuilder.FilterByClientId(clientId).Build();

            var response = await _repository.GetByExpressionAsync(builder, _serviceOrderMapper.MapResponse());

            return new Result(response.OrderBy(x => x.Date));
        }
    }
}
