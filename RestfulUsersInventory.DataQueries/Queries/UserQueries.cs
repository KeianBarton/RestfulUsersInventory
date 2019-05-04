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
    public class UserQueries : IUserQueries
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserQueries(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUser(int id)
        {
            User user = await _context.Users.SingleOrDefaultAsync(i => i.Id == id);
            UserDto dto = _mapper.Map<UserDto>(user);
            return dto;
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            IEnumerable<User> users = await _context.Users.ToListAsync();
            IEnumerable<UserDto> dtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return dtos;
        }

        public async Task<IEnumerable<UserDto>> GetUsers(IEnumerable<int> ids)
        {
            IEnumerable<User> users = await _context.Users
                .Where(i => ids.Any(id => id == i.Id))
                .ToListAsync();
            IEnumerable<UserDto> dtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return dtos;
        }
    }
}
