using Microsoft.IdentityModel.Tokens;
using Nutrigenius.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class AuthServices
{
    private readonly UserContext _userContext;

    public AuthServices(UserContext userContext)
    {
        _userContext = userContext;
    }

    //public async Task<string> Authenticate(string username, string password)
    //{
    //    // Validate the user credentials
    //    var user = await _userContext.GetUserByUsernameAndPassword(username, password);
    //    if (user == null)
    //    {
    //        throw new UnauthorizedAccessException("Invalid credentials");
    //    }

    //    // Generate and return the token
    //    return GenerateToken(user);
    //}

    public string GenerateToken(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User ID
        new Claim(ClaimTypes.Name, user.Username ?? string.Empty) // Username (fallback to empty if null)
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hass@123#$3344sas098jhfgbdSayu4@34d")); 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "Nutrigenius",
            audience: "NutrigeniusApp",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
    }

}
