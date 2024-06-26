using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow Dependency Injection For DbContext
                //builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityService(builder.Configuration);
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });


            var app = builder.Build();

            #region Update Database
            // StoreDbContext context = new StoreDbContext();
            //await context.Database.MigrateAsync();
            using var Scope = app.Services.CreateScope(); // Group services which lifetime Scoped
            var Services = Scope.ServiceProvider;  // Hold Services
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var DbContext = Services.GetRequiredService<StoreDbContext>(); // Ask ClR For Creating Object from DbContext Explictly

                await DbContext.Database.MigrateAsync(); // Update Database

                var IdentityContext = Services.GetRequiredService<AppIdentityDbContext>();

                await IdentityContext.Database.MigrateAsync();
                var UserManager = Services.GetRequiredService<UserManager<AppUser>>();
                // Call Data Seeding

                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);
                await StoreContextSeed.SeedAsync(DbContext);
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error During Applying The Migration");
            }

            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>();
                app.UseSwaggerMiddlewares();
            }
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();


            app.Run();
        }
    }
}
