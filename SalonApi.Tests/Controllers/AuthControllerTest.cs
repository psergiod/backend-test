using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SalonApi.Controllers;
using Salon.Application.Users.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.Users.Contracts;
using System.Net;
using System.Threading.Tasks;

namespace SalonApi.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTest
    {
        private Mock<IUserAuthenticationService> _mockAuthService;
        private AuthController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockAuthService = new Mock<IUserAuthenticationService>();
            _controller = new AuthController(_mockAuthService.Object);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [TearDown]
        public void TearDown()
        {
            // Controllers don't implement IDisposable, so no cleanup needed
        }

        [Test]
        public async Task Authenticate_Should_Return_Success_For_Valid_Credentials()
        {
            // Arrange
            var authCommand = new AuthCommand
            {
                Login = "testuser",
                Password = "testpassword"
            };

            var expectedResult = new Result("fake-jwt-token");
            _mockAuthService.Setup(x => x.Authenticate(authCommand))
                           .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Authenticate(authCommand);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task Authenticate_Should_Return_Error_For_Invalid_Credentials()
        {
            // Arrange
            var authCommand = new AuthCommand
            {
                Login = "testuser",
                Password = "wrongpassword"
            };

            var expectedResult = new Result("Username or Password is incorrect!");
            expectedResult.Error = true;
            expectedResult.StatusCode = (int)HttpStatusCode.BadRequest;

            _mockAuthService.Setup(x => x.Authenticate(authCommand))
                           .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Authenticate(authCommand);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be(expectedResult.StatusCode.Value);
        }

        [Test]
        public async Task Authenticate_Should_Call_Authentication_Service()
        {
            // Arrange
            var authCommand = new AuthCommand
            {
                Login = "testuser",
                Password = "testpassword"
            };

            var expectedResult = new Result("fake-jwt-token");
            _mockAuthService.Setup(x => x.Authenticate(authCommand))
                           .ReturnsAsync(expectedResult);

            // Act
            await _controller.Authenticate(authCommand);

            // Assert
            _mockAuthService.Verify(x => x.Authenticate(authCommand), Times.Once);
        }
    }
}