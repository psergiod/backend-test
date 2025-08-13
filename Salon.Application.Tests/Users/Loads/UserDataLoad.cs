using Salon.Application.Tests.DataLoad;
using Salon.Application.Tests.Users.Fakes;
using Salon.Domain.Users.Repositories;
using System.Threading.Tasks;
using UserEntity = Salon.Domain.Users.Entities.User;

namespace Salon.Application.Tests.Users.Loads
{
    public class UserDataLoad : IDataLoad<UserEntity>
    {
        private readonly IUserRepository _repository;

        public UserDataLoad(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task LoadAsync()
        {
            await _repository.InsertAsync(UserFake.GetUserEntity());
            await _repository.InsertAsync(UserFake.GetUserRobert());
            await _repository.InsertAsync(UserFake.GetUserTony());
        }

        public async Task<bool> IsLoadedAsync()
        {
            return await _repository.GetByIdAsync(UserFake.IdFoo) != null &&
                await _repository.GetUserByLoginAsync(UserFake.LOGIN_ROBERT) != null;
        }
    }
}
