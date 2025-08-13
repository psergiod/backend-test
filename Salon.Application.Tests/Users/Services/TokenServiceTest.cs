using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Salon.Application.Tests.Users.Fakes;
using Salon.Application.Users.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Salon.Application.Tests.Users.Services
{
    [TestFixture]
    public class TokenServiceTest
    {
        private IConfiguration _configuration;
        private TokenService _tokenService;

        [SetUp]
        public void SetUp()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:JwtSecret", "TestSecretKeyFor256BitHMacSha256!"},
                {"Jwt:Issuer", "https://localhost:5001"},
                {"Jwt:Audience", "https://localhost:5001"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _tokenService = new TokenService(_configuration);
        }

        [Test]
        public void Should_Generate_Valid_JWT_Token()
        {
            // Arrange
            var user = UserFake.GetUserJames();

            // Act
            var token = _tokenService.GenerateToken(user);

            // Assert
            token.Should().NotBeNullOrEmpty();
            
            // Verify that the token can be parsed without throwing an exception
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);
            
            jsonToken.Should().NotBeNull();
            jsonToken.Claims.Should().NotBeEmpty();
        }

        [Test]
        public void Should_Generate_Token_With_User_Claims()
        {
            // Arrange
            var user = UserFake.GetUserJames();

            // Act
            var token = _tokenService.GenerateToken(user);

            // Assert
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);

            var claims = jsonToken.Claims;
            claims.Should().Contain(c => c.Type == "Id" && c.Value == user.Id.ToString());
            claims.Should().Contain(c => c.Type == "Name" && c.Value == user.Name);
            claims.Should().Contain(c => c.Type == "Email" && c.Value == user.Email);
            claims.Should().Contain(c => c.Type == "Login" && c.Value == user.Login);
            claims.Should().Contain(c => c.Type == "Role" && c.Value == ((int)user.Role).ToString());
        }
    }
}