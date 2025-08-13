using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Salon.Application.Tests.Users.Fakes;
using Salon.Application.Users.Interfaces;
using Salon.Application.Users.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Salon.Application.Tests.Users.Services
{
    [TestFixture]
    public class TokenServiceTest
    {
        private ITokenService _tokenService;

        [SetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Jwt:JwtSecret", "SuperSecretKeyForJwtTokenGenerationThatIsAtLeast256BitsLong"}
                })
                .Build();

            _tokenService = new TokenService(configuration);
        }

        [Test]
        public void Should_Generate_Valid_JWT_Token()
        {
            var user = UserFake.GetUserRobert();

            var token = _tokenService.GenerateToken(user);

            token.Should().NotBeNullOrEmpty();
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var isValidToken = tokenHandler.CanReadToken(token);
            isValidToken.Should().BeTrue();
        }

        [Test]
        public void Should_Include_User_Claims_In_Token()
        {
            var user = UserFake.GetUserRobert();

            var token = _tokenService.GenerateToken(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            jwtToken.Claims.Should().Contain(c => c.Type == "Id" && c.Value == user.Id.ToString());
            jwtToken.Claims.Should().Contain(c => c.Type == "Name" && c.Value == user.Name);
            jwtToken.Claims.Should().Contain(c => c.Type == "Email" && c.Value == user.Email);
            jwtToken.Claims.Should().Contain(c => c.Type == "Login" && c.Value == user.Login);
            jwtToken.Claims.Should().Contain(c => c.Type == "Role" && c.Value == ((int)user.Role).ToString());
        }

        [Test]
        public void Should_Set_Token_Expiration_To_One_Year()
        {
            var user = UserFake.GetUserRobert();

            var token = _tokenService.GenerateToken(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var expectedExpiry = DateTime.UtcNow.AddYears(1);
            jwtToken.ValidTo.Should().BeCloseTo(expectedExpiry, TimeSpan.FromMinutes(1));
        }

        [Test]
        public void Should_Generate_Different_Tokens_For_Different_Users()
        {
            var user1 = UserFake.GetUserRobert();
            var user2 = UserFake.GetUserTony();

            var token1 = _tokenService.GenerateToken(user1);
            var token2 = _tokenService.GenerateToken(user2);

            token1.Should().NotBe(token2);
        }
    }
}