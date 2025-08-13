using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using SalonApi.Controllers;
using Salon.Application.Users.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.Users.Contracts;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SalonApi.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        private Mock<IUserService> _mockUserService;
        private UserController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [TearDown]
        public void TearDown()
        {
            // Controllers don't implement IDisposable, so no cleanup needed
        }

        [Test]
        public async Task Create_Should_Return_Created_For_Valid_User()
        {
            // Arrange
            var userCommand = new UserCommand
            {
                Login = "testuser",
                Password = "testpassword",
                Name = "Test User",
                Email = "test@example.com"
            };

            var expectedResult = new Result(HttpStatusCode.Created);
            _mockUserService.Setup(x => x.CreateUser(userCommand))
                           .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Create(userCommand);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Test]
        public async Task GetAll_Should_Return_All_Users()
        {
            // Arrange
            var users = new List<UserResponse>
            {
                new UserResponse { Id = "1", Name = "User1", Email = "user1@test.com" },
                new UserResponse { Id = "2", Name = "User2", Email = "user2@test.com" }
            };

            var expectedResult = new Result(users, HttpStatusCode.OK);
            _mockUserService.Setup(x => x.GetAllUsers())
                           .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task GetById_Should_Return_User_For_Valid_Id()
        {
            // Arrange
            var userId = ObjectId.GenerateNewId();
            var userResponse = new UserResponse 
            { 
                Id = userId.ToString(), 
                Name = "Test User", 
                Email = "test@example.com" 
            };

            var expectedResult = new Result(userResponse, HttpStatusCode.OK);
            _mockUserService.Setup(x => x.GetUserByIdAsync(userId))
                           .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetById(userId.ToString());

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task Update_Should_Return_Success_For_Valid_Update()
        {
            // Arrange
            var updateCommand = new UpdateUserCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Updated Name",
                Email = "updated@example.com"
            };

            var expectedResult = new Result(HttpStatusCode.OK);
            _mockUserService.Setup(x => x.UpdateUser(updateCommand))
                           .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(updateCommand);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task Remove_Should_Return_Success_For_Valid_Id()
        {
            // Arrange
            var userId = ObjectId.GenerateNewId();
            var expectedResult = new Result(HttpStatusCode.OK);
            _mockUserService.Setup(x => x.DeleteUser(userId))
                           .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Remove(userId.ToString());

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task Create_Should_Call_User_Service()
        {
            // Arrange
            var userCommand = new UserCommand
            {
                Login = "testuser",
                Password = "testpassword",
                Name = "Test User",
                Email = "test@example.com"
            };

            var expectedResult = new Result(HttpStatusCode.Created);
            _mockUserService.Setup(x => x.CreateUser(userCommand))
                           .ReturnsAsync(expectedResult);

            // Act
            await _controller.Create(userCommand);

            // Assert
            _mockUserService.Verify(x => x.CreateUser(userCommand), Times.Once);
        }
    }
}