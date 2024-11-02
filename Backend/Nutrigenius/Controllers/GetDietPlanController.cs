using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Nutrigenius.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Data;

namespace Nutrigenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetDietPlanController : ControllerBase
    {
        private readonly string _connectionString;

        public GetDietPlanController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private int GetNextDietID()
        {
            int nextDietID = 1;
            string query = "SELECT MAX(DietID) FROM UserHealthData";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        int maxDietID = Convert.ToInt32(result);
                        nextDietID = maxDietID + 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving next DietID: " + ex.Message);
                }
            }

            return nextDietID;
        }

        [HttpPost("GetDietPlan")]
        public async Task<IActionResult> GetDietPlan([FromBody] GetDietPlan getdietplan)
        {
            if (getdietplan == null)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                // Call Python script to generate diet plan
                var dietPlan = CallPythonScript(getdietplan);

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    int maxDietID = GetNextDietID();

                    string insertSql = @"INSERT INTO UserHealthData (Age, BMI, Diabetes, Cholesterol, ThyroidDiseases, HeartDiseases, Depression, DietPlan, DietID)
                                         VALUES (@Age, @BMI, @Diabetes, @Cholesterol, @ThyroidDiseases, @HeartDiseases, @Depression, @DietPlan, @DietID)";

                    using (SqlCommand insertCmd = new SqlCommand(insertSql, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@Age", getdietplan.Age);
                        insertCmd.Parameters.AddWithValue("@BMI", getdietplan.BMI);
                        insertCmd.Parameters.AddWithValue("@Diabetes", getdietplan.Diabetes);
                        insertCmd.Parameters.AddWithValue("@Cholesterol", getdietplan.Cholesterol);
                        insertCmd.Parameters.AddWithValue("@ThyroidDiseases", getdietplan.ThyroidDiseases);
                        insertCmd.Parameters.AddWithValue("@HeartDiseases", getdietplan.HeartDiseases);
                        insertCmd.Parameters.AddWithValue("@Depression", getdietplan.Depression);
                        insertCmd.Parameters.AddWithValue("@DietPlan", dietPlan);
                        insertCmd.Parameters.AddWithValue("@DietID", maxDietID);

                        int rowsInserted = await insertCmd.ExecuteNonQueryAsync();

                        if (rowsInserted > 0)
                        {
                            return Ok(new { message = "Records saved successfully", dietPlan });
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to save record");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        private string CallPythonScript(GetDietPlan getdietplan)
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = @"C:\Users\HP\AppData\Local\Programs\Python\Python313\python.exe",
                Arguments = $@"D:\NutriGenius\Model\Predict_Diet_Plan.py {getdietplan.Age} {getdietplan.BMI} {getdietplan.Diabetes} {getdietplan.Cholesterol} {getdietplan.ThyroidDiseases} {getdietplan.HeartDiseases} {getdietplan.Depression}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = @"D:\NutriGenius\Model"
            };

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(result))
                    {
                        string error = process.StandardError.ReadToEnd();
                        throw new Exception("Python script error: " + error);
                    }
                    return result;
                }
            }
        }

        [HttpGet("GetDietPlan")]
        public async Task<IActionResult> GetDietPlan(int maxDietID = 0)
        {
            try
            {
                if (maxDietID == 0)
                {
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        await conn.OpenAsync();
                        string latestDietIdSql = "SELECT MAX(DietID) FROM UserHealthData";

                        using (SqlCommand cmd = new SqlCommand(latestDietIdSql, conn))
                        {
                            object result = await cmd.ExecuteScalarAsync();
                            maxDietID = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        }
                    }
                }

                if (maxDietID == 0)
                {
                    return NotFound(new { message = "No diet plan found." });
                }

                // Retrieve the user's health data using the `maxDietID`
                GetDietPlan getdietplan = await GetUserDataById(maxDietID);
                if (getdietplan == null)
                {
                    return NotFound(new { message = "User data not found." });
                }

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string selectSql = @"SELECT TOP 1 * FROM UserHealthData
                                 WHERE Age = @Age AND BMI = @BMI AND Diabetes = @Diabetes 
                                 AND Cholesterol = @Cholesterol AND ThyroidDiseases = @ThyroidDiseases 
                                 AND HeartDiseases = @HeartDiseases AND Depression = @Depression
                                 ORDER BY UserID DESC";

                    using (SqlCommand selectCmd = new SqlCommand(selectSql, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@Age", getdietplan.Age);
                        selectCmd.Parameters.AddWithValue("@BMI", getdietplan.BMI);
                        selectCmd.Parameters.AddWithValue("@Diabetes", getdietplan.Diabetes);
                        selectCmd.Parameters.AddWithValue("@Cholesterol", getdietplan.Cholesterol);
                        selectCmd.Parameters.AddWithValue("@ThyroidDiseases", getdietplan.ThyroidDiseases);
                        selectCmd.Parameters.AddWithValue("@HeartDiseases", getdietplan.HeartDiseases);
                        selectCmd.Parameters.AddWithValue("@Depression", getdietplan.Depression);

                        using (SqlDataReader reader = await selectCmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                var userHealthData = new
                                {
                                    UserID = reader["UserID"],
                                    Age = reader["Age"],
                                    BMI = reader["BMI"],
                                    Diabetes = reader["Diabetes"],
                                    Cholesterol = reader["Cholesterol"],
                                    ThyroidDiseases = reader["ThyroidDiseases"],
                                    HeartDiseases = reader["HeartDiseases"],
                                    Depression = reader["Depression"],
                                    DietPlan = reader["DietPlan"],
                                    DietID = reader["DietID"]
                                };

                                return Ok(new { message = "Diet plan generated successfully", userHealthData });
                            }
                        }
                    }
                }

                // If no matching record is found
                return NotFound(new { message = "No diet plan found for the provided user data." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }



        //[HttpGet("GetDietPlan")]
        //public async Task<IActionResult> GetDietPlan(int maxDietID)
        //{
        //    try
        //    {
        //        GetDietPlan getdietplan = await GetUserDataById(maxDietID);

        //        if (getdietplan == null)
        //        {
        //            return NotFound(new { message = "User data not found." });
        //        }

        //        using (SqlConnection conn = new SqlConnection(_connectionString))
        //        {
        //            await conn.OpenAsync();

        //            string selectSql = @"SELECT TOP 1 * FROM UserHealthData
        //                                 WHERE Age = @Age AND BMI = @BMI AND Diabetes = @Diabetes 
        //                                 AND Cholesterol = @Cholesterol AND ThyroidDiseases = @ThyroidDiseases 
        //                                 AND HeartDiseases = @HeartDiseases AND Depression = @Depression
        //                                 ORDER BY UserID DESC";

        //            using (SqlCommand selectCmd = new SqlCommand(selectSql, conn))
        //            {
        //                selectCmd.Parameters.AddWithValue("@Age", getdietplan.Age);
        //                selectCmd.Parameters.AddWithValue("@BMI", getdietplan.BMI);
        //                selectCmd.Parameters.AddWithValue("@Diabetes", getdietplan.Diabetes);
        //                selectCmd.Parameters.AddWithValue("@Cholesterol", getdietplan.Cholesterol);
        //                selectCmd.Parameters.AddWithValue("@ThyroidDiseases", getdietplan.ThyroidDiseases);
        //                selectCmd.Parameters.AddWithValue("@HeartDiseases", getdietplan.HeartDiseases);
        //                selectCmd.Parameters.AddWithValue("@Depression", getdietplan.Depression);

        //                using (SqlDataReader reader = await selectCmd.ExecuteReaderAsync())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        var userHealthData = new
        //                        {
        //                            UserID = reader["UserID"],
        //                            Age = reader["Age"],
        //                            BMI = reader["BMI"],
        //                            Diabetes = reader["Diabetes"],
        //                            Cholesterol = reader["Cholesterol"],
        //                            ThyroidDiseases = reader["ThyroidDiseases"],
        //                            HeartDiseases = reader["HeartDiseases"],
        //                            Depression = reader["Depression"],
        //                            DietPlan = reader["DietPlan"],
        //                            DietID = reader["DietID"]
        //                        };

        //                        return Ok(new { message = "Diet plan generated successfully", userHealthData });
        //                    }
        //                    else
        //                    {
        //                        return NotFound(new { message = "No diet plan found for the provided user data." });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        //    }
        //}

        private async Task<GetDietPlan> GetUserDataById(int dietID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string selectSql = "SELECT Age, BMI, Diabetes, Cholesterol, ThyroidDiseases, HeartDiseases, Depression, DietPlan, DietID FROM UserHealthData WHERE DietID = @DietID";

                using (SqlCommand selectCmd = new SqlCommand(selectSql, conn))
                {
                    selectCmd.Parameters.AddWithValue("@DietID", dietID);
                    using (SqlDataReader reader = await selectCmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new GetDietPlan
                            {
                                Age = reader.GetInt32(0),
                                BMI = (double)reader.GetDecimal(1),
                                Diabetes = reader.GetInt32(2),
                                Cholesterol = reader.GetInt32(3),
                                ThyroidDiseases = reader.GetInt32(4),
                                HeartDiseases = reader.GetInt32(5),
                                Depression = reader.GetInt32(6),
                                DietPlan = reader.GetString(7),
                                DietID = reader.GetInt32(8)
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
