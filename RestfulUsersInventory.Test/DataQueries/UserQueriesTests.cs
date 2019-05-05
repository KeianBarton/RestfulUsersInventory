using AutoMapper;
using RestfulUsersInventory.DataAccess;
using RestfulUsersInventory.DataAccess.Entities;
using RestfulUsersInventory.DataQueries.DTOs;
using RestfulUsersInventory.DataQueries.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RestfulUsersInventory.Test.DataQueries
{
    public class UserQueriesTests
    {
        private IMapper GetAutoMapperMapper()
        {
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }

        [Fact]
        public async Task GetUser_ShouldReturnNull_IfUserWithIdCannotBeFound()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                int userId = 1;
                var userQueries = new UserQueries(context, mapper);

                // Act
                UserDto user = await userQueries.GetUser(userId);

                // Assert
                Assert.Null(user);
            }
        }

        [Fact]
        public async Task GetUser_ShouldReturnUser_IfUserWithIdCanBeFound()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userQueries = new UserQueries(context, mapper);
                int userId = 1;
                string userName = "Rick Sanchez";
                context.Users.Add(new User { Id = userId, Name = userName });
                context.SaveChanges();

                // Act
                UserDto user = await userQueries.GetUser(userId);

                // Assert
                Assert.NotNull(user);
                Assert.Equal(userName, user.Name);
                Assert.Equal(userId, user.Id);
            }
        }

        [Fact]
        public async Task GetUsers_ShouldReturnAllUsers_IfTheyExist()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userQueries = new UserQueries(context, mapper);
                var user1 = new User { Id = 1, Name = "Person1" };
                var user2 = new User { Id = 2, Name = "Person2" };
                var user3 = new User { Id = 3, Name = "Person3" };
                context.Users.AddRange(user1, user2, user3);
                context.SaveChanges();

                // Act
                IEnumerable<UserDto> users = await userQueries.GetUsers();

                // Assert
                Assert.NotEmpty(users);
                Assert.Equal(3, users.Count());
                Assert.Equal(new List<int> { 1, 2, 3 }, users.Select(i => i.Id).ToList());
                Assert.Equal(new List<string> { "Person1", "Person2", "Person3" }, users.Select(i => i.Name).ToList());
            }
        }
    }
}
