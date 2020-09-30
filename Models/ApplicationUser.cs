using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceWebApi.Models
{
    // Inherit from default identity
    public class ApplicationUser : IdentityUser
    {
        // Insert new column into Identity User table
        [Column(TypeName = "nvarchar(150)")]
        [Required]
        public string FullName { get; set; }
    }
}
