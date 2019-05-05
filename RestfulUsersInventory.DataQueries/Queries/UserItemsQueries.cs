using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestfulUsersInventory.DataAccess;
using RestfulUsersInventory.DataAccess.Entities;
using RestfulUsersInventory.DataQueries.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulUsersInventory.DataQueries.Queries
{
    public class UserItemQueries : IUserItemQueries
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserItemQueries(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> DoesUserHaveItem(int userId, ItemDto item)
        {
            return await _context.UserItems
                .AsNoTracking()
                .AnyAsync
                (
                    ui =>
                    ui.UserId == userId &&
                    (ui.Item.Id == item.Id || ui.Item.Name == item.Name)
                );
        }

        public async Task<IEnumerable<UserItemDto>> GetAllItemsForUser(int userId)
        {
            IEnumerable<UserItem> userItems = await _context.UserItems
                .AsNoTracking()
                .Where(ui => ui.UserId == userId)
                .Include(ui => ui.Item) // Required for mapping
                .Include(ui => ui.User) // Required for mapping
                .GroupBy(ui => new { ui.UserId, ui.ItemId })
                .Select(g => g.First())
                .ToListAsync();
            IEnumerable<UserItemDto> userItemsDtos = _mapper.Map<IEnumerable<UserItemDto>>(userItems);
            foreach(UserItemDto ui in userItemsDtos)
            {
                ui.ItemCount = await GetNumberOfMatchingItemsForUser(ui.ItemId, ui.UserId);
            }
            return userItemsDtos;
        }

        public async Task<UserItemDto> GetMatchingItemsForUser(int userId, ItemDto item)
        {
            UserItem userItem = await _context.UserItems
                .AsNoTracking()
                .Include(ui => ui.Item) // Required for mapping
                .Include(ui => ui.User) // Required for mapping
                .Where(ui => ui.Item.Id == item.Id || ui.Item.Name == item.Name)
                .FirstOrDefaultAsync(ui => ui.UserId == userId);
            if (userItem == null)
            {
                return null;
            }
            UserItemDto userItemDto = _mapper.Map<UserItemDto>(userItem);
            userItemDto.ItemCount = await GetNumberOfMatchingItemsForUser(userItem.ItemId, userItem.UserId);
            return userItemDto;
        }

        public async Task AddItemForUser(int userId, ItemDto item)
        {
            Item itemFromDb = await _context.Items
                .AsNoTracking()
                .SingleOrDefaultAsync(i => i.Id == item.Id || i.Name == item.Name);
            if (itemFromDb == null)
            {
                return;
            }
            bool userExistsInDb = await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == userId);
            if (!userExistsInDb)
            {
                return;
            }
            int numberOfItems = await GetNumberOfMatchingItemsForUser(itemFromDb.Id, userId);
            if (numberOfItems == UserItemDto.MaximumNumberOfAnyItemAllowed)
            {
                return;
            }
            var userItem = new UserItem { ItemId = itemFromDb.Id, UserId = userId };
            _context.Add(userItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemForUser(int userId, ItemDto item)
        {
            UserItem userItem = await _context.UserItems
                .Where(ui => ui.Item.Id == item.Id || ui.Item.Name == item.Name)
                .FirstOrDefaultAsync(ui => ui.UserId == userId);
            if (userItem == null)
            {
                return;
            }
            _context.Remove(userItem);
            await _context.SaveChangesAsync();
        }

        private async Task<int> GetNumberOfMatchingItemsForUser(int itemId, int userId)
        {
            return await _context.UserItems
                .AsNoTracking()
                .CountAsync(ui => ui.ItemId == itemId && ui.UserId == userId);
        }
    }
}
