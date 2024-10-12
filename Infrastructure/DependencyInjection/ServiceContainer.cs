using Application.Extension.Identity;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(config.GetConnectionString("Default")), ServiceLifetime.Scoped);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();
            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddAuthorizationBuilder()
                .AddPolicy("AdminitrationPolicy", adp =>
                {
                    adp.RequireAuthenticatedUser();
                    adp.RequireRole("Admin", "Manager");
                })
                .AddPolicy("YserPolicy", adp =>
                {
                    adp.RequireAuthenticatedUser();
                    adp.RequireRole("User");
                });
            return services;
        }
    }
}
