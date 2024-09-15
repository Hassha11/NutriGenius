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
    public class LoginController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly UserContext _userContext;

        public LoginController(IConfiguration configuration, UserContext userContext)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _userContext = userContext;
        }

        //[HttpPost("Login")]  //16-09-2024
        //public async Task<IActionResult> Login([FromBody] Login login)
        //{
        //    if (login == null || string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
        //    {
        //        return Unauthorized(new { message = "0" });
        //    }

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connectionString))
        //        {
        //            await conn.OpenAsync();

        //            string query = "SELECT USERID FROM LOGIN WHERE USERNAME = @Username AND PASSWORD = @Password ";

        //            string sql = "SELECT COUNT(1) FROM Login WHERE USERNAME = @UserName AND PASSWORD = @Password";

        //            using (SqlCommand cmd = new SqlCommand(sql, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@UserName", login.UserName);
        //                cmd.Parameters.AddWithValue("@Password", login.Password);

        //                var result = await cmd.ExecuteScalarAsync();

        //                if (result != null && int.TryParse(result.ToString(), out int userCount) && userCount > 0)
        //                {
        //                    return Ok(1);
        //                }
        //                else
        //                {
        //                    return Unauthorized(new { message = "Invalid username or password." });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
        //    }
        //}

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (login == null || string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
            {
                return Unauthorized(new { message = "0" });
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // First, check the Login table
                    string userQuery = "SELECT COUNT(1) FROM Login WHERE USERNAME = @UserName AND PASSWORD = @Password";
                    using (SqlCommand userCmd = new SqlCommand(userQuery, conn))
                    {
                        userCmd.Parameters.AddWithValue("@UserName", login.UserName);
                        userCmd.Parameters.AddWithValue("@Password", login.Password);

                        var userResult = await userCmd.ExecuteScalarAsync();
                        if (userResult != null && int.TryParse(userResult.ToString(), out int userCount) && userCount > 0)
                        {
                            return Ok(new { userType = "User" });
                        }
                    }

                    // If not found in Login table, check the Dietitian Login table
                    string dietitianQuery = "SELECT COUNT(1) FROM DietLogin WHERE USERNAME = @UserName AND PASSWORD = @Password";
                    using (SqlCommand dietitianCmd = new SqlCommand(dietitianQuery, conn))
                    {
                        dietitianCmd.Parameters.AddWithValue("@UserName", login.UserName);
                        dietitianCmd.Parameters.AddWithValue("@Password", login.Password);

                        var dietitianResult = await dietitianCmd.ExecuteScalarAsync();
                        if (dietitianResult != null && int.TryParse(dietitianResult.ToString(), out int dietitianCount) && dietitianCount > 0)
                        {
                            return Ok(new { userType = "Dietitian" });
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
    }
}
