using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestfulUsersInventory.DataQueries.Queries
{
    public interface IUserItemQueries
    {
        Task<bool> DoesUserHaveItem(int userId, ItemDto item);
        Task<IEnumerable<UserItemDto>> GetAllItemsForUser(int userId);
        Task<UserItemDto> GetMatchingItemsForUser(int userId, ItemDto item);
        Task AddItemForUser(int userId, ItemDto item);
        Task RemoveItemForUser(int userId, ItemDto item);
    }
}
