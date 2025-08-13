using Microsoft.Extensions.DependencyInjection;

namespace Salon.Application.Tests.Modules
{
    public interface IModule
    {
        ServiceCollection Register(ServiceCollection service);
    }
}
