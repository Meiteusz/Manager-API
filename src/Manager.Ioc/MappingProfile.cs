using AutoMapper;
using Manager.Domain.Entities;
using Manager.Services.DTO_s;
using Manager.Services.DTO_s.ViewModels;

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
        }

        private void MappingViewModels()
        {
            CreateMap<CreateUserViewModel, UserDto>();
            CreateMap<UpdateUserViewModel, UserDto>();
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
