using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nutrigenius.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Nutrigenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BMIController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly UserContext _userContext;
        private readonly IConfiguration _configuration;

        public BMIController(IConfiguration configuration, UserContext userContext)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _userContext = userContext;
        }

        [HttpPost("BMI")]
        public async Task<IActionResult> BMI([FromBody] BMI bmi)
        {
            // Validate input data
            if (bmi == null || bmi.Age <= 0 || string.IsNullOrEmpty(bmi.Gender) || bmi.Height <= 0 || bmi.Weight <= 0)
            {
                return BadRequest(new { message = "Invalid input data" });
            }

            // Convert height from cm to m and calculate BMI
            decimal heightInMeters = bmi.Height / 100;
            decimal calculatedBmi = bmi.Weight / (heightInMeters * heightInMeters);

            // Determine BMI status
            string status = calculatedBmi switch
            {
                < 18.5m => "Underweight",
                >= 18.5m and <= 24.9m => "Healthy",
                >= 25.0m and <= 29.9m => "Overweight",
                _ => "Obese"
            };

            // Get the logged-in user's ID from the UserContext
            //var userId = _userContext.UserId;
            //if (userId == null)
            //{
            //    return Unauthorized(new { message = "User is not authenticated" });
            //}

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Insert new BMI record
                    string insertSql = @"INSERT INTO BMI (UserId, Age, Gender, Height, Weight, BMI, Status)
                                         VALUES (@UserId, @Age, @Gender, @Height, @Weight, @BMI, @Status)";

                    using (SqlCommand insertCmd = new SqlCommand(insertSql, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@UserId", bmi.UserID);
                        insertCmd.Parameters.AddWithValue("@Age", bmi.Age);
                        insertCmd.Parameters.AddWithValue("@Gender", bmi.Gender);
                        insertCmd.Parameters.AddWithValue("@Height", bmi.Height);
                        insertCmd.Parameters.AddWithValue("@Weight", bmi.Weight);
                        insertCmd.Parameters.AddWithValue("@BMI", calculatedBmi);
                        insertCmd.Parameters.AddWithValue("@Status", status);

                        int rowsInserted = await insertCmd.ExecuteNonQueryAsync();

                        var userId = await insertCmd.ExecuteScalarAsync();
                        if (userId != null)
                        {
                            // Create token for the user
                            var token = GenerateJwtToken(userId.ToString(), "User");
                            return Ok(new { token, userType = "User" });
                        }

                        if (rowsInserted > 0)
                        {
                            // Generate and return JWT token for the user
                            //var token = GenerateJwtToken(userId.ToString(), "User");
                            //return Ok(new { token, userType = "User", bmi = calculatedBmi, status });
                            return Ok(new { message = "BMI calculation and record insertion successful", bmi = calculatedBmi, status });
                        }

                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to insert BMI record");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }

        private string GenerateJwtToken(string userId, string userType)
        {
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
