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
    public class DietPlanController : ControllerBase
    {
        private readonly string _connectionString;

        public DietPlanController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("DietPlan")]
        public async Task<IActionResult> DietPlan([FromBody] DietPlan dietplan)
        {
            if (dietplan == null || string.IsNullOrEmpty(dietplan.Diebetes) || string.IsNullOrEmpty(dietplan.Cholesterol) || string.IsNullOrEmpty(dietplan.Thyroid) || string.IsNullOrEmpty(dietplan.Heart) || string.IsNullOrEmpty(dietplan.Depression))
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
                        // Insert into DietPlan table
                        string sqlInsertRegistration = @"
                            INSERT INTO Registration (NAME, GENDER, DOB, USERNAME, PASSWORD, CONFIRMPASS) 
                            OUTPUT INSERTED.USERID
                            VALUES (@Name, @Gender, @DOB, @UserName, @Password, @ConfirmPass)";

                        int newUserId;
                        using (SqlCommand cmd = new SqlCommand(sqlInsertRegistration, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Diebetes", dietplan.Diebetes);
                            cmd.Parameters.AddWithValue("@Cholesterol", dietplan.Cholesterol);
                            cmd.Parameters.AddWithValue("@Thyroid", dietplan.Thyroid);
                            cmd.Parameters.AddWithValue("@Heart", dietplan.Heart);
                            cmd.Parameters.AddWithValue("@Depression", dietplan.Depression);

                            newUserId = (int)await cmd.ExecuteScalarAsync();
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
