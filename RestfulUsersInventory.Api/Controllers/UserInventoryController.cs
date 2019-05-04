using Microsoft.AspNetCore.Mvc;
using RestfulUsersInventory.DataQueries;
using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulUsersInventory.Api.Controllers
{
    [Route("api/")]
    public class UserInventoryController : ControllerBase
    {
        private readonly IQueryHelper _queryHelper;

        public UserInventoryController(IQueryHelper queryHelper)
        {
            _queryHelper = queryHelper;
        }

        [HttpGet("Users/{userId}/Items/{itemId}")]
        public async Task<IActionResult> GetItemForUser(int userId, int itemId)
        {
            return await GetItemForUser(userId, new ItemDto { Id = itemId });
        }

        private async Task<IActionResult> GetItemForUser(int userId, ItemDto itemReq)
        {
            if (itemReq == null)
            {
                return BadRequest("Item in request is malformed");
            }
            UserDto user = await _queryHelper.UserQueries.GetUser(userId);
            if (user == null)
            {
                return NotFound($"No user can be found with ID {userId}");
            }
            ItemDto itemRes = await _queryHelper.UserInventoryQueries
                .GetFirstMatchingItemForUser(userId, itemReq);
            if (itemRes == null)
            {
                return NotFound($"No items matched for request for user with ID {userId}");
            }
            return Ok(itemRes);
        }

        [HttpGet("Users/{userId}/Items")]
        public async Task<IActionResult> GetItemsForUser(int userId)
        {
            UserDto user = await _queryHelper.UserQueries.GetUser(userId);
            if (user == null)
            {
                return NotFound($"No user can be found with ID {userId}");
            }
            IEnumerable<ItemDto> items = await _queryHelper.UserInventoryQueries
                .GetItemsForUser(userId);
            if (!items.Any())
            {
                return NotFound($"No items found for user with ID {userId}");
            }
            return Ok(items);
        }

        [HttpPost("Users/{userId}/Items")]
        public async Task<IActionResult> AddItemForUser(int userId, [FromBody] ItemDto item)
        {
            if (item == null)
            {
                return BadRequest("Item in request is malformed");
            }
            ItemDto itemFromDb = await _queryHelper.ItemQueries.GetItem(item);
            if (itemFromDb == null)
            {
                return NotFound($"No item can be found with the data in the request body");
            }
            UserDto user = await _queryHelper.UserQueries.GetUser(userId);
            if (user == null)
            {
                return NotFound($"No user can be found with ID {userId}");
            }
            int numberOfItemUserHas = await _queryHelper.UserInventoryQueries
                .GetCountOfItemUserHas(userId, itemFromDb.Id);
            if (numberOfItemUserHas == UserDto.MaximumNumberOfAnyItemTypeAllowed)
            {
                return Forbid($"User with ID {userId} has the maximum number of item with ID {itemFromDb.Id} allowed");
            }
            ItemDto itemAdded = await _queryHelper.UserInventoryQueries
                .AddItemForUser(userId, item);
            return Ok(itemAdded);
        }

        [HttpPost("Users/{userId}/Items")]
        public async Task<IActionResult> AddItemsForUser(int userId, [FromBody] IEnumerable<ItemDto> items)
        {
            //if (items == null || !items.Any())
            //{
            //    return BadRequest("Items in request are malformed");
            //}
            //UserDto user = await _queryHelper.UserQueries.GetUser(userId);
            //if (user == null)
            //{
            //    return NotFound($"No user can be found with ID {userId}");
            //}
            //var itemsAdded = new List<ItemDto>();
            //var itemsFromDb = _queryHelper.ItemQueries.GetItems()
            //int numberOfItemUserHas = await _queryHelper.UserInventoryQueries.GetCountOfItemUserHas(userId, itemInGroup.Id);
            //foreach (ItemDto itemInGroup in ItemsThatCanBeAddedBeforeLimit(items, numberOfItemsUserHas))
            //{
            //    ItemDto itemAdded = await _queryHelper.UserInventoryQueries.AddItemForUser(userId, itemInGroup);
            //    itemsAdded.Add(itemAdded);
            //}
            //return Ok(itemsAdded);
        }

        [HttpDelete("Users/{userId}/Items")]
        public async Task<IActionResult> RemoveItemForUser(int userId, [FromBody] ItemDto item)
        {
            if (item == null)
            {
                return BadRequest("Item in request is malformed");
            }
            ItemDto itemFromDb = await _queryHelper.ItemQueries.GetItem(item);
            if (itemFromDb == null)
            {
                return NotFound($"No item can be found with the data in the request body");
            }
            UserDto user = await _queryHelper.UserQueries.GetUser(userId);
            if (user == null)
            {
                return NotFound($"No user can be found with ID {userId}");
            }
            bool userHasItem = await _queryHelper.UserInventoryQueries
                .DoesUserHaveItem(userId, item);
            if (!userHasItem)
            {
                return NotFound($"The user with ID {userId} does not have item in the request");
            }
            await _queryHelper.UserInventoryQueries
                .RemoveFirstMatchingItemPossibleForUser(userId, item);
            return Ok();
        }

        [HttpDelete("Users/{userId}/Items")]
        public async Task<IActionResult> RemoveItemsForUser(int userId, [FromBody] IEnumerable<ItemDto> items)
        {
            //if (items == null || !items.Any())
            //{
            //    return BadRequest("Items in request are malformed");
            //}
            //UserDto user = await _queryHelper.UserQueries.GetUser(userId);
            //if (user == null)
            //{
            //    return NotFound($"No user can be found with ID {userId}");
            //}
            //await _queryHelper.UserInventoryQueries.RemoveItemsForUser(userId, items);
            //return Ok();
        }

        private IEnumerable<ItemDto> ItemsThatCanBeAddedBeforeLimit(IEnumerable<ItemDto> items, int numberOfItemsUserHas)
        {
            foreach (ItemDto item in items)
            {
                if (numberOfItemsUserHas < UserDto.MaximumNumberOfAnyItemTypeAllowed)
                {
                    yield return item;
                    numberOfItemsUserHas++;
                }
            }
        }
    }
}
