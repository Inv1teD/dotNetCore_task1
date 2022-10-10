using dotNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.RegularExpressions;

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



            using (ApplicationContext db = new ApplicationContext(options))
            {
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

                app.Run(async (context) =>
                {
                    var response = context.Response;
                    var request = context.Request;
                    var path = request.Path;
                    string expressionForNumber = "^/api/users/[0-9]+$";  


                    Console.WriteLine(path);

                    if (path == "/api/users" && request.Method == "GET")
                    {
                        await GetAllPeople(response, db);
                    }
                    else if ((Regex.IsMatch(path, expressionForNumber)) && request.Method == "GET")
                    {
                        int? id = int.Parse(path.Value?.Split("/")[3]);
                        await GetPerson(response, id);

                    }
                    else if (path == "/api/users" && request.Method == "POST")
                    {
                        await CreatePerson(response, request);
                    }
                    else if ((Regex.IsMatch(path, expressionForNumber)) && request.Method == "DELETE")
                    {
                        int? id = int.Parse(path.Value?.Split("/")[3]);
                        await DeletePerson(response, id);
                    }
                    else
                    {
                        Console.WriteLine("Loaded base page");
                        response.ContentType = "text/html; charset=utf-8";
                        await response.SendFileAsync("Pages/Index.cshtml");
                    }
                });


                app.Run();

                // Gets all users
                static async Task GetAllPeople(HttpResponse response, ApplicationContext db)
                {
                    var ukrainians = db.Ukrainians.ToList();
                    await response.WriteAsJsonAsync(ukrainians);
                }
                // Gets one user with specific id
                async Task GetPerson(HttpResponse response, int? id)
                {

                    Ukrainian? ukrainian = db.Ukrainians.FirstOrDefault((u) => u.Id == id);
                    // If user exist, sending him

                    if (ukrainian != null)
                        await response.WriteAsJsonAsync(ukrainian);
                    // If user doesn't exist, sending status code and error message
                    else
                    {
                        response.StatusCode = 404;
                        await response.WriteAsJsonAsync(new { message = "User wasn't found" });
                    }
                }
                    
                async Task CreatePerson(HttpResponse response, HttpRequest request)
                    {
                    
                        try
                        {
                        var ukrainian = await request.ReadFromJsonAsync<Ukrainian>();

                        if (ukrainian != null)
                            {
                                db.Ukrainians.Add(ukrainian);
                                db.SaveChanges();
                                await response.WriteAsJsonAsync(ukrainian);
                            }
                            else
                            {
                                throw new Exception("Incorrect data");
                            }
                        }
                        catch (Exception)
                        {
                            response.StatusCode = 400;
                            await response.WriteAsJsonAsync(new { message = "Incorrect data" });
                        }
                    }
                async Task DeletePerson(HttpResponse response, int? id)
                {

                    Ukrainian? ukrainian = db.Ukrainians.FirstOrDefault((u) => u.Id == id);

                  
                    if (ukrainian != null)
                    {
                        db.Ukrainians.Remove(ukrainian);
                        db.SaveChanges();
                        await response.WriteAsJsonAsync(ukrainian);
                    }
                    else
                    {
                        response.StatusCode = 404;
                        await response.WriteAsJsonAsync(new { message = "Incorrect param id" });
                    }
                    
                }
                }
            }
        }
    }







