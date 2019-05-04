using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestfulUsersInventory.DataQueries.Queries
{
    public interface IItemQueries
    {
        Task<IEnumerable<ItemDto>> GetItems();
        Task<IEnumerable<ItemDto>> GetItems(IEnumerable<int> ids);
        Task<ItemDto> GetItem(int id);
        Task<ItemDto> GetItem(ItemDto item);
    }
}
