using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestfulUsersInventory.DataQueries.Queries
{
    public interface IUserInventoryQueries
    {
        Task<int> GetCountOfItemUserHas(int userId, int itemId);
        Task<bool> DoesUserHaveItem(int userId, ItemDto item);
        Task RemoveFirstMatchingItemPossibleForUser(int userId, ItemDto item);
        Task<ItemDto> AddItemForUser(int userId, ItemDto item);
        Task<IEnumerable<ItemDto>> GetItemsForUser(int userId);
        Task<ItemDto> GetFirstMatchingItemForUser(int userId, ItemDto item);
    }
}
