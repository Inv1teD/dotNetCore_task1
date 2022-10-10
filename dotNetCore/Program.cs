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
            // установка пути к текущему каталогу
            config_builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            config_builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = config_builder.Build();
            // получаем строку подключения
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            var options = optionsBuilder
                .UseNpgsql(connectionString)
                .Options;



            using (ApplicationContext db = new ApplicationContext(options))
            {

                // Добавление
                /*using (ApplicationContext db = new ApplicationContext(options))
                {
                    Ukrainian user1 = new Ukrainian { Name = "Mykola", City = "Kharkiv", is_Angry = true };
                    Ukrainian user2 = new Ukrainian { Name = "Dmytro", City = "Lviv", is_Angry = true };


                    // Добавление
                    db.Ukrainians.Add(user1);
                    db.Ukrainians.Add(user2);
                    db.SaveChanges();
                } */
                
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddRazorPages();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

                // получение всех пользователей
                static async Task GetAllPeople(HttpResponse response, ApplicationContext db)
                {
                    var ukrainians = db.Ukrainians.ToList();
                    await response.WriteAsJsonAsync(ukrainians);
                }
                // получение одного пользователя по id
                async Task GetPerson(HttpResponse response, int? id)
                {

                    Ukrainian? ukrainian = db.Ukrainians.FirstOrDefault((u) => u.Id == id);
                    // если пользователь найден, отправляем его

                    if (ukrainian != null)
                        await response.WriteAsJsonAsync(ukrainian);
                    // если не найден, отправляем статусный код и сообщение об ошибке
                    else
                    {
                        response.StatusCode = 404;
                        await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
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
                                throw new Exception("Некорректные данные");
                            }
                        }
                        catch (Exception)
                        {
                            response.StatusCode = 400;
                            await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
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
                        await response.WriteAsJsonAsync(new { message = "Некорректный параметр id" });
                    }
                    
                }
                }
            }
        }
    }







