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
    public class ForgotPasswordController : ControllerBase
    {
        private readonly string _connectionString;

        public ForgotPasswordController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //[HttpPost("ResetPassword")] //16-09-2024
        //public async Task<IActionResult> ResetPassword([FromBody] ForgotPassword forgotPassword)
        //{
        //    if (forgotPassword == null || string.IsNullOrEmpty(forgotPassword.UserName) || string.IsNullOrEmpty(forgotPassword.Password))
        //    {
        //        return BadRequest(new { message = "Invalid request" });
        //    }

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connectionString))
        //        {
        //            await conn.OpenAsync();

        //            string sql = "UPDATE Login SET Password = @Password WHERE UserName = @UserName";

        //            using (SqlCommand cmd = new SqlCommand(sql, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@Password", forgotPassword.Password); 
        //                cmd.Parameters.AddWithValue("@UserName", forgotPassword.UserName);

        //                int rowsAffected = await cmd.ExecuteNonQueryAsync();

        //                if (rowsAffected > 0)
        //                {
        //                    return Ok(new { message = "Password has been reset successfully" });
        //                }
        //                else
        //                {
        //                    return BadRequest(new { message = "Invalid username" });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
        //    }
        //}

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgotPassword forgotPassword)
        {
            if (forgotPassword == null || string.IsNullOrEmpty(forgotPassword.UserName) || string.IsNullOrEmpty(forgotPassword.Password))
            {
                return BadRequest(new { message = "Invalid request" });
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // First, attempt to update the password in the Login table
                    string userUpdateSql = "UPDATE Login SET Password = @Password WHERE UserName = @UserName";
                    using (SqlCommand userCmd = new SqlCommand(userUpdateSql, conn))
                    {
                        userCmd.Parameters.AddWithValue("@Password", forgotPassword.Password);
                        userCmd.Parameters.AddWithValue("@UserName", forgotPassword.UserName);

                        int rowsAffectedInLogin = await userCmd.ExecuteNonQueryAsync();

                        // If the update was successful in the Login table, return success
                        //if (rowsAffectedInLogin > 0)
                        //{
                        //    return Ok(new { message = "Password has been reset successfully" });
                        //}

                        if (rowsAffectedInLogin > 0)
                        {
                            // Also update the password in the Registration table if user exists in Login
                            string registrationUpdateSql = "UPDATE Registration SET Password = @Password WHERE UserName = @UserName";
                            using (SqlCommand regCmd = new SqlCommand(registrationUpdateSql, conn))
                            {
                                regCmd.Parameters.AddWithValue("@Password", forgotPassword.Password);
                                regCmd.Parameters.AddWithValue("@UserName", forgotPassword.UserName);

                                await regCmd.ExecuteNonQueryAsync(); 
                            }

                            return Ok(new { message = "Password has been reset successfully" });
                        }

                        if (rowsAffectedInLogin == 0)
                        {
                            return Ok(new { message = "Please enter the correct username" });
                        }

                    }

                    // If the username is not found in the Login table, try the Dietitian table
                    string dietitianUpdateSql = "UPDATE DietLogin SET Password = @Password WHERE UserName = @UserName";
                    using (SqlCommand dietitianCmd = new SqlCommand(dietitianUpdateSql, conn))
                    {
                        dietitianCmd.Parameters.AddWithValue("@Password", forgotPassword.Password);
                        dietitianCmd.Parameters.AddWithValue("@UserName", forgotPassword.UserName);

                        int rowsAffectedInDietitian = await dietitianCmd.ExecuteNonQueryAsync();

                        //if (rowsAffectedInDietitian > 0)
                        //{
                        //    return Ok(new { message = "Password has been reset successfully for Dietitian" });
                        //}

                        if (rowsAffectedInDietitian > 0)
                        {
                            // Also update the password in the Dietitian_Registration table
                            string dietRegUpdateSql = "UPDATE Dietitian_Registration SET Password = @Password WHERE UserName = @UserName";
                            using (SqlCommand dietRegCmd = new SqlCommand(dietRegUpdateSql, conn))
                            {
                                dietRegCmd.Parameters.AddWithValue("@Password", forgotPassword.Password);
                                dietRegCmd.Parameters.AddWithValue("@UserName", forgotPassword.UserName);

                                await dietRegCmd.ExecuteNonQueryAsync(); 
                            }

                            return Ok(new { message = "Password has been reset successfully for Dietitian" });
                        }

                        else
                        {
                            return BadRequest(new { message = "Invalid username" });
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
