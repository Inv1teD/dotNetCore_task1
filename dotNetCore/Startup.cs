using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace DotNetMentorship.TestAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // rename config_builder
            var config_builder = new ConfigurationBuilder();
            config_builder.SetBasePath(Directory.GetCurrentDirectory());
            config_builder.AddJsonFile("appsettings.json");
            var config = config_builder.Build();
            string UkrainiansDbConnection = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<UkrainianDbContext>();

            services.AddDbContext<UkrainianDbContext>(options => options.UseNpgsql(UkrainiansDbConnection));
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
                    await context.Response.WriteAsync("Use /api :)");
                });

                endpoints.MapControllers();
            });
            
        }
    }
}
