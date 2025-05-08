using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SecureService.BLL.Repositories;
using SecureService.BLL.Services;
using SecureService.DAL.Repositories;
using SecureService.DAL.Services;
using SecureService.Logging;

namespace SecureService.API.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<ILogRepository, LogService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            services.AddScoped<IDalClsRepository, DalClsService>();
            services.AddScoped<IClsRepository, ClsService>();

            services.AddScoped<IDalJWTTokenRepository, DalJWTTokenService>();
            services.AddScoped<IJWTTokenRepository, JWTTokenService>();

            services.AddScoped<IDalRegistrationRepository, DalRegistrationService>();
            services.AddScoped<IRegistrationRepository, RegistrationService>();

            services.AddScoped<IDalLoginRepository, DalLoginService>();
            services.AddScoped<ILoginRepository, LoginService>();

            services.AddScoped<IDalLogoutRepository, DalLogoutService>();
            services.AddScoped<ILogoutRepository, LogoutService>();

            services.AddScoped<IDalMatchRepository, DalMatchService>();
            services.AddScoped<IMatchRepository, MatchService>();
        }
    }
}
