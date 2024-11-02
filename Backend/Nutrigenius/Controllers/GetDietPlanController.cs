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

        [HttpPost("GetDietPlan")]
        public async Task<IActionResult> GetDietPlan([FromBody] GetDietPlan getdietplan)
        {
            if (getdietplan == null)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                // Call Python script with user details to generate diet plan
                var dietPlan = CallPythonScript(getdietplan);

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    //select max(dietId) + 1 from health table = variable

                    // Insert new record into the database, excluding UserID since it is an identity column
                    string insertSql = @"INSERT INTO UserHealthData (Age, BMI, Diabetes, Cholesterol, ThyroidDiseases, HeartDiseases, Depression, DietPlan)
                                         VALUES (@Age, @BMI, @Diabetes, @Cholesterol, @ThyroidDiseases, @HeartDiseases, @Depression, @DietPlan)";// added new field

                    //string DietPlan = dietPlan;

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

        private string CallPythonScript(GetDietPlan getdietplan) //Commented 29-10-2024

        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = @"C:\Users\HP\AppData\Local\Programs\Python\Python313\python.exe",  // Path to python.exe
                Arguments = $@"D:\NutriGenius\Model\Predict_Diet_Plan.py {getdietplan.Age} {getdietplan.BMI} {getdietplan.Diabetes} {getdietplan.Cholesterol} {getdietplan.ThyroidDiseases} {getdietplan.HeartDiseases} {getdietplan.Depression} {getdietplan.DietPlan}", // add new field
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

        //public async Task<string> CallPythonScriptAsync(GetDietPlan getdietplan)
        //{
        //    // Initialize TaskCompletionSource for handling completion
        //    var tcs = new TaskCompletionSource<string>();

        //    try
        //    {
        //        // Set up the process start info
        //        var startInfo = new ProcessStartInfo
        //        {
        //            FileName = @"C:\Users\HP\AppData\Local\Programs\Python\Python313\python.exe",
        //            Arguments = $@"D:\NutriGenius\Model\Predict_Diet_Plan.py {getdietplan.Age} {getdietplan.BMI} {getdietplan.Diabetes} {getdietplan.Cholesterol} {getdietplan.ThyroidDiseases} {getdietplan.HeartDiseases} {getdietplan.Depression} {getdietplan.DietPlan}",
        //            RedirectStandardOutput = true,
        //            RedirectStandardError = true,
        //            UseShellExecute = false,
        //            CreateNoWindow = true
        //        };

        //        // Start the process
        //        var process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

        //        // Handle process exit event
        //        process.Exited += (sender, e) =>
        //        {
        //            if (process.ExitCode == 0)
        //            {
        //                // Success: Read output asynchronously
        //                string output = process.StandardOutput.ReadToEnd();
        //                tcs.TrySetResult(output);
        //            }
        //            else
        //            {
        //                // Error: Read error message
        //                string error = process.StandardError.ReadToEnd();
        //                tcs.TrySetException(new Exception($"Python script error: {error}"));
        //            }
        //            process.Dispose();
        //        };

        //        // Start the process asynchronously
        //        if (!process.Start())
        //        {
        //            tcs.TrySetException(new Exception("Failed to start Python process."));
        //        }

        //        // Timeout in case of long-running script (e.g., 30 seconds)
        //        using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
        //        {
        //            cancellationTokenSource.Token.Register(() =>
        //            {
        //                if (!process.HasExited)
        //                {
        //                    process.Kill();
        //                    tcs.TrySetException(new TimeoutException("Python script execution timed out."));
        //                }
        //            });

        //            // Await the TaskCompletionSource to return the result
        //            return await tcs.Task;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        tcs.TrySetException(new Exception($"Error running Python script: {ex.Message}"));
        //        throw;
        //    }
        //}


        //30-10-2024 //remove this
        private async Task<int> GetmaxUserIDAsync()
        {
            int maxUserID = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = "SELECT MAX(UserID) FROM UserHealthData";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    var result = await cmd.ExecuteScalarAsync();
                    if (result != DBNull.Value)
                    {
                        maxUserID = Convert.ToInt32(result);
                    }
                }
            }

            return maxUserID;
        }


        [HttpGet("GetDietPlan")]
        public async Task<IActionResult> GetDietPlan(int userId) // new field
        {
            try
            {
                int maxUserID = await GetmaxUserIDAsync();

                // Retrieve user's health data based on userId
                GetDietPlan getdietplan = await GetUserDataById(maxUserID); // add new field

                if (getdietplan == null)
                {
                    return NotFound(new { message = "User data not found." });
                }

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Query to retrieve the user's diet plan based on the retrieved health data
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
                        selectCmd.Parameters.AddWithValue("@DietPlan", getdietplan.DietPlan); //add new field

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
                                    DietPlan = reader["DietPlan"] //add new field
                                };

                                return Ok(new { message = "Diet plan generated successfully", userHealthData });
                            }
                            else
                            {
                                return NotFound(new { message = "No diet plan found for the provided user data." });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        // Helper method to get user data based on userId
        private async Task<GetDietPlan> GetUserDataById(int userId)//add new field
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string selectSql = "SELECT Age, BMI, Diabetes, Cholesterol, ThyroidDiseases, HeartDiseases, Depression FROM UserHealthData WHERE UserID = @UserId";

                using (SqlCommand selectCmd = new SqlCommand(selectSql, conn))
                {
                    selectCmd.Parameters.AddWithValue("@UserId", userId);
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
                                DietPlan = reader.GetString(7)// add new field

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
