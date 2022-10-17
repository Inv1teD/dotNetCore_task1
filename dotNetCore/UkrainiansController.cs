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
    [Route("api")]
    [ApiController]
    public class UkrainiansController : ControllerBase
    {
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
                return BadRequest(new { message = "Incorrect data" });
            }
        }
    }
}