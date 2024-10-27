﻿using Microsoft.AspNetCore.Http;
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
    public class DietController : ControllerBase
    {
        private readonly string _connectionString;

        public DietController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("get-diet-plan")]
        public async Task<IActionResult> GetDietPlan([FromBody] UserData userData)
        {
            if (userData == null)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                // Call Python script with user details to generate diet plan
                var dietPlan = CallPythonScript(userData);

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Insert new record into the database, excluding UserID since it is an identity column
                    string insertSql = @"INSERT INTO UserHealthData (Age, BMI, Diabetes, Cholesterol, ThyroidDiseases, HeartDiseases, Depression, DietPlan)
                                         VALUES (@Age, @BMI, @Diabetes, @Cholesterol, @ThyroidDiseases, @HeartDiseases, @Depression, @DietPlan)";

                    //string DietPlan = dietPlan;

                    using (SqlCommand insertCmd = new SqlCommand(insertSql, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@Age", userData.Age);
                        insertCmd.Parameters.AddWithValue("@BMI", userData.BMI);
                        insertCmd.Parameters.AddWithValue("@Diabetes", userData.Diabetes);
                        insertCmd.Parameters.AddWithValue("@Cholesterol", userData.Cholesterol);
                        insertCmd.Parameters.AddWithValue("@ThyroidDiseases", userData.ThyroidDiseases);
                        insertCmd.Parameters.AddWithValue("@HeartDiseases", userData.HeartDiseases);
                        insertCmd.Parameters.AddWithValue("@Depression", userData.Depression);
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

        private string CallPythonScript(UserData userData)

        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = @"C:\Users\HP\AppData\Local\Programs\Python\Python313\python.exe",  // Path to python.exe
                Arguments = $@"D:\NutriGenius\Model\Predict_Diet_Plan.py {userData.Age} {userData.BMI} {userData.Diabetes} {userData.Cholesterol} {userData.ThyroidDiseases} {userData.HeartDiseases} {userData.Depression} {userData.DietPlan}",
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

        [HttpGet("get-user-diet-plan")]
        public async Task<IActionResult> GetUserDietPlan(int userId)
        {
            try
            {
                // Retrieve user's health data based on userId
                UserData userData = await GetUserDataById(userId);

                if (userData == null)
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
                        selectCmd.Parameters.AddWithValue("@Age", userData.Age);
                        selectCmd.Parameters.AddWithValue("@BMI", userData.BMI);
                        selectCmd.Parameters.AddWithValue("@Diabetes", userData.Diabetes);
                        selectCmd.Parameters.AddWithValue("@Cholesterol", userData.Cholesterol);
                        selectCmd.Parameters.AddWithValue("@ThyroidDiseases", userData.ThyroidDiseases);
                        selectCmd.Parameters.AddWithValue("@HeartDiseases", userData.HeartDiseases);
                        selectCmd.Parameters.AddWithValue("@Depression", userData.Depression);
                        selectCmd.Parameters.AddWithValue("@DietPlan", userData.DietPlan);

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
                                    DietPlan = reader["DietPlan"]
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
        private async Task<UserData> GetUserDataById(int userId)
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
                            return new UserData
                            {
                                Age = reader.GetInt32(0), 
                                BMI = (double)reader.GetDecimal(1), 
                                Diabetes = reader.GetInt32(2), 
                                Cholesterol = reader.GetInt32(3), 
                                ThyroidDiseases = reader.GetInt32(4), 
                                HeartDiseases = reader.GetInt32(5), 
                                Depression = reader.GetInt32(6),
                                DietPlan = reader.GetString(7)

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
