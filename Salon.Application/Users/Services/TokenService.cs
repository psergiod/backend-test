using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Salon.Application.Users.Interfaces;
using Salon.Domain.Users.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Salon.Application.Users.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:JwtSecret"]);
            var userRole = (int)user.Role;
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(nameof(User.Id), user.Id.ToString()),
                    new Claim(nameof(User.Name), user.Name),
                    new Claim(nameof(User.Email), user.Email),
                    new Claim(nameof(User.Login), user.Login),
                    new Claim(nameof(User.Role),userRole.ToString())
                }),
                Expires = DateTime.UtcNow.AddYears(1), // ver sobre refresh token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
