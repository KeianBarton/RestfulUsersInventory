using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestfulUsersInventory.DataQueries.Queries
{
    public interface IUserQueries
    {
        Task<IEnumerable<UserDto>> GetUsers();
        Task<IEnumerable<UserDto>> GetUsers(IEnumerable<int> ids);
        Task<UserDto> GetUser(int id);
    }
}
