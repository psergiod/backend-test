using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Salon.Application.Users.Interfaces;
using Salon.Domain.Models.Enums;
using Salon.Domain.Users.Contracts;
using System.Threading.Tasks;

namespace SalonApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCommand user)
        {
            var response = await _userService.CreateUser(user);

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userService.GetAllUsers();

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> Update(UpdateUserCommand user)
        {
            var response = await _userService.UpdateUser(user);

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await _userService.GetUserByIdAsync(ObjectId.Parse(id));

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Remove([FromRoute] string id)
        {
            var response = await _userService.DeleteUser(ObjectId.Parse(id));

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }

    }
}
