using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using RestfulUsersInventory.Api.Controllers;
using RestfulUsersInventory.DataQueries;
using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RestfulUsersInventory.Test.Controllers
{
    public class ItemsControllerTests
    {
        [Fact]
        public async Task GetItems_ShouldReturnNotFound_IfNoItemsFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new ItemsController(mockQueryHelper.Object);
            mockQueryHelper.Setup(q => q.ItemQueries.GetItems())
                .ReturnsAsync(new List<ItemDto>());

            // Act
            IActionResult result = await controller.GetItems();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No items exist", notFoundResult.Value);
        }

        [Fact]
        public async Task GetItems_ShouldReturnItems_IfItemsFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new ItemsController(mockQueryHelper.Object);
            var expectedItems = new List<ItemDto>
            {
                new ItemDto { Id = 1, Name = "Shortsword" },
                new ItemDto { Id = 2, Name = "Large battery" }
            };
            mockQueryHelper.Setup(q => q.ItemQueries.GetItems())
                .ReturnsAsync(expectedItems);

            // Act
            IActionResult result = await controller.GetItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal
            (
                JsonConvert.SerializeObject(expectedItems),
                JsonConvert.SerializeObject(okResult.Value)
            );
        }

        [Theory]
        [InlineData(0)]
        public async Task GetItem_ShouldReturnNotFound_IfNoItemFound(int itemId)
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new ItemsController(mockQueryHelper.Object);
            mockQueryHelper.Setup(q => q.ItemQueries.GetItem(itemId))
                .ReturnsAsync((ItemDto) null);

            // Act
            IActionResult result = await controller.GetItem(itemId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No items found for the requested ID", notFoundResult.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetItem_ShouldReturnItem_IfItemFound(int itemId)
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new ItemsController(mockQueryHelper.Object);
            var expectedItem = new ItemDto { Id = 1, Name = "Shortsword" };
            mockQueryHelper.Setup(q => q.ItemQueries.GetItem(itemId))
                .ReturnsAsync(expectedItem);

            // Act
            IActionResult result = await controller.GetItem(itemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal
            (
                JsonConvert.SerializeObject(expectedItem),
                JsonConvert.SerializeObject(okResult.Value)
            );
        }
    }
}
