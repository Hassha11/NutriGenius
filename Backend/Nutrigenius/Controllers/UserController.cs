using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nutrigenius.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Nutrigenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _connectionString;

        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("User")]
        public async Task<IActionResult> User([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.UserName) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return Unauthorized(new { message = "0" });
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Check if user exists and retrieve user details
                    string sqlQuery = @"
                        SELECT 
                            r.USERID, r.NAME, r.GENDER, r.DOB, r.USERNAME, r.PASSWORD
                        FROM 
                            LOGIN l
                        INNER JOIN 
                            REGISTRATION r ON l.USERID = r.USERID
                        WHERE 
                            l.USERNAME = @UserName AND l.PASSWORD = @Password";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", loginRequest.UserName);
                        cmd.Parameters.AddWithValue("@Password", loginRequest.Password);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                var user = new
                                {
                                    UserID = reader["USERID"].ToString(),
                                    Name = reader["NAME"].ToString(),
                                    Gender = reader["GENDER"].ToString(),
                                    DOB = reader["DOB"].ToString(),
                                    UserName = reader["USERNAME"].ToString(),
                                    Password = reader["PASSWORD"].ToString()
                                };

                                // You might want to create and return a JWT token here instead
                                return Ok(user);
                            }
                            else
                            {
                                return Unauthorized(new { message = "0" });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }

        // Profile retrieval endpoint
        [HttpGet("Profile/{userId}")]
        public async Task<IActionResult> Profile(string userId)
        {
            try
            
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string sqlQuery = @"
                        SELECT 
                            r.NAME, r.GENDER, r.DOB, r.USERNAME, 
                            b.Age, b.Height, b.Weight, b.BMI, b.Status
                        FROM 
                            REGISTRATION r
                        LEFT JOIN 
                            BMI b ON r.USERID = b.UserID
                        WHERE 
                            r.USERID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                var userProfile = new
                                {
                                    Name = reader["NAME"].ToString(),
                                    Gender = reader["GENDER"].ToString(),
                                    DOB = reader["DOB"].ToString(),
                                    UserName = reader["USERNAME"].ToString(),
                                    Password = reader["PASSWORD"].ToString()
                                    //Age = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : (int?)null,
                                    //Height = reader["Height"] != DBNull.Value ? Convert.ToDecimal(reader["Height"]) : (decimal?)null,
                                    //Weight = reader["Weight"] != DBNull.Value ? Convert.ToDecimal(reader["Weight"]) : (decimal?)null,
                                    //BMI = reader["BMI"] != DBNull.Value ? Convert.ToDecimal(reader["BMI"]) : (decimal?)null,
                                    //Status = reader["Status"].ToString()
                                };

                                //return Ok(userProfile);
                                return Ok();
                            }
                            else
                            {
                                return NotFound(new { message = "User not found" });
                            }
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

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
