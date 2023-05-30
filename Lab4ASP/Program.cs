using Lab4ASP.Data;
using Lab4ASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using DotNetEnv;


namespace Lab4ASP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {

          
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString));


            DotNetEnv.Env.Load();
            // Retrieve the connection string from the environment variable
            //var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_AZURE");
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'CONNECTION_STRING' not found.");
            }
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));



            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true) //sätt till false om det krånglar kan lösa problemet.
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(); //To enable session support  behövs för lägga till users

            var app = builder.Build();


            //// Call SeedRolesAsync method to seed Identity roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                ContextSeed.SeedRolesAsync(userManager, roleManager).Wait();
                ContextSeed.SeedSuperAdminAsync(userManager, roleManager).Wait();

                //Add data to database at program run
               ContextSeed.Initialize(context);
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession(); // Add this line to enable session middleware behövs för att lägga till ny user

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=DashBoards}/{action=GetRandomBook}/{id?}");

            app.MapRazorPages();

                //app.Run();

                if (app.Environment.IsDevelopment())
                {
                    app.Run();
                }
                else
                {
                    app.Run("http://0.0.0.0:8080");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}