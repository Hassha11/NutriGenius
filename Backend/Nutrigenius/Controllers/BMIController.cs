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

                        //var userId = await insertCmd.ExecuteScalarAsync();
                        //if (userId != null)
                        //{
                        //    // Create token for the user
                        //    var token = GenerateJwtToken(userId.ToString(), "User");
                        //    return Ok(new { token, userType = "User" });
                        //}

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


        //[HttpPost("UserBMI")] //27-10-2024
        //public async Task<IActionResult> User([FromBody] LoginRequest loginRequest)
        //{
        //    if (loginRequest == null || string.IsNullOrEmpty(loginRequest.UserName) || string.IsNullOrEmpty(loginRequest.Password))
        //    {
        //        return Unauthorized(new { message = "0" });
        //    }

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connectionString))
        //        {
        //            await conn.OpenAsync();

        //            // Check if user exists and retrieve user details
        //            string sqlQuery = @"
        //                SELECT 
        //                    r.USERID, r.NAME, r.GENDER, r.DOB, r.USERNAME, r.PASSWORD
        //                FROM 
        //                    LOGIN l
        //                INNER JOIN 
        //                    REGISTRATION r ON l.USERID = r.USERID
        //                WHERE 
        //                    l.USERNAME = @UserName AND l.PASSWORD = @Password";

        //            using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@UserName", loginRequest.UserName);
        //                cmd.Parameters.AddWithValue("@Password", loginRequest.Password);

        //                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        var user = new
        //                        {
        //                            UserID = reader["USERID"].ToString(),
        //                            //Name = reader["NAME"].ToString(),
        //                            //Gender = reader["GENDER"].ToString(),
        //                            //DOB = reader["DOB"].ToString(),
        //                            //UserName = reader["USERNAME"].ToString(),
        //                            //Password = reader["PASSWORD"].ToString()
        //                        };

        //                        return Ok(user);
        //                    }
        //                    else
        //                    {
        //                        return Unauthorized(new { message = "0" });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
        //    }
        //}

        [HttpGet("BMIUserID/{userId}")] //27-10-2024
        public async Task<IActionResult> UserID(string userId)
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
                                var userData = new
                                {
                                    Name = reader["NAME"].ToString(),
                                    Gender = reader["GENDER"].ToString(),
                                    DOB = reader["DOB"].ToString(),
                                    UserName = reader["USERNAME"].ToString(),
                                    //Password = reader["PASSWORD"].ToString()
                                };

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

        //private string GenerateJwtToken(string userId, string userType) //27-10-2024
        //{
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, userId),
        //        new Claim(ClaimTypes.Role, userType)
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(30),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        //public class LoginRequest
        //{
        //    public string UserName { get; set; }
        //    public string Password { get; set; }
        //}

    }
}
