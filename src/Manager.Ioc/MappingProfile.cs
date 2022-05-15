using AutoMapper;
using Manager.API.ViewModels;
using Manager.Domain.Entities;
using Manager.Services.DTO_s;

namespace Manager.Ioc
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MappingEntities();
            ReverseMappingEntities();
        }

        private void MappingEntities()
        {
            CreateMap<UserDto, User>();
            CreateMap<CreateUserViewModel, UserDto>();
        }
        private void ReverseMappingEntities()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }

        public static MapperConfiguration CreateMapperConfiguration()
            => new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
    }
}
