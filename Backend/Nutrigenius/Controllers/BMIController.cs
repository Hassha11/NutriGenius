using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nutrigenius.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Azure.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Nutrigenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BMIController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly UserContext _userContext;

        public BMIController(IConfiguration configuration, UserContext userContext)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _userContext = userContext;
        }

        [HttpPost("BMI")]
        public async Task<IActionResult> BMI([FromBody] BMI bmi)
        {
            if (bmi == null || bmi.Age <= 0 || string.IsNullOrEmpty(bmi.Gender) || bmi.Height <= 0 || bmi.Weight <= 0)
            {
                return BadRequest(new { message = "Invalid input data" });
            }

            // Convert height from cm to m
            decimal heightInMeters = bmi.Height / 100;

            // Calculate BMI
            decimal calculatedBmi = bmi.Weight / (heightInMeters * heightInMeters);

            // Determine BMI status based on calculated BMI
            string status;
            if (calculatedBmi < 18.5m)
            {
                status = "Underweight";
            }
            else if (calculatedBmi >= 18.5m && calculatedBmi <= 24.9m)
            {
                status = "Healthy";
            }
            else if (calculatedBmi >= 25.0m && calculatedBmi <= 29.9m)
            {
                status = "Overweight";
            }
            else
            {
                status = "Obese";
            }

            // Get the logged-in user's ID
            //var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userId == null)
            //{
            //    return Unauthorized(new { message = "User is not authenticated" });
            //}

            // Retrieve and parse the user ID from claims
            //var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId) || userId <= 0)
            //{
            //    return Unauthorized(new { message = "User is not authenticated or user ID is invalid" });
            //}

            var userId = _userContext.UserId;
            if (userId == null)
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }

            _userContext.UserId = userId;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    //Select Uqueryser Details
                    string query = "SELECT USERID FROM LOGIN WHERE USERNAME = @Username AND PASSWORD = @Password ";

                    // Insert new BMI record
                    string insertSql = @"INSERT INTO BMI (UserId, Age, Gender, Height, Weight, BMI, Status)
                             VALUES (@UserId, @Age, @Gender, @Height, @Weight, @BMI, @Status)";

                    using (SqlCommand insertCmd = new SqlCommand(insertSql, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@UserId", userId);
                        insertCmd.Parameters.AddWithValue("@Age", bmi.Age);
                        insertCmd.Parameters.AddWithValue("@Gender", bmi.Gender);
                        insertCmd.Parameters.AddWithValue("@Height", bmi.Height);
                        insertCmd.Parameters.AddWithValue("@Weight", bmi.Weight);
                        insertCmd.Parameters.AddWithValue("@BMI", calculatedBmi);
                        insertCmd.Parameters.AddWithValue("@Status", status);

                        int rowsInserted = await insertCmd.ExecuteNonQueryAsync();

                        if (rowsInserted > 0)
                        {
                            return Ok(new { message = "BMI calculation and record insertion successful", bmi = calculatedBmi });
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
    }
}
