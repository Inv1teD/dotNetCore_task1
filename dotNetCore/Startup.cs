using DotNetMentorship.TestAPI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace DotNetMentorship.TestAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var Config_Builder = new ConfigurationBuilder();
            Config_Builder.SetBasePath(Directory.GetCurrentDirectory());
            Config_Builder.AddJsonFile("appsettings.json");
            var config = Config_Builder.Build();
            string UkrainiansDbConnection = config.GetConnectionString("UkrainianDbConnection");
            var optionsBuilder = new DbContextOptionsBuilder<UkrainianDbContext>();

            services.AddMvc();

            services.AddSwaggerGen();
            
            services.AddDbContext<UkrainianDbContext>(options => options.UseNpgsql(UkrainiansDbConnection));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddRazorPages();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Use /api/ukrainians :)");
                });

                endpoints.MapControllers();
            });


            app.UseSwagger();
            app.UseSwaggerUI();

        }
    }
}
