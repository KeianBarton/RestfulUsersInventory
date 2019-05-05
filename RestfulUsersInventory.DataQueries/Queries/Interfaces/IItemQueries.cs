using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestfulUsersInventory.DataQueries.Queries
{
    public interface IItemQueries
    {
        Task<ItemDto> GetItem(int id);
        Task<ItemDto> GetItem(ItemDto item);
        Task<IEnumerable<ItemDto>> GetItems();
    }
}
