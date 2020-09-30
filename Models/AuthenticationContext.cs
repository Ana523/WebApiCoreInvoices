using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceWebApi.Models
{
    // Inherit from default identity context
    public class AuthenticationContext : IdentityDbContext
    {
        public AuthenticationContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
