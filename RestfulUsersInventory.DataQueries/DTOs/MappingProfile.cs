using AutoMapper;
using RestfulUsersInventory.DataAccess.Entities;

namespace RestfulUsersInventory.DataQueries.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
