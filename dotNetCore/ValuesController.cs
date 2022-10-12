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
    // rename to UkrainiansController

    // consider using async db calls instead of sync, rename methods using Async convention
    // MethodAsync instead of Method
    public class ValuesController : ControllerBase
    {
        // use ApplicationContext injection in constructor instead of this method
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
        [HttpGet]
        public IActionResult GetAll()
        {
            ApplicationContext db = new ApplicationContext(DBConnect());
            var users = db.Ukrainians.ToList();
            db.Dispose();
            return Ok(users);

        }

        // GET api/{id}
        [HttpGet("{id}")]
        public IActionResult Getone(int id) // rename to Get
        {
            // Get data from db
            ApplicationContext db = new ApplicationContext(DBConnect());
            var user = db.Ukrainians.FirstOrDefault((u) => u.Id == id);
            db.Dispose();

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return StatusCode(418);
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] Ukrainian user)
        {
            using ApplicationContext db = new ApplicationContext(DBConnect());
            try
            {
                if (user != null)
                {
                    db.Ukrainians.Add(user);
                    db.SaveChanges();
                    // remove .Dispose() from the code
                    db.Dispose();
                    return Ok(user);
                }
                else
                {
                    // remove .Dispose() from the code
                    db.Dispose();
                    throw new Exception("Incorrect data");
                }
            }
            catch (Exception)
            {
                db.Dispose();
                return BadRequest(new { message = "Incorrect data" });
            }
        }
    }
}