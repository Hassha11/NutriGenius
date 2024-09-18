using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
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
                                    //Password = reader["PASSWORD"].ToString()
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

        //// Profile retrieval endpoint
        //[HttpGet("Profile/{userId}")]
        //public async Task<IActionResult> Profile(string userId)
        //{
        //    try

        //    {
        //        using (SqlConnection conn = new SqlConnection(_connectionString))
        //        {
        //            await conn.OpenAsync();

        //            string sqlQuery = @"
        //                SELECT 
        //                    r.NAME, r.GENDER, r.DOB, r.USERNAME, r.PASSWORD, 
        //                    b.Age, b.Height, b.Weight, b.BMI, b.Status
        //                FROM 
        //                    REGISTRATION r
        //                LEFT JOIN 
        //                    BMI b ON r.USERID = b.UserID
        //                WHERE 
        //                    r.USERID = @UserID";

        //            using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@UserID", userId);

        //                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        var userProfile = new
        //                        {
        //                            Name = reader["NAME"].ToString(),
        //                            Gender = reader["GENDER"].ToString(),
        //                            DOB = reader["DOB"].ToString(),
        //                            UserName = reader["USERNAME"].ToString(),
        //                            Password = reader["PASSWORD"].ToString()
        //                            //Age = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : (int?)null,
        //                            //Height = reader["Height"] != DBNull.Value ? Convert.ToDecimal(reader["Height"]) : (decimal?)null,
        //                            //Weight = reader["Weight"] != DBNull.Value ? Convert.ToDecimal(reader["Weight"]) : (decimal?)null,
        //                            //BMI = reader["BMI"] != DBNull.Value ? Convert.ToDecimal(reader["BMI"]) : (decimal?)null,
        //                            //Status = reader["Status"].ToString()
        //                        };

        //                        //return Ok(userProfile);
        //                        return Ok();
        //                    }
        //                    else
        //                    {
        //                        return NotFound(new { message = "User not found" });
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

        // Profile retrieval endpoint
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile(string userName, string password)
        {
            //if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            //{
            //    return BadRequest(new { message = "Please log into the system." });
            //}

            try

            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    //string sqlQuery = @"
                    //SELECT 
                    //r.NAME, r.GENDER, r.DOB, r.USERNAME, r.PASSWORD, 
                    //b.Age, b.Height, b.Weight, b.BMI, b.Status
                    //FROM 
                    //REGISTRATION r
                    //LEFT JOIN 
                    //BMI b ON r.USERID = b.UserID
                    //WHERE 
                    //r.USERNAME = @userName AND r.PASSWORD = @password";

                    string sqlQuery = @"
                    SELECT 
                    NAME, GENDER, DOB, USERNAME, PASSWORD
                    FROM 
                    REGISTRATION 
                    WHERE 
                    USERNAME = @userName AND PASSWORD = @password";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@userName", userName);
                        cmd.Parameters.AddWithValue("@password", password);

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
                                };

                                return Ok(userProfile);
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


        //[HttpDelete("UserProfile")]
        //public async Task<IActionResult> UserProfile([FromBody] LoginRequest loginRequest)
        //{
        //    if (loginRequest == null || string.IsNullOrEmpty(loginRequest.UserName) || string.IsNullOrEmpty(loginRequest.Password))
        //    {
        //        return Unauthorized(new { message = "Invalid username or password" });
        //    }

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connectionString))
        //        {
        //            await conn.OpenAsync();

        //            // Query to delete records from both Login and REGISTRATION tables
        //            string sqlDeleteQuery = @"
        //            DELETE FROM Login WHERE USERID = (SELECT USERID FROM REGISTRATION WHERE USERNAME = @UserName AND PASSWORD = @Password);
        //            DELETE FROM REGISTRATION WHERE USERNAME = @UserName AND PASSWORD = @Password;";

        //            using (SqlCommand cmd = new SqlCommand(sqlDeleteQuery, conn))
        //            {
        //                // Add parameters to the query
        //                cmd.Parameters.AddWithValue("@UserName", loginRequest.UserName);
        //                cmd.Parameters.AddWithValue("@Password", loginRequest.Password);

        //                // Execute the DELETE command
        //                int rowsAffected = await cmd.ExecuteNonQueryAsync();

        //                // Check if any rows were deleted
        //                if (rowsAffected > 0)
        //                {
        //                    return Ok(new { message = "User profile deleted successfully" });
        //                }
        //                else
        //                {
        //                    return Unauthorized(new { message = "Invalid username or password" });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
        //    }
        //}

        [HttpDelete("UserProfile")]
        public async Task<IActionResult> UserProfile([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.UserName) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Start a transaction to ensure both deletes succeed or fail together
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Query to get the USERID based on username and password
                            string sqlGetUserIdQuery = "SELECT USERID FROM REGISTRATION WHERE USERNAME = @UserName AND PASSWORD = @Password";

                            int userId = 0;
                            using (SqlCommand cmdGetUserId = new SqlCommand(sqlGetUserIdQuery, conn, transaction))
                            {
                                cmdGetUserId.Parameters.AddWithValue("@UserName", loginRequest.UserName);
                                cmdGetUserId.Parameters.AddWithValue("@Password", loginRequest.Password);

                                object result = await cmdGetUserId.ExecuteScalarAsync();
                                if (result == null)
                                {
                                    return Unauthorized(new { message = "Invalid username or password" });
                                }
                                userId = Convert.ToInt32(result);
                            }

                            // Delete from the Login table first (child table)
                            string sqlDeleteLogin = "DELETE FROM Login WHERE USERID = @UserID";
                            using (SqlCommand cmdDeleteLogin = new SqlCommand(sqlDeleteLogin, conn, transaction))
                            {
                                cmdDeleteLogin.Parameters.AddWithValue("@UserID", userId);
                                await cmdDeleteLogin.ExecuteNonQueryAsync();
                            }

                            // Then delete from the Registration table (parent table)
                            string sqlDeleteRegistration = "DELETE FROM REGISTRATION WHERE USERID = @UserID";
                            using (SqlCommand cmdDeleteRegistration = new SqlCommand(sqlDeleteRegistration, conn, transaction))
                            {
                                cmdDeleteRegistration.Parameters.AddWithValue("@UserID", userId);
                                await cmdDeleteRegistration.ExecuteNonQueryAsync();
                            }

                            // Commit the transaction
                            transaction.Commit();

                            return Ok(new { message = "User profile deleted successfully" });
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction in case of error
                            transaction.Rollback();
                            return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }



        [HttpPut("UserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateProfileRequest updateProfileRequest)
        {
            if (updateProfileRequest == null || string.IsNullOrEmpty(updateProfileRequest.UserName) || string.IsNullOrEmpty(updateProfileRequest.Password))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string sqlUpdateQuery = @"
                           UPDATE REGISTRATION 
                           SET NAME = @Name, GENDER = @Gender, DOB = @Dob
                           WHERE USERNAME = @UserName AND PASSWORD = @Password;

                           UPDATE Login
                           SET USERNAME = @UserName
                           WHERE USERID = (SELECT USERID FROM REGISTRATION WHERE USERNAME = @UserName AND PASSWORD = @Password);";

                    using (SqlCommand cmd = new SqlCommand(sqlUpdateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", updateProfileRequest.Name);
                        cmd.Parameters.AddWithValue("@Gender", updateProfileRequest.Gender);
                        cmd.Parameters.AddWithValue("@Dob", updateProfileRequest.DOB);
                        cmd.Parameters.AddWithValue("@UserName", updateProfileRequest.UserName);
                        cmd.Parameters.AddWithValue("@Password", updateProfileRequest.Password);

                        // Execute the UPDATE command
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            return Ok(new { message = "User profile updated successfully" });
                        }
                        else
                        {
                            return Unauthorized(new { message = "Invalid username or password" });
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

    public class UpdateProfileRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
    }

}
