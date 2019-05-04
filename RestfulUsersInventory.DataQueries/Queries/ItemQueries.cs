using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestfulUsersInventory.DataAccess;
using RestfulUsersInventory.DataAccess.Entities;
using RestfulUsersInventory.DataQueries.DTOs;

namespace RestfulUsersInventory.DataQueries.Queries
{
    public class ItemQueries : IItemQueries
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ItemQueries(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ItemDto> GetItem(int id)
        {
            Item item = await _context.Items.SingleOrDefaultAsync(i => i.Id == id);
            ItemDto dto = _mapper.Map<ItemDto>(item);
            return dto;
        }

        public async Task<ItemDto> GetItem(ItemDto item)
        {
            Item itemFromDb = await _context.Items
                .SingleOrDefaultAsync(i => i.Id == item.Id || i.Name == item.Name);
            ItemDto dto = _mapper.Map<ItemDto>(item);
            return dto;
        }

        public async Task<IEnumerable<ItemDto>> GetItems()
        {
            IEnumerable<Item> items = await _context.Items.ToListAsync();
            IEnumerable<ItemDto> dtos = _mapper.Map<IEnumerable<ItemDto>>(items);
            return dtos;
        }

        public async Task<IEnumerable<ItemDto>> GetItems(IEnumerable<int> ids)
        {
            IEnumerable<Item> items = await _context.Items
                .Where(i => ids.Any(id => id == i.Id))
                .ToListAsync();
            IEnumerable<ItemDto> dtos = _mapper.Map<IEnumerable<ItemDto>>(items);
            return dtos;
        }
    }
}
