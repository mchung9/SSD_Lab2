using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSD_Lab1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSD_Lab1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //Database Tables
        public DbSet<Team> Teams { get; set; }
    }
}
