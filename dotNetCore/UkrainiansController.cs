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

namespace DotNetMentorship.TestAPI
{
    // Its better to add related domain to routing
    // Something like api/ukrainians
    // Look through REST best practices
    [Route("api")]
    [ApiController]
    public class UkrainiansController : ControllerBase
    {
        // Please add accessor(public, private, protected) to any property/field/method
        // Its more convenient way, makes code more readable
        UkrainianDbContext _dbContext;
        public UkrainiansController(UkrainianDbContext dbContext) {
            _dbContext = dbContext;
            }

        // GET: api/

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _dbContext.Ukrainians.ToListAsync();
            return Ok(users);
        }

        //GET api/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var user = await _dbContext.Ukrainians.FindAsync(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return StatusCode(418);
            }
        }

        // POST /api
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Ukrainian user)
        {
            try
            {
                if (user != null)
                {
                    // Add more info to logs. For example "Creating user {user.Name}"
                    // It will help to look through a big amount of logs 
                    Console.WriteLine(user.Name);
                    await _dbContext.Ukrainians.AddAsync(user);
                    await _dbContext.SaveChangesAsync(); 
                    return Ok(user);
                }
                else
                {
                    throw new Exception("Incorrect data");
                }
            }
            catch (Exception)
            {
                // you can get a message from exception here
                // and please create custom error model for your responses
                // like UkrainiansApiSuccessResponse or just accessor UkrainiansApiResponse with field success
                // Its a good way to have consistent structure for both successfull and failed responses
                return BadRequest(new { message = "Incorrect data" });
            }
        }
    }
}