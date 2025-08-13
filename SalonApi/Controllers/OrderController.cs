using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Salon.Application.ServiceOrders.Interfaces;
using Salon.Domain.ServiceOrders.Contracts;
using System.Threading.Tasks;

namespace SalonApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceOrderCommand command)
        {
            var response = await _orderService.CreateOrder(command);

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _orderService.GetAllOrders();

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await _orderService.GetOrderByIdAsync(ObjectId.Parse(id));

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Remove([FromRoute] string id)
        {
            var response = await _orderService.DeleteOrder(ObjectId.Parse(id));

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("client/{id}")]
        public async Task<IActionResult> GetOrdersFromClientId([FromRoute] string clientId)
        {
            var response = await _orderService.GetOrdersByClientId(ObjectId.Parse(clientId));

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }
    }
}
