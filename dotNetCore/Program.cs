using dotNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
// DotNetMentorship.Data

namespace dotNetCore // PascalCase; Example: DotNetMentorship.TestAPI
{
    class Program
    {

        static void Main(string[] args)
        {
            // rename using camelCase
            var config_builder = new ConfigurationBuilder();

            config_builder.SetBasePath(Directory.GetCurrentDirectory());
            config_builder.AddJsonFile("appsettings.json");
            var config = config_builder.Build();

            string connectionString = config.GetConnectionString("DefaultConnection");
            // consider renaming connection string name to UkrainiansDbConnection

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            var options = optionsBuilder
                .UseNpgsql(connectionString)
                .Options;

            var builder = WebApplication.CreateBuilder(args);

            // Consider using separate Startup class
            // Add services to the container.
            builder
                .Services
                //.AddDbContext<ApplicationContext>
                // is the right way of setuping db context for your application
                .AddRazorPages();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();    
            });
            app.Run();

        }
    }
}








