using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nutrigenius.Models;
using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nutrigenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (login == null || string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest(new { message = "Invalid login request" });
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Check User Login
                    string userQuery = "SELECT USERID FROM Login WHERE USERNAME = @UserName AND PASSWORD = @Password"; // Use hashed password
                    using (SqlCommand userCmd = new SqlCommand(userQuery, conn))
                    {
                        userCmd.Parameters.AddWithValue("@UserName", login.UserName);
                        userCmd.Parameters.AddWithValue("@Password", login.Password); // Hash the password before checking

                        var userId = await userCmd.ExecuteScalarAsync();
                        if (userId != null)
                        {
                            // Create token for the user
                            var token = GenerateJwtToken(userId.ToString(), "User");
                            return Ok(new { token, userType = "User" });
                        }
                    }

                    // Check Dietitian Login
                    string dietitianQuery = "SELECT USERID FROM DietLogin WHERE USERNAME = @UserName AND PASSWORD = @Password"; // Use hashed password
                    using (SqlCommand dietitianCmd = new SqlCommand(dietitianQuery, conn))
                    {
                        dietitianCmd.Parameters.AddWithValue("@UserName", login.UserName);
                        dietitianCmd.Parameters.AddWithValue("@Password", login.Password); // Hash the password before checking

                        var dietitianId = await dietitianCmd.ExecuteScalarAsync();
                        if (dietitianId != null)
                        {
                            // Create token for the dietitian
                            var token = GenerateJwtToken(dietitianId.ToString(), "Dietitian");
                            return Ok(new { token, userType = "Dietitian" });
                        }
                    }

                    return Unauthorized(new { message = "Invalid username or password." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }

        private string GenerateJwtToken(string userId, string userType)
        {
            // Define token expiration time
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, userType)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
