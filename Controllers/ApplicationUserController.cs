using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InvoiceWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InvoiceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        // Define properties
        private UserManager<ApplicationUser> _userManager;
        private AuthenticationContext _context;
        private readonly ApplicationSettings _appSettings;

        // Use constructor to inject providers
        public ApplicationUserController(UserManager<ApplicationUser> userManager,
                                         AuthenticationContext context,
                                         IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _context = context;
            _appSettings = appSettings.Value;
        }

        // Method for signing user up
        [HttpPost]
        [Route("Signup")]
        public async Task<Object> CreateUser(SignUpModel model)
        {
            // Check if there is a duplicate email address
            foreach (var user in _context.ApplicationUsers)
            {
                if (model.Email == user.Email)
                {
                    return BadRequest(new { message = "Email already exists" });
                };
            };

            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Email = model.Email
            };

            // Create user and provide password to hash
            var result = await _userManager.CreateAsync(applicationUser, model.Password);

            // return 200 status code
            return Ok(result);
        }

        // Method for signing user in
        [HttpPost]
        [Route("Signin")]
        public async Task<IActionResult> SigninUser(SignInModel model)
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(model.Email);

            // Check if user exists and if password is correct
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var TokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString())
                    }),

                    Expires = DateTime.Now.AddHours(1),

                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256)
                };

                // Create and write token
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(TokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                // Store token expiration time and username in variables
                var tokenExpirationTime = TokenDescriptor.Expires;
                var username = user.UserName;

                return Ok(new { token, tokenExpirationTime, username });
            }
            else
            {
                return BadRequest(new { message = "Email or password is incorrect" });
            }; 
        }
    }
}