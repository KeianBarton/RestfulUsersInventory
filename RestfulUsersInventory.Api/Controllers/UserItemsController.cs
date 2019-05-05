using Microsoft.AspNetCore.Mvc;
using RestfulUsersInventory.DataQueries;
using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulUsersInventory.Api.Controllers
{
    [Route("api/")]
    public class UserItemsController : ControllerBase
    {
        private readonly IQueryHelper _queryHelper;

        public UserItemsController(IQueryHelper queryHelper)
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
            if (itemReq == null || itemReq.Id < 1 || userId < 1)
            {
                return BadRequest("Item in request is malformed / has invalid IDs");
            }
            UserDto user = await _queryHelper.UserQueries.GetUser(userId);
            if (user == null)
            {
                return NotFound($"No user can be found with ID {userId}");
            }
            UserItemDto itemRes = await _queryHelper.UserItemQueries
                .GetMatchingItemsForUser(userId, itemReq);
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
            IEnumerable<UserItemDto> userItems = await _queryHelper.UserItemQueries
                .GetAllItemsForUser(userId);
            if (!userItems.Any())
            {
                return NotFound($"No items found for user with ID {userId}");
            }
            return Ok(userItems);
        }

        [HttpPost("Users/{userId}/Items")]
        public async Task<IActionResult> AddItemsForUser(int userId, [FromBody] IEnumerable<ItemDto> items)
        {
            if (items == null || !items.Any())
            {
                return BadRequest("Items in request body could not be parsed");
            }
            UserDto user = await _queryHelper.UserQueries.GetUser(userId);
            if (user == null)
            {
                return NotFound($"No user can be found with ID {userId}");
            }
            foreach (ItemDto item in items)
            {
                // Regardless of what items were added or not, the API should
                // cleanly tell the user the latest inventory
                await _queryHelper.UserItemQueries.AddItemForUser(userId, item);
            }
            IEnumerable<UserItemDto> userItems = await _queryHelper.UserItemQueries
                .GetAllItemsForUser(userId);
            return Ok(userItems);
        }

        [HttpDelete("Users/{userId}/Items")]
        public async Task<IActionResult> RemoveItemsForUser(int userId, [FromBody] IEnumerable<ItemDto> items)
        {
            if (items == null || !items.Any())
            {
                return BadRequest("Items in request body could not be parsed");
            }
            UserDto user = await _queryHelper.UserQueries.GetUser(userId);
            if (user == null)
            {
                return NotFound($"No user can be found with ID {userId}");
            }
            foreach (ItemDto item in items)
            {
                // Regardless of what items were removed or not, the API should
                // cleanly tell the user the latest inventory
                await _queryHelper.UserItemQueries.RemoveItemForUser(userId, item);
            }
            IEnumerable<UserItemDto> userItems = await _queryHelper.UserItemQueries
                .GetAllItemsForUser(userId);
            return Ok(userItems);
        }
    }
}
