using EQueue.Db.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EQueue.Db
{
    public static class BuildExtension
    {
        public static void AddDbSetup(this IServiceCollection services,
            IConfiguration configuration)
        {
            var configurationString = configuration
                .GetRequiredSection("ConnectionString").Value;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configurationString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddDefaultIdentity<User>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            })
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
