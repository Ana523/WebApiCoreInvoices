using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceWebApi.Models
{
    public class SignInModel
    {
        [Column(TypeName = "nvarchar(150)")]
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._-]*@[a-z]*\.[a-z]{2,3}$")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [Required]
        public string Password { get; set; }
    }
}
