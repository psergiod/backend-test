using Salon.Application.Helpers;
using Salon.Application.Users.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.Users.Contracts;
using Salon.Domain.Users.Repositories;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Salon.Application.Users.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private const string WRONG_CREDENTIALS = "Username or Password is incorrect!";
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserAuthenticationService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<Result> Authenticate(AuthCommand authCommand)
        {
            var user = await _userRepository.GetUserByLoginAsync(authCommand.Login);

            if (user == null || !BCryptNet.Verify(authCommand.Password, user.Password))
            {
                return ResultHelper.GetErrorResult(WRONG_CREDENTIALS);
            }

            return new Result(_tokenService.GenerateToken(user));
        }
    }
}
