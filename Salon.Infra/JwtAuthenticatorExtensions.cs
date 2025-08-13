using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Salon.Infra
{
    public static class JwtAuthenticatorExtensions
    {
        public static IServiceCollection AuthenticationConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:JwtSecret"]);
            serviceCollection.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = false
                };
            });

            return serviceCollection;
        }
    }
}

