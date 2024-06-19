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

                    string sql = "UPDATE Login SET Password = @Password WHERE UserName = @UserName";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Password", forgotPassword.Password); // In a real app, hash the password before storing it
                        cmd.Parameters.AddWithValue("@UserName", forgotPassword.UserName);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            return Ok(new { message = "Password has been reset successfully" });
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
