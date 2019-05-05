using AutoMapper;
using Newtonsoft.Json;
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
    public class UserItemQueriesTests
    {
        private IMapper GetAutoMapperMapper()
        {
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }

        [Fact]
        public async Task DoesUserHaveItem_ShouldReturnFalse_IfNoUserItemCanBeMatched()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);

                // Act
                bool matchFound = await userItemQueries
                    .DoesUserHaveItem(1, new ItemDto { Id = 1, Name = "ExampleItem" });

                // Assert
                Assert.False(matchFound);
            }
        }

        [Fact]
        public async Task DoesUserHaveItem_ShouldReturnTrue_IfCanBeMatchedOnUserAndItemId()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                int userId = 1;
                int itemId = 1;
                context.Add(new Item { Id = itemId, Name = "Irrelevant" });
                context.Add(new UserItem { ItemId = itemId, UserId = userId });
                context.SaveChanges();
                var userItemQueries = new UserItemQueries(context, mapper);

                // Act
                bool matchFound = await userItemQueries
                    .DoesUserHaveItem(userId, new ItemDto { Id = itemId });

                // Assert
                Assert.True(matchFound);
            }
        }

        [Fact]
        public async Task DoesUserHaveItem_ShouldReturnTrue_IfCanBeMatchedOnUserAndItemName()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                int userId = 1;
                int itemId = 1;
                string itemName = "Longsword";
                context.Add(new Item { Id = itemId, Name = itemName });
                context.Add(new UserItem { ItemId = itemId, UserId = userId });
                context.SaveChanges();
                var userItemQueries = new UserItemQueries(context, mapper);

                // Act
                bool matchFound = await userItemQueries
                    .DoesUserHaveItem(userId, new ItemDto { Name = itemName });

                // Assert
                Assert.True(matchFound);
            }
        }

        [Fact]
        public async Task GetAllItemsForUser_ShouldReturnEmptyList_IfUserCannotBeMatchedOrDataDoesntExist()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);

                // Act
                IEnumerable<UserItemDto> userItems = await userItemQueries.GetAllItemsForUser(1);

                // Assert
                Assert.Empty(userItems);
            }
        }

        [Fact]
        public async Task GetAllItemsForUser_ShouldReturnListOfUserItemsWithCorrectCounts_IfDataCanBeMatched()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                int userId = 1;
                string userName = "Rick Sanchez";
                context.Add(new User { Id = userId, Name = userName });

                int numberOfSwords = 46;
                int swordId = 1;
                string swordName = "Longsword";
                context.Add(new Item { Id = swordId, Name = swordName });

                int numberOfBatteries = 3;
                int batteryId = 2;
                string batteryName = "Large Battery";
                context.Add(new Item { Id = batteryId, Name = batteryName });

                for (int i = 0; i < numberOfSwords; i++)
                {
                    context.Add(new UserItem { ItemId = swordId, UserId = userId });
                }
                for (int i = 0; i < numberOfBatteries; i++)
                {
                    context.Add(new UserItem { ItemId = batteryId, UserId = userId });
                }

                context.SaveChanges();

                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);

                // Act
                IEnumerable<UserItemDto> actualUserItems = await userItemQueries.GetAllItemsForUser(userId);

                // Assert
                var expectedUserItems = new List<UserItemDto>
                {
                    new UserItemDto
                    {
                        UserId = userId,
                        UserName = userName,
                        ItemCount = numberOfSwords,
                        ItemId = swordId,
                        ItemName = swordName
                    },
                    new UserItemDto
                    {
                        UserId = userId,
                        UserName = userName,
                        ItemCount = numberOfBatteries,
                        ItemId = batteryId,
                        ItemName = batteryName
                    }
                };
                Assert.Equal
                (
                    JsonConvert.SerializeObject(expectedUserItems),
                    JsonConvert.SerializeObject(actualUserItems)
                );
            }
        }

        [Fact]
        public async Task GetMatchingItemsForUser_ShouldReturnNull_IfUserCannotBeMatchedOrDataDoesntExist()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);

                // Act
                UserItemDto userItem = await userItemQueries
                    .GetMatchingItemsForUser(1, new ItemDto { Id = 1, Name = "Weapon" });

                // Assert
                Assert.Null(userItem);
            }
        }

        [Fact]
        public async Task GetMatchingItemsForUser_ShouldReturnUserItem_IfUserAndItemIdCanBeMatched()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                int userId = 1;
                string userName = "Rick Sanchez";
                context.Add(new User { Id = userId, Name = userName });

                int numberOfSwords = 46;
                int swordId = 1;
                string swordName = "Longsword";
                context.Add(new Item { Id = swordId, Name = swordName });

                for (int i = 0; i < numberOfSwords; i++)
                {
                    context.Add(new UserItem { ItemId = swordId, UserId = userId });
                }

                context.SaveChanges();

                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);

                // Act
                UserItemDto userItem = await userItemQueries
                    .GetMatchingItemsForUser(userId, new ItemDto { Id = swordId });

                // Assert
                Assert.NotNull(userItem);
                Assert.Equal(userId, userItem.UserId);
                Assert.Equal(userName, userItem.UserName);
                Assert.Equal(numberOfSwords, userItem.ItemCount);
                Assert.Equal(swordId, userItem.ItemId);
                Assert.Equal(swordName, userItem.ItemName);
            }
        }

        [Fact]
        public async Task AddItemForUser_ShouldDoNothing_IfItemCannotBeMatched()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);
                int userId = 1;
                context.Add(new User { Id = userId, Name = "Rick Sanchez" });
                context.SaveChanges();

                // Act
                await userItemQueries.AddItemForUser(userId, new ItemDto { Id = 1 });
                await userItemQueries.AddItemForUser(userId, new ItemDto { Name = "Weapon" });

                // Assert
                Assert.Empty(context.UserItems);
            }
        }

        [Fact]
        public async Task AddItemForUser_ShouldDoNothing_IfUserCannotBeMatched()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);
                int itemId = 1;
                context.Add(new Item { Id = itemId, Name = "Weapon" });
                context.SaveChanges();

                // Act
                await userItemQueries.AddItemForUser(1, new ItemDto { Id = itemId });

                // Assert
                Assert.Empty(context.UserItems);
            }
        }

        [Fact]
        public async Task AddItemForUser_ShouldDoNothing_IfValidArgumentsButMaximumNumberOfItemsReached()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);
                int itemId = 1;
                int userId = 1;
                context.Add(new User { Id = userId, Name = "Rick Sanchez" });
                context.Add(new Item { Id = itemId, Name = "Weapon" });
                for (int i = 0; i < UserItemDto.MaximumNumberOfAnyItemAllowed; i++)
                {
                    context.Add(new UserItem { ItemId = itemId, UserId = userId });
                }
                context.SaveChanges();

                // Act
                await userItemQueries.AddItemForUser(userId, new ItemDto { Id = itemId });

                // Assert
                Assert.Equal(UserItemDto.MaximumNumberOfAnyItemAllowed, context.UserItems.Count());
            }
        }

        [Fact]
        public async Task AddItemForUser_ShouldDoNothing_IfValidArgumentsAndMaximumNumberOfItemsNotReached()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);
                int itemId = 1;
                int userId = 1;
                context.Add(new User { Id = userId, Name = "Rick Sanchez" });
                context.Add(new Item { Id = itemId, Name = "Weapon" });
                for (int i = 0; i < UserItemDto.MaximumNumberOfAnyItemAllowed - 1; i++)
                {
                    context.Add(new UserItem { ItemId = itemId, UserId = userId });
                }
                context.SaveChanges();

                // Act
                await userItemQueries.AddItemForUser(userId, new ItemDto { Id = itemId });

                // Assert
                Assert.Equal(UserItemDto.MaximumNumberOfAnyItemAllowed, context.UserItems.Count());
            }
        }

        [Fact]
        public async Task RemoveItemForUser_ShouldDoNothing_IfUserItemCannotBeFound()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);
                int itemId = 1;
                context.Add(new UserItem { Id = itemId, UserId = 2 });
                context.SaveChanges();

                // Act
                await userItemQueries.RemoveItemForUser(1, new ItemDto { Id = itemId });

                // Assert
                Assert.Single(context.UserItems);
            }
        }

        [Fact]
        public async Task RemoveItemForUser_ShouldRemoveItem_IfUserItemCanBeFoundByItemId()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);
                int userId = 1;
                int itemId = 1;
                context.Add(new Item { Id = itemId, Name = "Shortsword" });
                context.Add(new UserItem { ItemId = itemId, UserId = userId });
                context.SaveChanges();

                // Act
                await userItemQueries.RemoveItemForUser(userId, new ItemDto { Id = itemId });

                // Assert
                Assert.Empty(context.UserItems);
            }
        }

        [Fact]
        public async Task RemoveItemForUser_ShouldRemoveItem_IfUserItemCanBeFoundByItemName()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var userItemQueries = new UserItemQueries(context, mapper);
                int userId = 1;
                int itemId = 1;
                string itemName = "Shortsword";
                context.Add(new Item { Id = itemId, Name = itemName });
                context.Add(new UserItem { ItemId = itemId, UserId = userId });
                context.SaveChanges();

                // Act
                await userItemQueries.RemoveItemForUser(userId, new ItemDto { Name = itemName });

                // Assert
                Assert.Empty(context.UserItems);
            }
        }
    }
}
