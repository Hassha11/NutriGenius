using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nutrigenius.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Nutrigenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BMIController : ControllerBase
    {
        private readonly string _connectionString;

        public BMIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("BMI")]
        public async Task<IActionResult> BMI([FromBody] BMI bmi)
        {
            if (bmi == null || string.IsNullOrEmpty(bmi.Age) || string.IsNullOrEmpty(bmi.Gender) || bmi.Height <= 0 || bmi.Weight <= 0)
            {
                return Unauthorized(new { message = "Invalid input data" });
            }

            // Convert height from cm to m
            decimal heightInMeters = bmi.Height / 100;

            // Calculate BMI
            decimal calculatedBmi = bmi.Weight / (heightInMeters * heightInMeters);

            // Retrieve user ID from claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }

            string userId = userIdClaim.Value;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string sql = @"INSERT INTO BMI (USERID, AGE, GENDER, HEIGHT, WEIGHT) VALUES (@UserID, @Age, @Gender, @Height, @Weight)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@Age", bmi.Age);
                        cmd.Parameters.AddWithValue("@Gender", bmi.Gender);
                        cmd.Parameters.AddWithValue("@Height", bmi.Height);
                        cmd.Parameters.AddWithValue("@Weight", bmi.Weight);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            return Ok(new { message = "BMI calculation and record insertion successful", bmi = calculatedBmi });
                        }
                        else
                        {
                            return Unauthorized(new { message = "Failed to insert BMI record" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }
    }
}
