using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using SalonApi.Controllers;
using Salon.Application.ServiceOrders.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.Models.Enums;
using Salon.Domain.ServiceOrders.Contracts;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SalonApi.Tests.Controllers
{
    [TestFixture]
    public class OrderControllerTest
    {
        private Mock<IOrderService> _mockOrderService;
        private OrderController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrderController(_mockOrderService.Object);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [TearDown]
        public void TearDown()
        {
            // Controllers don't implement IDisposable, so no cleanup needed
        }

        [Test]
        public async Task Create_Should_Return_Created_For_Valid_Order()
        {
            // Arrange
            var orderCommand = new ServiceOrderCommand
            {
                ClientId = ObjectId.GenerateNewId().ToString(),
                Date = System.DateTime.UtcNow,
                PaymentMethod = PaymentMethod.Cash,
                Obs = "Test order",
                Items = new List<ItemOrderDto>
                {
                    new ItemOrderDto { Id = ObjectId.GenerateNewId().ToString(), Value = 100.0M }
                }
            };

            var expectedResult = new Result(HttpStatusCode.Created);
            _mockOrderService.Setup(x => x.CreateOrder(orderCommand))
                            .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Create(orderCommand);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Test]
        public async Task GetAll_Should_Return_All_Orders()
        {
            // Arrange
            var orders = new List<OrderResponse>
            {
                new OrderResponse { Id = "1", ClientId = "client1", Date = System.DateTime.UtcNow },
                new OrderResponse { Id = "2", ClientId = "client2", Date = System.DateTime.UtcNow }
            };

            var expectedResult = new Result(orders, HttpStatusCode.OK);
            _mockOrderService.Setup(x => x.GetAllOrders())
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
        public async Task GetById_Should_Return_Order_For_Valid_Id()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId();
            var orderResponse = new OrderResponse 
            { 
                Id = orderId.ToString(), 
                ClientId = "client1", 
                Date = System.DateTime.UtcNow 
            };

            var expectedResult = new Result(orderResponse, HttpStatusCode.OK);
            _mockOrderService.Setup(x => x.GetOrderByIdAsync(orderId))
                            .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetById(orderId.ToString());

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
            var orderId = ObjectId.GenerateNewId();
            var expectedResult = new Result(HttpStatusCode.OK);
            _mockOrderService.Setup(x => x.DeleteOrder(orderId))
                            .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Remove(orderId.ToString());

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task GetOrdersFromClientId_Should_Return_Orders_For_Client()
        {
            // Arrange
            var clientId = ObjectId.GenerateNewId();
            var orders = new List<OrderResponse>
            {
                new OrderResponse { Id = "1", ClientId = clientId.ToString(), Date = System.DateTime.UtcNow }
            };

            var expectedResult = new Result(orders, HttpStatusCode.OK);
            _mockOrderService.Setup(x => x.GetOrdersByClientId(clientId))
                            .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetOrdersFromClientId(clientId.ToString());

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task Create_Should_Call_Order_Service()
        {
            // Arrange
            var orderCommand = new ServiceOrderCommand
            {
                ClientId = ObjectId.GenerateNewId().ToString(),
                Date = System.DateTime.UtcNow,
                PaymentMethod = PaymentMethod.Cash,
                Items = new List<ItemOrderDto>()
            };

            var expectedResult = new Result(HttpStatusCode.Created);
            _mockOrderService.Setup(x => x.CreateOrder(orderCommand))
                            .ReturnsAsync(expectedResult);

            // Act
            await _controller.Create(orderCommand);

            // Assert
            _mockOrderService.Verify(x => x.CreateOrder(orderCommand), Times.Once);
        }

        [Test]
        public async Task GetAll_Should_Call_Order_Service()
        {
            // Arrange
            var expectedResult = new Result(new List<OrderResponse>(), HttpStatusCode.OK);
            _mockOrderService.Setup(x => x.GetAllOrders())
                            .ReturnsAsync(expectedResult);

            // Act
            await _controller.GetAll();

            // Assert
            _mockOrderService.Verify(x => x.GetAllOrders(), Times.Once);
        }

        [Test]
        public async Task GetById_Should_Call_Order_Service()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId();
            var expectedResult = new Result(new OrderResponse(), HttpStatusCode.OK);
            _mockOrderService.Setup(x => x.GetOrderByIdAsync(orderId))
                            .ReturnsAsync(expectedResult);

            // Act
            await _controller.GetById(orderId.ToString());

            // Assert
            _mockOrderService.Verify(x => x.GetOrderByIdAsync(orderId), Times.Once);
        }

        [Test]
        public async Task Remove_Should_Call_Order_Service()
        {
            // Arrange
            var orderId = ObjectId.GenerateNewId();
            var expectedResult = new Result(HttpStatusCode.OK);
            _mockOrderService.Setup(x => x.DeleteOrder(orderId))
                            .ReturnsAsync(expectedResult);

            // Act
            await _controller.Remove(orderId.ToString());

            // Assert
            _mockOrderService.Verify(x => x.DeleteOrder(orderId), Times.Once);
        }

        [Test]
        public async Task GetOrdersFromClientId_Should_Call_Order_Service()
        {
            // Arrange
            var clientId = ObjectId.GenerateNewId();
            var expectedResult = new Result(new List<OrderResponse>(), HttpStatusCode.OK);
            _mockOrderService.Setup(x => x.GetOrdersByClientId(clientId))
                            .ReturnsAsync(expectedResult);

            // Act
            await _controller.GetOrdersFromClientId(clientId.ToString());

            // Assert
            _mockOrderService.Verify(x => x.GetOrdersByClientId(clientId), Times.Once);
        }
    }
}