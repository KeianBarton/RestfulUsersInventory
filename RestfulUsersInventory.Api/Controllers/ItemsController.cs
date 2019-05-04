using Microsoft.AspNetCore.Mvc;
using RestfulUsersInventory.DataQueries;
using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulUsersInventory.Api.Controllers
{
    public class ItemsController : ControllerBase
    {
        private readonly IQueryHelper _queryHelper;

        public ItemsController(IQueryHelper queryHelper)
        {
            _queryHelper = queryHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            IEnumerable<ItemDto> items = await _queryHelper.ItemQueries.GetItems();
            if (!items.Any())
            {
                return NotFound("No items exist");
            }
            return Ok(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetItems([FromBody] IEnumerable<int> ids)
        {
            IEnumerable<int> uniqueIds = ids.Distinct().OrderBy(i => i).ToList();
            IEnumerable<ItemDto> items = await _queryHelper.ItemQueries.GetItems(uniqueIds);
            if (!items.Any())
            {
                return NotFound("No items found for the requested IDs");
            }
            return Ok(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetItem(int id)
        {
            ItemDto item = await _queryHelper.ItemQueries.GetItem(id);
            if (item == null)
            {
                return NotFound("No items found for the requested ID");
            }
            return Ok(item);
        }
    }
}
