using Microsoft.AspNetCore.Mvc;
using RestfulUsersInventory.DataQueries;
using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulUsersInventory.Api.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IQueryHelper _queryHelper;

        public UsersController(IQueryHelper queryHelper)
        {
            _queryHelper = queryHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            IEnumerable<UserDto> users = await _queryHelper.UserQueries.GetUsers();
            if (!users.Any())
            {
                return NotFound("No users exist");
            }
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromBody] IEnumerable<int> ids)
        {
            IEnumerable<int> uniqueIds = ids.Where(i => i > 0).Distinct().OrderBy(i => i).ToList();
            IEnumerable<UserDto> users = await _queryHelper.UserQueries.GetUsers(uniqueIds);
            if (!users.Any())
            {
                return NotFound("No users found for the requested IDs");
            }
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            UserDto user = await _queryHelper.UserQueries.GetUser(id);
            if (user == null)
            {
                return NotFound("No users found for the requested ID");
            }
            return Ok(user);
        }
    }
}
