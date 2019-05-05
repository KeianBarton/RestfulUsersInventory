using Microsoft.AspNetCore.Mvc;
using Moq;
using RestfulUsersInventory.Api.Controllers;
using RestfulUsersInventory.DataQueries;
using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RestfulUsersInventory.Test.Controllers
{
    public class UserItemsControllerTests
    {
        [Fact]
        public async Task GetItemForUser_ShouldReturnBadRequest_IfRequestItemIdIsInvalid()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int itemId = 0;
            int userId = 1;

            // Act
            IActionResult result = await controller.GetItemForUser(userId, itemId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Item in request is malformed / has invalid IDs", badRequestResult.Value);
        }

        [Fact]
        public async Task GetItemForUser_ShouldReturnBadRequest_IfRequestUserIdIsInvalid()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int itemId = 1;
            int userId = 0;

            // Act
            IActionResult result = await controller.GetItemForUser(userId, itemId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Item in request is malformed / has invalid IDs", badRequestResult.Value);
        }

        [Fact]
        public async Task GetItemForUser_ShouldReturnNotFound_IfRequestUserCannotBeFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int itemId = 1;
            int userId = 1;
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(userId))
                .ReturnsAsync((UserDto)null);

            // Act
            IActionResult result = await controller.GetItemForUser(userId, itemId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"No user can be found with ID {userId}", notFoundResult.Value);
        }

        [Fact]
        public async Task GetItemForUser_ShouldReturnNotFound_IfMatchingItemsCannotBeFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int userId = 1;
            int itemId = 1;
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(userId))
                .ReturnsAsync(new UserDto());
            mockQueryHelper.Setup(q => q.UserItemQueries
                .GetMatchingItemsForUser(userId, It.Is<ItemDto>(i => i.Id == itemId )))
                .ReturnsAsync((UserItemDto)null);

            // Act
            IActionResult result = await controller.GetItemForUser(userId, itemId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"No items matched for request for user with ID {userId}", notFoundResult.Value);
        }

        [Fact]
        public async Task GetItemForUser_ShouldReturnResult_IfMatchingItemsCanBeFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int userId = 1;
            int itemId = 1;
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(userId))
                .ReturnsAsync(new UserDto());
            var expectedUserItem = new UserItemDto();
            mockQueryHelper.Setup(q => q.UserItemQueries
                .GetMatchingItemsForUser(userId, It.Is<ItemDto>(i => i.Id == itemId)))
                .ReturnsAsync(expectedUserItem);

            // Act
            IActionResult result = await controller.GetItemForUser(userId, itemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedUserItem, okResult.Value);
        }

        [Fact]
        public async Task GetItemsForUser_ShouldReturnNotFound_IfRequestUserCannotBeFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int userId = 1;
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(userId))
                .ReturnsAsync((UserDto)null);

            // Act
            IActionResult result = await controller.GetItemsForUser(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"No user can be found with ID {userId}", notFoundResult.Value);
        }

        [Fact]
        public async Task GetItemsForUser_ShouldReturnNotFound_IfNoItemsCanBeFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int userId = 1;
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(userId))
                .ReturnsAsync(new UserDto());
            mockQueryHelper.Setup(q => q.UserItemQueries
                .GetAllItemsForUser(userId))
                .ReturnsAsync(new List<UserItemDto>());

            // Act
            IActionResult result = await controller.GetItemsForUser(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"No items found for user with ID {userId}", notFoundResult.Value);
        }

        [Fact]
        public async Task GetItemsForUser_ShouldReturnResult_IfItemsCanBeFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int userId = 1;
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(userId))
                .ReturnsAsync(new UserDto());
            var expectedUserItems = new List<UserItemDto>
            {
                new UserItemDto()
            };
            mockQueryHelper.Setup(q => q.UserItemQueries
                .GetAllItemsForUser(userId))
                .ReturnsAsync(expectedUserItems);

            // Act
            IActionResult result = await controller.GetItemsForUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedUserItems, okResult.Value);
        }

        [Fact]
        public async Task AddItemsForUser_ShouldReturnResult_IfItemsCannotBeParsed()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);

            // Act
            IActionResult result = await controller.AddItemsForUser(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Items in request body could not be parsed", badRequestResult.Value);
        }

        [Fact]
        public async Task AddItemsForUser_ShouldReturnResult_IfNoItemsSentInRequest()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);

            // Act
            IActionResult result = await controller.AddItemsForUser(1, new List<ItemDto>());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Items in request body could not be parsed", badRequestResult.Value);
        }

        [Fact]
        public async Task AddItemsForUser_ShouldReturnNotFound_IfRequestUserCannotBeFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int userId = 1;
            var items = new List<ItemDto>() { new ItemDto() };
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(userId))
                .ReturnsAsync((UserDto)null);

            // Act
            IActionResult result = await controller.AddItemsForUser(userId, items);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"No user can be found with ID {userId}", notFoundResult.Value);
        }

        [Fact]
        public async Task AddItemsForUser_ShouldReturnLatestInventory_IfAllArgumentsValid()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int userId = 1;
            var items = new List<ItemDto>() { new ItemDto() };
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(userId))
                .ReturnsAsync(new UserDto());
            mockQueryHelper.Setup(q => q.UserItemQueries
                .AddItemForUser(userId, It.IsAny<ItemDto>()))
                .Returns(Task.CompletedTask);
            var expectedLatestInventory = new List<UserItemDto>();
            mockQueryHelper.Setup(q => q.UserItemQueries.GetAllItemsForUser(userId))
                .ReturnsAsync(expectedLatestInventory);

            // Act
            IActionResult result = await controller.AddItemsForUser(userId, items);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedLatestInventory, okResult.Value);
        }

        [Fact]
        public async Task RemoveItemsForUser_ShouldReturnResult_IfItemsCannotBeParsed()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);

            // Act
            IActionResult result = await controller.RemoveItemsForUser(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Items in request body could not be parsed", badRequestResult.Value);
        }

        [Fact]
        public async Task RemoveItemsForUser_ShouldReturnResult_IfNoItemsSentInRequest()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);

            // Act
            IActionResult result = await controller.RemoveItemsForUser(1, new List<ItemDto>());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Items in request body could not be parsed", badRequestResult.Value);
        }

        [Fact]
        public async Task RemoveItemsForUser_ShouldReturnNotFound_IfRequestUserCannotBeFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int userId = 1;
            var items = new List<ItemDto>() { new ItemDto() };
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(userId))
                .ReturnsAsync((UserDto)null);

            // Act
            IActionResult result = await controller.RemoveItemsForUser(userId, items);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"No user can be found with ID {userId}", notFoundResult.Value);
        }

        [Fact]
        public async Task RemoveItemsForUser_ShouldReturnLatestInventory_IfAllArgumentsValid()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UserItemsController(mockQueryHelper.Object);
            int userId = 1;
            var items = new List<ItemDto>() { new ItemDto() };
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(userId))
                .ReturnsAsync(new UserDto());
            mockQueryHelper.Setup(q => q.UserItemQueries
                .RemoveItemForUser(userId, It.IsAny<ItemDto>()))
                .Returns(Task.CompletedTask);
            var expectedLatestInventory = new List<UserItemDto>();
            mockQueryHelper.Setup(q => q.UserItemQueries.GetAllItemsForUser(userId))
                .ReturnsAsync(expectedLatestInventory);

            // Act
            IActionResult result = await controller.RemoveItemsForUser(userId, items);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedLatestInventory, okResult.Value);
        }
    }
}
