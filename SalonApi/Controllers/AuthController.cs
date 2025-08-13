using Microsoft.AspNetCore.Mvc;
using Salon.Application.Users.Interfaces;
using Salon.Domain.Users.Contracts;
using System.Threading.Tasks;

namespace SalonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserAuthenticationService _userAuthenticationService;

        public AuthController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthCommand authCommand)
        {
            var response = await _userAuthenticationService.Authenticate(authCommand);

            Response.StatusCode = (int)response.StatusCode;

            return new JsonResult(response);
        }
    }
}
