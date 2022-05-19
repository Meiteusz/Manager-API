using AutoMapper;
using Manager.Domain.Entities;
using Manager.Services.DTO_s;

namespace Manager.Tests.TestUtilities
{
    public class AutoMapperConfiguration
    {
        public static IMapper GetConfiguration()
            => new MapperConfiguration(x =>
            {
        
                x.CreateMap<User, UserDto>().ReverseMap();
        
            }).CreateMapper();
    }
}
