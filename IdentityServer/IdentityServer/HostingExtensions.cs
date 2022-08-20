using IdentityServer.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            Log.Information("Starting up");
            builder.Services.AddRazorPages();
            var configuration = builder.Configuration;

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var migrationsAssembly = typeof(Config).Assembly.GetName().Name;
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
            });
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;
                })
                .AddConfigurationStore(options => options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                    opt => opt.MigrationsAssembly(migrationsAssembly)))
                .AddOperationalStore(options => options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                    opt => opt.MigrationsAssembly(migrationsAssembly)))
                .AddAspNetIdentity<IdentityUser>()
                .AddProfileService<ProfileService>();
            builder.Services.AddAuthentication()
                .AddCookie();
            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages().RequireAuthorization();
            return app;
        }
    }
}