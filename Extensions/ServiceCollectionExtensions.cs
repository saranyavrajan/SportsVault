using SportsVault.Services.Implementations;
using SportsVault.Services.Interfaces;

namespace SportsVault.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICategoryService, CategoryService>();
            
            return services;
        }        
    }
}
