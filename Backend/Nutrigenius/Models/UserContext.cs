using Microsoft.EntityFrameworkCore;
using Nutrigenius.Models;
using System.Threading.Tasks;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public int? UserId { get; set; } // Add this property to hold UserId

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    public async Task<User> GetUserByUsernameAndPassword(string username, string password)
    {
        // Replace with secure password checking, such as comparing hashes
        var user = await Users
            .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        return user;
    }
}
