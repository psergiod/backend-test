using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Salon.Application.Clients.Interfaces;
using Salon.Domain.Clients.Contracts;
using System.Threading.Tasks;

namespace SalonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientCommand client)
        {
            var response = await _clientService.CreateClient(client);

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("all/{amount}")]
        public async Task<IActionResult> GetAll([FromRoute] int? amount)
        {
            var response = await _clientService.GetAllClients(amount);

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _clientService.GetAllClients();

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> Update(UpdateClientCommand client)
        {
            var response = await _clientService.UpdateClient(client);

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await _clientService.GetClientById(ObjectId.Parse(id));

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Remove([FromRoute] string id)
        {
            var response = await _clientService.DeleteClient(ObjectId.Parse(id));

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

    }
}
