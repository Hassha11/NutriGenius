using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Nutrigenius.Models;

namespace Nutrigenius.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DietController : ControllerBase
    {
        [HttpPost("get-diet-plan")]
        public IActionResult GetDietPlan([FromBody] UserData userData)
        {
            if (userData == null)
            {
                return BadRequest("Invalid user data.");
            }

            // Call Python script with user details
            try
            {
                var dietPlan = CallPythonScript(userData);
                return Ok(new { dietPlan });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string CallPythonScript(UserData userData)
        {
            // Create process to run Python script
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\HP\AppData\Local\Programs\Python\Python313\python.exe";  // Full path to python.exe
            start.Arguments = @$"D:\NutriGenius\Model\Predict_Diet_Plan.py {userData.Age} {userData.BMI} {userData.Diabetes} {userData.Cholesterol} {userData.ThyroidDiseases} {userData.HeartDiseases} {userData.Depression}";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true; // Capture any error output from the Python script
            start.WorkingDirectory = @"D:\NutriGenius\Model";  // Set working directory

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(result))
                    {
                        // Read error output if result is empty
                        string error = process.StandardError.ReadToEnd();
                        throw new Exception("Python script error: " + error);
                    }
                    return result;
                }
            }
        }
    }
}
