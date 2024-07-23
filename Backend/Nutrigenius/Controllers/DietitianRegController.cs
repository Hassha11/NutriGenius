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
    public class DietitianRegController : ControllerBase
    {
        private readonly string _connectionString;

        public DietitianRegController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("DietRegistration")]
        public async Task<IActionResult> Registration([FromBody] DietitianReg Dietregistration)
        {
            if (Dietregistration == null || string.IsNullOrEmpty(Dietregistration.Name) || string.IsNullOrEmpty(Dietregistration.Gender) || string.IsNullOrEmpty(Dietregistration.UserName) || string.IsNullOrEmpty(Dietregistration.Password) || string.IsNullOrEmpty(Dietregistration.ConfirmPassword) || string.IsNullOrEmpty(Dietregistration.Qualifications))
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
                        // Insert into Dietitian Registration table
                        string sqlInsertRegistration = @"
                            INSERT INTO DIETITIAN_REGISTRATION (NAME, GENDER, USERNAME, PASSWORD, CONFIRMPASSWORD, QUALIFICATIONS) 
                            OUTPUT INSERTED.USERID
                            VALUES (@Name, @Gender, @UserName, @Password, @ConfirmPass, @Qualifications)";

                        int newUserId;
                        using (SqlCommand cmd = new SqlCommand(sqlInsertRegistration, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Name", Dietregistration.Name);
                            cmd.Parameters.AddWithValue("@Gender", Dietregistration.Gender);
                            cmd.Parameters.AddWithValue("@UserName", Dietregistration.UserName);
                            cmd.Parameters.AddWithValue("@Password", Dietregistration.Password);
                            cmd.Parameters.AddWithValue("@ConfirmPass", Dietregistration.ConfirmPassword);
                            cmd.Parameters.AddWithValue("@Qualifications", Dietregistration.Qualifications);

                            object result = await cmd.ExecuteScalarAsync();
                            if (result == null)
                            {
                                throw new Exception("Failed to retrieve new user ID.");
                            }

                            newUserId = (int)result;

                            // Log the newUserId for debugging purposes
                            Console.WriteLine("Inserted UserID: " + newUserId);
                        }

                        // Insert into Login table
                        string sqlInsertLogin = @"
                            INSERT INTO DietLogin (USERID, USERNAME, PASSWORD) 
                            VALUES (@UserID, @UserName, @Password)";

                        using (SqlCommand cmd = new SqlCommand(sqlInsertLogin, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserID", newUserId);
                            cmd.Parameters.AddWithValue("@UserName", Dietregistration.UserName);
                            cmd.Parameters.AddWithValue("@Password", Dietregistration.Password);

                            await cmd.ExecuteNonQueryAsync();
                        }

                        // Commit the transaction
                        transaction.Commit();

                        return Ok(1);
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if any error occurs
                        transaction.Rollback();
                        // Log the error message for debugging purposes
                        Console.WriteLine("Transaction Error: " + ex.Message);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error message for debugging purposes
                Console.WriteLine("Database Error: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }
    }
}
