using Manager.Infra;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Services.Services;
using Manager.Services.Services.Interfaces;
using Manager.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Ioc
{
    public abstract class Injector
    {
        public static void InjectBusinessIocServices(IServiceCollection serviceColletion)
        {
            InjectRepositories(serviceColletion);
            InjectServices(serviceColletion);
            InjectEfCore(serviceColletion);
            InjectOthersServices(serviceColletion);
        }

        // AddScoped - One instance to the all projet
        // AddTransient - One instance for every time the class to be called
        // AddSingleton - One instance to the all cicle project

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
            serviceColletion.AddTransient<IManagerApiContext, ManagerApiContext>();
        }

        private static void InjectOthersServices(IServiceCollection serviceColletion)
        {
            serviceColletion.AddScoped<ITokenGenerator, TokenGenerator>();
            serviceColletion.AddSingleton(MappingProfile.CreateMapperConfiguration().CreateMapper());
        }
    }
}
