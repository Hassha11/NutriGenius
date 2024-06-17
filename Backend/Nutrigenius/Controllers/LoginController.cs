using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public LoginController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login login)
        {

            try
            {
                //string connectionString = "Server=DESKTOP-KLCTTP5\\SQLEXPRESS01;Database=Nutrigenius;Trusted_Connection=True;";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string sql = "INSERT INTO LOGIN (USERNAME, PASSWORD) VALUES (@UserName, @Password)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", login.UserName);
                        cmd.Parameters.AddWithValue("@Password", login.Password);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            return Ok("Data Inserted");
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, "Error inserting data");
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
