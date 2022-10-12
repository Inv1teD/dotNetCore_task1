﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace dotNetCore
{
    public class ApplicationContext : DbContext // rename to UkrainiansContext
    {
        public DbSet<Ukrainian> Ukrainians { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
