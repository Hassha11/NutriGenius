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
    public class RegistrationController : ControllerBase
    {
        private readonly string _connectionString;

        public RegistrationController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] Registration registration)
        {
            if (registration == null || string.IsNullOrEmpty(registration.Name) || string.IsNullOrEmpty(registration.Gender) || string.IsNullOrEmpty(registration.DOB) || string.IsNullOrEmpty(registration.UserName) || string.IsNullOrEmpty(registration.Password) || string.IsNullOrEmpty(registration.ConfirmPass))
            {
                return Unauthorized(new { message = "0" });
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Start a transaction
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // Insert into Registration table
                        string sqlInsertRegistration = @"
                            INSERT INTO Registration (NAME, GENDER, DOB, USERNAME, PASSWORD, CONFIRMPASS) 
                            OUTPUT INSERTED.USERID
                            VALUES (@Name, @Gender, @DOB, @UserName, @Password, @ConfirmPass)";

                        int newUserId;
                        using (SqlCommand cmd = new SqlCommand(sqlInsertRegistration, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Name", registration.Name);
                            cmd.Parameters.AddWithValue("@Gender", registration.Gender);
                            cmd.Parameters.AddWithValue("@DOB", registration.DOB);
                            cmd.Parameters.AddWithValue("@UserName", registration.UserName);
                            cmd.Parameters.AddWithValue("@Password", registration.Password);
                            cmd.Parameters.AddWithValue("@ConfirmPass", registration.ConfirmPass);

                            newUserId = (int)await cmd.ExecuteScalarAsync();
                        }

                        // Insert into Login table
                        string sqlInsertLogin = @"
                            INSERT INTO Login (USERID, USERNAME, PASSWORD) 
                            VALUES (@UserID, @UserName, @Password)";

                        using (SqlCommand cmd = new SqlCommand(sqlInsertLogin, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserID", newUserId);
                            cmd.Parameters.AddWithValue("@UserName", registration.UserName);
                            cmd.Parameters.AddWithValue("@Password", registration.Password);

                            await cmd.ExecuteNonQueryAsync();
                        }

                        // Commit the transaction
                        transaction.Commit();

                        return Ok(1);
                    }
                    catch (Exception)
                    {
                        // Rollback the transaction if any error occurs
                        transaction.Rollback();
                        throw;
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
