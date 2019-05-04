using System;
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
    public class UserInventoryQueries : IUserInventoryQueries
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserInventoryQueries(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> GetCountOfItemUserHas(int userId, int itemId)
        {
            return await _context.UserItems
                .CountAsync(ui => ui.UserId == userId && ui.ItemId == itemId);
        }

        public async Task<bool> DoesUserHaveItem(int userId, ItemDto item)
        {
            return await _context.UserItems
                .Include(ui => ui.Item)
                .Where(ui => ui.Item.Id == item.Id || ui.Item.Name == item.Name)
                .AnyAsync(ui => ui.UserId == userId);
        }

        public async Task<ItemDto> AddItemForUser(int userId, ItemDto item)
        {
            Item itemFromDb = await _context.Items
                .SingleOrDefaultAsync(i => i.Id == item.Id || i.Name == item.Name);
            var userItem = new UserItem { ItemId = itemFromDb.Id, UserId = userId };
            _context.Add(userItem);
            await _context.SaveChangesAsync();
            ItemDto itemAddedDto = _mapper.Map<ItemDto>(itemFromDb);
            return itemAddedDto;
        }

        public async Task<ItemDto> GetFirstMatchingItemForUser(int userId, ItemDto item)
        {
            UserItem userItem = await _context.UserItems
                .Include(ui => ui.Item)
                .Where(ui => ui.Item.Id == item.Id || ui.Item.Name == item.Name)
                .FirstOrDefaultAsync(ui => ui.UserId == userId);
            if (userItem == null)
            {
                return null;
            }
            ItemDto itemDto = _mapper.Map<ItemDto>(userItem.Item);
            return itemDto;
        }

        public async Task<IEnumerable<ItemDto>> GetItemsForUser(int userId)
        {
            IEnumerable<Item> items = await _context.UserItems
                .Where(ui => ui.UserId == userId)
                .Select(ui => ui.Item)
                .ToListAsync();
            IEnumerable<ItemDto> itemsDtos = _mapper.Map<IEnumerable<ItemDto>>(items);
            return itemsDtos;
        }

        public async Task RemoveFirstMatchingItemPossibleForUser(int userId, ItemDto item)
        {
            UserItem firstUserItemMatchingCriteria = await _context.UserItems
                .Include(ui => ui.Item)
                .Where(ui => ui.Item.Id == item.Id || ui.Item.Name == item.Name)
                .FirstOrDefaultAsync(ui => ui.UserId == userId);
            _context.Remove(firstUserItemMatchingCriteria);
            await _context.SaveChangesAsync();
        }
    }
}
