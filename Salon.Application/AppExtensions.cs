using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Salon.Application.Clients.Interfaces;
using Salon.Application.Clients.Mappers;
using Salon.Application.Clients.Services;
using Salon.Application.Clients.Validators;
using Salon.Application.ServiceOrders.Interfaces;
using Salon.Application.ServiceOrders.Mappers;
using Salon.Application.ServiceOrders.Services;
using Salon.Application.ServiceOrders.Validators;
using Salon.Application.Users.Interfaces;
using Salon.Application.Users.Mappers;
using Salon.Application.Users.Services;
using Salon.Application.Users.Validators;
using Salon.Domain.Clients.Contracts;
using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.Users.Contracts;

namespace Salon.Application
{
    public static class AppExtensions
    {
        public static IServiceCollection AddAppDependencyInjection(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IClientService, ClientService>();
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IOrderService, OrderService>();
            serviceCollection.AddTransient<IItemService, ItemService>();
            serviceCollection.AddTransient<ITokenService, TokenService>();
            serviceCollection.AddTransient<IUserAuthenticationService, UserAuthenticationService>();

            serviceCollection.AddTransient<IClientMapper, ClientMapper>();
            serviceCollection.AddTransient<IUserMapper, UserMapper>();
            serviceCollection.AddTransient<IServiceOrderMapper, ServiceOrderMapper>();
            serviceCollection.AddTransient<IItemMapper, ItemMapper>();

            serviceCollection.AddTransient<IValidator<ClientCommand>, ClientCommandValidator>();
            serviceCollection.AddTransient<IValidator<UpdateClientCommand>, UpdateClientCommandValidator>();
            serviceCollection.AddTransient<IValidator<UserCommand>, UserCommandValidator>();
            serviceCollection.AddTransient<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
            serviceCollection.AddTransient<IValidator<ServiceOrderCommand>, ServiceOrderCommandValidator>();

            serviceCollection.AddTransient<IClientServiceBuilder, ClientServiceBuilder>();
            serviceCollection.AddTransient<IUserServiceBuilder, UserServiceBuilder>();
            serviceCollection.AddTransient<IOrderServiceBuilder, OrderServiceBuilder>();

            return serviceCollection;
        }
    }
}
