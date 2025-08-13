using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Salon.Application.ServiceOrders.Interfaces;
using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.ServiceOrders.Entities;
using System.Threading.Tasks;

namespace SalonApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService itemService;

        public ItemController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItemCommand item)
        {
            var response = await itemService.CreateItem(item);

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response); // avaliar OK or BADREQUEST  or NOTFOUND
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await itemService.GetAllAsync();

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await itemService.GetItemById(ObjectId.Parse(id));

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Remove([FromRoute] string id)
        {
            var response = await itemService.RemoveItem(ObjectId.Parse(id));

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }
    }
}
