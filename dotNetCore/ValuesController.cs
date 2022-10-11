using Microsoft.AspNetCore.Mvc;
using dotNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Nodes;

namespace dotNetCore
{
    [Route("api")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public DbContextOptions<ApplicationContext> DBConnect()
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
            return options;
        }

        // GET: api/
        // Returns data about all users
        [HttpGet]
        public IActionResult GetAll()
        {
            // Get data from db
            ApplicationContext db = new ApplicationContext(DBConnect());
            var users = db.Ukrainians.ToList();
            db.Dispose();
            // Return all users and StatusCode 200
            return Ok(users);

        }

        // GET api/{id}
        // Returns data about user with specific id
        [HttpGet("{id}")]
        public IActionResult Getone(int id)
        {
            // Get data from db
            ApplicationContext db = new ApplicationContext(DBConnect());
            var user = db.Ukrainians.FirstOrDefault((u) => u.Id == id);
            db.Dispose();
            // If user exist, sending data about user
            if (user != null)
            {
                return Ok(user);
            }
            // If user doesn't exist, sending status code and error message
            else
            {
                return StatusCode(418);
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] Ukrainian user)
        {
            // Connect to Db
            ApplicationContext db = new ApplicationContext(DBConnect());
            try
            {
                // If there is any object in request body, trying to create new user
                if (user != null)
                {
                    db.Ukrainians.Add(user);
                    db.SaveChanges();
                    db.Dispose();
                    return Ok(user);
                }
                // If there is a problem with given data (ex. wrong type of data)
                else
                {
                    db.Dispose();
                    throw new Exception("Incorrect data");
                }
            }
            // If exception is caught, sending 400 status code 
            catch (Exception)
            {
                db.Dispose();
                return BadRequest(new { message = "Incorrect data" });
            }
        }
    }
}