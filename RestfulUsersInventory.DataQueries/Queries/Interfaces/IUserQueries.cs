using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestfulUsersInventory.DataQueries.Queries
{
    public interface IUserQueries
    {
        Task<UserDto> GetUser(int id);
        Task<IEnumerable<UserDto>> GetUsers();
    }
}
