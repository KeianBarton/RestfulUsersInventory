using Microsoft.AspNetCore.Mvc;
using RestfulUsersInventory.DataQueries;
using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulUsersInventory.Api.Controllers
{
    [Route("api/")]
    public class UsersController : ControllerBase
    {
        private readonly IQueryHelper _queryHelper;

        public UsersController(IQueryHelper queryHelper)
        {
            _queryHelper = queryHelper;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            IEnumerable<UserDto> users = await _queryHelper.UserQueries.GetUsers();
            if (!users.Any())
            {
                return NotFound("No users exist");
            }
            return Ok(users);
        }

        [HttpGet("Users/{id}")]
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
