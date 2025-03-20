using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SecurePosts.BLL.Repositories;
using SecurePosts.BLL.Services;
using SecurePosts.DAL.Repositories;
using SecurePosts.DAL.Services;
using SecurePosts.Logging;

namespace SecurePosts.API.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<ILogRepository, LogService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IDalClsRepository, DalClsService>();
            services.AddScoped<IClsRepository, ClsService>();

            services.AddScoped<IDalJWTTokenRepository, DalJWTTokenService>();
            services.AddScoped<IJWTTokenRepository, JWTTokenService>();

            services.AddScoped<IDalRegistrationRepository, DalRegistrationService>();
            services.AddScoped<IRegistrationRepository, RegistrationService>();

            services.AddScoped<IDalLoginRepository, DalLoginService>();
            services.AddScoped<ILoginRepository, LoginService>();
        }
    }
}
