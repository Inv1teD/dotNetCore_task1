using dotNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace dotNetCore
{
    class Program
    {

        static void Main(string[] args)
        {
            var config_builder = new ConfigurationBuilder();
            // Setting a path to current directory
            config_builder.SetBasePath(Directory.GetCurrentDirectory());
            // Getting config from appsettings.json
            config_builder.AddJsonFile("appsettings.json");
            // Creating config
            var config = config_builder.Build();
            // Getting string for connecting
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            var options = optionsBuilder
                .UseNpgsql(connectionString)
                .Options;

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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








