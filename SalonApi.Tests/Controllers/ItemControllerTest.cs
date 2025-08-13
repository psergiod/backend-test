using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using SalonApi.Controllers;
using Salon.Application.ServiceOrders.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.ServiceOrders.Contracts;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SalonApi.Tests.Controllers
{
    [TestFixture]
    public class ItemControllerTest
    {
        private Mock<IItemService> _mockItemService;
        private ItemController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockItemService = new Mock<IItemService>();
            _controller = new ItemController(_mockItemService.Object);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [TearDown]
        public void TearDown()
        {
            // Controllers don't implement IDisposable, so no cleanup needed
        }

        [Test]
        public async Task Create_Should_Return_Created_For_Valid_Item()
        {
            // Arrange
            var itemCommand = new ItemCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Description = "Test Description"
            };

            var expectedResult = new Result(HttpStatusCode.Created);
            _mockItemService.Setup(x => x.CreateItem(itemCommand))
                           .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Create(itemCommand);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Test]
        public async Task GetAll_Should_Return_All_Items()
        {
            // Arrange
            var items = new List<ItemResponse>
            {
                new ItemResponse { Id = "1", Description = "Item1" },
                new ItemResponse { Id = "2", Description = "Item2" }
            };

            var expectedResult = new Result(items, HttpStatusCode.OK);
            _mockItemService.Setup(x => x.GetAllAsync())
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
        public async Task GetById_Should_Return_Item_For_Valid_Id()
        {
            // Arrange
            var itemId = ObjectId.GenerateNewId();
            var itemResponse = new ItemResponse 
            { 
                Id = itemId.ToString(), 
                Description = "Test Item" 
            };

            var expectedResult = new Result(itemResponse, HttpStatusCode.OK);
            _mockItemService.Setup(x => x.GetItemById(itemId))
                           .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetById(itemId.ToString());

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
            var itemId = ObjectId.GenerateNewId();
            var expectedResult = new Result(HttpStatusCode.OK);
            _mockItemService.Setup(x => x.RemoveItem(itemId))
                           .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Remove(itemId.ToString());

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            jsonResult!.Value.Should().Be(expectedResult);
            _controller.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task Create_Should_Call_Item_Service()
        {
            // Arrange
            var itemCommand = new ItemCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Description = "Test Description"
            };

            var expectedResult = new Result(HttpStatusCode.Created);
            _mockItemService.Setup(x => x.CreateItem(itemCommand))
                           .ReturnsAsync(expectedResult);

            // Act
            await _controller.Create(itemCommand);

            // Assert
            _mockItemService.Verify(x => x.CreateItem(itemCommand), Times.Once);
        }

        [Test]
        public async Task GetAll_Should_Call_Item_Service()
        {
            // Arrange
            var expectedResult = new Result(new List<ItemResponse>(), HttpStatusCode.OK);
            _mockItemService.Setup(x => x.GetAllAsync())
                           .ReturnsAsync(expectedResult);

            // Act
            await _controller.GetAll();

            // Assert
            _mockItemService.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetById_Should_Call_Item_Service()
        {
            // Arrange
            var itemId = ObjectId.GenerateNewId();
            var expectedResult = new Result(new ItemResponse(), HttpStatusCode.OK);
            _mockItemService.Setup(x => x.GetItemById(itemId))
                           .ReturnsAsync(expectedResult);

            // Act
            await _controller.GetById(itemId.ToString());

            // Assert
            _mockItemService.Verify(x => x.GetItemById(itemId), Times.Once);
        }

        [Test]
        public async Task Remove_Should_Call_Item_Service()
        {
            // Arrange
            var itemId = ObjectId.GenerateNewId();
            var expectedResult = new Result(HttpStatusCode.OK);
            _mockItemService.Setup(x => x.RemoveItem(itemId))
                           .ReturnsAsync(expectedResult);

            // Act
            await _controller.Remove(itemId.ToString());

            // Assert
            _mockItemService.Verify(x => x.RemoveItem(itemId), Times.Once);
        }
    }
}