using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using SalonApi.Controllers;
using Salon.Application.Clients.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.Clients.Contracts;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SalonApi.Tests.Controllers
{
    [TestFixture]
    public class ClientControllerTest
    {
        private Mock<IClientService> _mockClientService;
        private ClientController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockClientService = new Mock<IClientService>();
            _controller = new ClientController(_mockClientService.Object);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [TearDown]
        public void TearDown()
        {
            // Controllers don't implement IDisposable, so no cleanup needed
        }

        [Test]
        public async Task Create_Should_Return_Created_For_Valid_Client()
        {
            // Arrange
            var clientCommand = new ClientCommand
            {
                Name = "Test Client",
                Email = "test@example.com",
                ContactNumbers = new List<string> { "123456789" }
            };

            var expectedResult = new Result(HttpStatusCode.Created);
            _mockClientService.Setup(x => x.CreateClient(clientCommand))
                             .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Create(clientCommand);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Test]
        public async Task GetAll_Should_Return_All_Clients()
        {
            // Arrange
            var clients = new List<ClientResponse>
            {
                new ClientResponse { Id = "1", Name = "Client1", Email = "client1@test.com" },
                new ClientResponse { Id = "2", Name = "Client2", Email = "client2@test.com" }
            };

            var expectedResult = new Result(clients, HttpStatusCode.OK);
            _mockClientService.Setup(x => x.GetAllClients(null))
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
        public async Task GetAll_With_Amount_Should_Return_Limited_Clients()
        {
            // Arrange
            var clients = new List<ClientResponse>
            {
                new ClientResponse { Id = "1", Name = "Client1", Email = "client1@test.com" }
            };

            var expectedResult = new Result(clients, HttpStatusCode.OK);
            _mockClientService.Setup(x => x.GetAllClients(1))
                             .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetAll(1);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task GetById_Should_Return_Client_For_Valid_Id()
        {
            // Arrange
            var clientId = ObjectId.GenerateNewId();
            var clientResponse = new ClientResponse 
            { 
                Id = clientId.ToString(), 
                Name = "Test Client", 
                Email = "test@example.com" 
            };

            var expectedResult = new Result(clientResponse, HttpStatusCode.OK);
            _mockClientService.Setup(x => x.GetClientById(clientId))
                             .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetById(clientId.ToString());

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
            var updateCommand = new UpdateClientCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Updated Name",
                Email = "updated@example.com"
            };

            var expectedResult = new Result(HttpStatusCode.OK);
            _mockClientService.Setup(x => x.UpdateClient(updateCommand))
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
            var clientId = ObjectId.GenerateNewId();
            var expectedResult = new Result(HttpStatusCode.OK);
            _mockClientService.Setup(x => x.DeleteClient(clientId))
                             .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Remove(clientId.ToString());

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task Create_Should_Call_Client_Service()
        {
            // Arrange
            var clientCommand = new ClientCommand
            {
                Name = "Test Client",
                Email = "test@example.com",
                ContactNumbers = new List<string> { "123456789" }
            };

            var expectedResult = new Result(HttpStatusCode.Created);
            _mockClientService.Setup(x => x.CreateClient(clientCommand))
                             .ReturnsAsync(expectedResult);

            // Act
            await _controller.Create(clientCommand);

            // Assert
            _mockClientService.Verify(x => x.CreateClient(clientCommand), Times.Once);
        }
    }
}