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
    public class ItemQueriesTests
    {
        private IMapper GetAutoMapperMapper()
        {
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }

        [Fact]
        public async Task GetItem_ShouldReturnNull_IfItemWithIdCannotBeFound()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                int itemId = 1;
                var itemQueries = new ItemQueries(context, mapper);

                // Act
                ItemDto item = await itemQueries.GetItem(itemId);

                // Assert
                Assert.Null(item);
            }
        }

        [Fact]
        public async Task GetItem_ShouldReturnItem_IfItemWithIdCanBeFound()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var itemQueries = new ItemQueries(context, mapper);
                int itemId = 1;
                string itemName = "Weapon";
                context.Items.Add(new Item { Id = itemId, Name = itemName });
                context.SaveChanges();

                // Act
                ItemDto item = await itemQueries.GetItem(itemId);

                // Assert
                Assert.NotNull(item);
                Assert.Equal(itemName, item.Name);
                Assert.Equal(itemId, item.Id);
            }
        }

        [Fact]
        public async Task GetItem_ShouldReturnNull_IfItemCannotBeMatchedToIncomingData()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var itemQueries = new ItemQueries(context, mapper);

                // Act
                ItemDto item = await itemQueries.GetItem(new ItemDto());

                // Assert
                Assert.Null(item);
            }
        }

        [Fact]
        public async Task GetItem_ShouldReturnItem_IfItemCanBeMatchedByIncomingId()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var itemQueries = new ItemQueries(context, mapper);
                int itemId = 1;
                string itemName = "Weapon";
                context.Items.Add(new Item { Id = itemId, Name = itemName });
                context.SaveChanges();

                // Act
                ItemDto item = await itemQueries.GetItem(new ItemDto { Id = itemId });

                // Assert
                Assert.NotNull(item);
                Assert.Equal(itemName, item.Name);
                Assert.Equal(itemId, item.Id);
            }
        }

        [Fact]
        public async Task GetItem_ShouldReturnItem_IfItemCanBeMatchedByIncomingName()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var itemQueries = new ItemQueries(context, mapper);
                int itemId = 1;
                string itemName = "Weapon";
                context.Items.Add(new Item { Id = itemId, Name = itemName });
                context.SaveChanges();

                // Act
                ItemDto item = await itemQueries.GetItem(new ItemDto { Name = itemName });

                // Assert
                Assert.NotNull(item);
                Assert.Equal(itemName, item.Name);
                Assert.Equal(itemId, item.Id);
            }
        }

        [Fact]
        public async Task GetItems_ShouldReturnAllItems_IfTheyExist()
        {
            using (ApplicationDbContext context = ApplicationDbContextHelper.GetContext())
            {
                // Arrange
                IMapper mapper = GetAutoMapperMapper();
                var itemQueries = new ItemQueries(context, mapper);
                var item1 = new Item { Id = 1, Name = "Gun1" };
                var item2 = new Item { Id = 2, Name = "Gun2" };
                var item3 = new Item { Id = 3, Name = "Gun3" };
                context.Items.AddRange(item1, item2, item3);
                context.SaveChanges();

                // Act
                IEnumerable<ItemDto> items = await itemQueries.GetItems();

                // Assert
                Assert.NotEmpty(items);
                Assert.Equal(3, items.Count());
                Assert.Equal(new List<int> { 1, 2, 3 }, items.Select(i => i.Id).ToList());
                Assert.Equal(new List<string> { "Gun1", "Gun2", "Gun3" }, items.Select(i => i.Name).ToList());
            }
        }
    }
}
