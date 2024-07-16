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
    public class MealsController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly UserContext _userContext;

        public MealsController(IConfiguration configuration, UserContext userContext)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _userContext = userContext;
        }

        [HttpPost("Diet")]
        public async Task<IActionResult> Diet([FromBody] DietPlan diet)
        {
            if (diet == null || string.IsNullOrEmpty(diet.Diebetes) || string.IsNullOrEmpty(diet.Cholesterol) || string.IsNullOrEmpty(diet.Thyroid) || string.IsNullOrEmpty(diet.Heart) || string.IsNullOrEmpty(diet.Depression))
            {
                return Unauthorized(new { message = "0" });
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string sql = "INSERT INTO Diet (Diebetes, Cholesterol, Thyroid, Heart, Depression, Points) " +
                         "VALUES (@Diebetes, @Cholesterol, @Thyroid, @Heart, @Depression, @Points);";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Diebetes", diet.Diebetes);
                        cmd.Parameters.AddWithValue("@Cholesterol", diet.Cholesterol);
                        cmd.Parameters.AddWithValue("@Thyroid", diet.Thyroid);
                        cmd.Parameters.AddWithValue("@Heart", diet.Heart);
                        cmd.Parameters.AddWithValue("@Depression", diet.Depression);
                        //cmd.Parameters.AddWithValue("@Points", diet.Points);

                        int userCount = (int)await cmd.ExecuteScalarAsync();

                        if (userCount > 0)
                        {
                            return Ok(1); // Return the username if login is successful
                        }
                        else
                        {
                            return Unauthorized(new { message = "1" });
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
