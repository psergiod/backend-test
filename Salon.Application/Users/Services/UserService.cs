using FluentValidation;
using MongoDB.Bson;
using Salon.Application.Helpers;
using Salon.Application.Users.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.Users.Contracts;
using Salon.Domain.Users.Repositories;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Salon.Application.Users.Services
{
    public class UserService : IUserService
    {
        private const string INVALID_ID = "User Invalid!";
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserCommand> _newValidator;
        private readonly IValidator<UpdateUserCommand> _updateValidator;
        private readonly IUserMapper _userMapper;
        private readonly IUserServiceBuilder _userServiceBuilder;

        public UserService(
            IUserRepository userRepository,
            IValidator<UserCommand> newValidator,
            IValidator<UpdateUserCommand> updateValidator,
            IUserMapper userMapper,
            IUserServiceBuilder userServiceBuilder)
        {
            _userRepository = userRepository;
            _newValidator = newValidator;
            _updateValidator = updateValidator;
            _userMapper = userMapper;
            _userServiceBuilder = userServiceBuilder;
        }

        public async Task<Result> CreateUser(UserCommand userCommand)
        {
            var validation = await _newValidator.ValidateAsync(userCommand);

            if (!validation.IsValid)
                return ResultHelper.GetErrorResult(validation.Errors);

            var User = _userMapper.MapCommandToEntity(userCommand);

            User.Password = BCryptNet.HashPassword(User.Password);

            await _userRepository.InsertAsync(User);

            return new Result(HttpStatusCode.Created);
        }

        public async Task<Result> DeleteUser(ObjectId id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ResultHelper.GetErrorResult(INVALID_ID);
            }

            user.Remove();
            await _userRepository.UpdateAsync(user);

            return new Result(HttpStatusCode.OK);
        }

        public async Task<Result> GetAllUsers()
        {
            var users = await _userRepository.GetByExpressionAsync(_userServiceBuilder.Build(), _userMapper.MapResponse());
            return new Result(users.ToList(), HttpStatusCode.OK);

        }

        public async Task<Result> GetUserByIdAsync(ObjectId id)
        {
            var builder = _userServiceBuilder.FilterById(id).Build();
            var user = await _userRepository.GetByFirstOrDefaultExpressionAsync(builder, _userMapper.MapResponse());
            return new Result(user, HttpStatusCode.OK);
        }

        public async Task<Result> UpdateUser(UpdateUserCommand UserCommand)
        {
            var validation = await _updateValidator.ValidateAsync(UserCommand);

            if (!validation.IsValid)
                return ResultHelper.GetErrorResult(validation.Errors);

            var User = _userMapper.MapCommandToEntity(UserCommand);

            User.Password = BCryptNet.HashPassword(User.Password);

            await _userRepository.UpdateAsync(User);

            return new Result(HttpStatusCode.OK);
        }
    }
}
