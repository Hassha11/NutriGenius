﻿using Microsoft.AspNetCore.Http;
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

                    string query = "SELECT USERID FROM LOGIN WHERE USERNAME = @Username AND PASSWORD = @Password ";

                    string sql = "SELECT COUNT(1) FROM Login WHERE USERNAME = @UserName AND PASSWORD = @Password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", login.UserName);
                        cmd.Parameters.AddWithValue("@Password", login.Password);

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