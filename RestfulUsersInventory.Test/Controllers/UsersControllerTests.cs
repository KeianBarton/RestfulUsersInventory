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
    public class UsersControllerTests
    {
        [Fact]
        public async Task GetUsers_ShouldReturnNotFound_IfNoUsersFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UsersController(mockQueryHelper.Object);
            mockQueryHelper.Setup(q => q.UserQueries.GetUsers())
                .ReturnsAsync(new List<UserDto>());

            // Act
            IActionResult result = await controller.GetUsers();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No users exist", notFoundResult.Value);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnUsers_IfUsersFound()
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UsersController(mockQueryHelper.Object);
            var expectedUsers = new List<UserDto>
            {
                new UserDto { Id = 1, Name = "Shortsword" },
                new UserDto { Id = 2, Name = "Large battery" }
            };
            mockQueryHelper.Setup(q => q.UserQueries.GetUsers())
                .ReturnsAsync(expectedUsers);

            // Act
            IActionResult result = await controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal
            (
                JsonConvert.SerializeObject(expectedUsers),
                JsonConvert.SerializeObject(okResult.Value)
            );
        }

        [Theory]
        [InlineData(0)]
        public async Task GetUser_ShouldReturnNotFound_IfNoUserFound(int UserId)
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UsersController(mockQueryHelper.Object);
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(UserId))
                .ReturnsAsync((UserDto)null);

            // Act
            IActionResult result = await controller.GetUser(UserId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No users found for the requested ID", notFoundResult.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetUser_ShouldReturnUser_IfUserFound(int UserId)
        {
            // Arrange
            var mockQueryHelper = new Mock<IQueryHelper>(MockBehavior.Strict);
            var controller = new UsersController(mockQueryHelper.Object);
            var expectedUser = new UserDto { Id = 1, Name = "Shortsword" };
            mockQueryHelper.Setup(q => q.UserQueries.GetUser(UserId))
                .ReturnsAsync(expectedUser);

            // Act
            IActionResult result = await controller.GetUser(UserId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal
            (
                JsonConvert.SerializeObject(expectedUser),
                JsonConvert.SerializeObject(okResult.Value)
            );
        }
    }
}
