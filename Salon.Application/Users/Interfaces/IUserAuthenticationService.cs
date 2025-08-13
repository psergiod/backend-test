using Salon.Domain.Base;
using Salon.Domain.Users.Contracts;
using System.Threading.Tasks;

namespace Salon.Application.Users.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<Result> Authenticate(AuthCommand authCommand);
    }
}