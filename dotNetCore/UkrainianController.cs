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
using System.Diagnostics;
using static DotNetMentorship.TestAPI.UkrainianBadResponse;
using static DotNetMentorship.TestAPI.UkrainianSuccessResponse;
using Npgsql;
using Microsoft.AspNetCore.Http.Features;

namespace DotNetMentorship.TestAPI
{
    [Route("api/ukrainians")]
    [ApiController]
    public class UkrainianController : ControllerBase
    {
        private readonly UkrainianDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public UkrainianController(UkrainianDbContext dbContext, IUnitOfWork unitOfWork) {
           _unitOfWork = unitOfWork;
        }

        // GET: api/ukrainians

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _unitOfWork.Ukrainians.GetAllAsync();
            
            return Ok(users);
        }

        //GET api/ukrainians/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var user = await _unitOfWork.Ukrainians.GetByIdAsync(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return StatusCode(418);
            }
        }

        // POST /api/ukrainians
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] UkrainianDTO UserDTO)
        {
            try
            {
                if (UserDTO != null && UserDTO.Name != null && UserDTO.City != null && UserDTO.IsCalm != null)
                {
                    var user = new Ukrainian()
                    {
                        Name = UserDTO.Name,
                        City = UserDTO.City,
                        IsCalm = UserDTO.IsCalm
                    };
                    Ukrainian CreatedUkrainian = await _unitOfWork.Ukrainians.AddAsync(user);

                    Console.WriteLine($"Creating ukrainian {CreatedUkrainian.Name} with next props:" +
                        $"\nName: {CreatedUkrainian.Name}," +
                        $"\nCity: {CreatedUkrainian.City},\nIsCalm: {CreatedUkrainian.IsCalm}");

                    return Ok(new UkrainianObjectCreatedResponse($"User {CreatedUkrainian.Name} was successfully created!", CreatedUkrainian));
                }
                else
                {
                    throw new Exception("Incorrect data");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return BadRequest(new UkrainianValidationError(ex.Message));
            }
            }

        // PUT api/ukrainians/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UkrainianDTO UserDTO)
        {
            try
            {
                if (UserDTO != null && UserDTO.Name != null && UserDTO.City != null && UserDTO.IsCalm != null)
                {
                    var user = new Ukrainian()
                    {
                        Name = UserDTO.Name,
                        City = UserDTO.City,
                        IsCalm = UserDTO.IsCalm
                    };
                    Ukrainian CreatedUkrainian = await _unitOfWork.Ukrainians.UpdateAsync(id, user);

                    Console.WriteLine($"Updating ukrainian {CreatedUkrainian.Name} with next props:" +
                        $"\nId: {CreatedUkrainian.Id},\nName: {CreatedUkrainian.Name}," +
                        $"\nCity: {CreatedUkrainian.City},\nIsCalm: {CreatedUkrainian.IsCalm}");

                    return Ok(new UkrainianObjectCreatedResponse($"User {CreatedUkrainian.Name} was successfully updated!", CreatedUkrainian));

                }
                else
                {
                    throw new Exception("Incorrect data");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return BadRequest(new UkrainianValidationError(ex.Message));
            }
        }
    }
}
