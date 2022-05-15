using Manager.Infra;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Services.Services;
using Manager.Services.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Ioc
{
    public static class Injector
    {
        public static void InjectIoCServices(this IServiceCollection serviceColletion)
        {
            InjectRepositories(serviceColletion);
            InjectServices(serviceColletion);
            InjectEfCore(serviceColletion);
            InjectIMapper(serviceColletion);
        }

        private static void InjectRepositories(IServiceCollection serviceColletion)
        {
            serviceColletion.AddScoped<IUserRepository, UserRepository>();
        }

        private static void InjectServices(IServiceCollection serviceColletion)
        {
            serviceColletion.AddScoped<IUserService, UserService>();
        }

        private static void InjectEfCore(IServiceCollection serviceColletion)
        {
            serviceColletion.AddScoped<IManagerApiContext, ManagerApiContext>();
        }

        private static void InjectIMapper(IServiceCollection serviceColletion)
            => serviceColletion.AddSingleton(MappingProfile.CreateMapperConfiguration().CreateMapper());
    }
}
