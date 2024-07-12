namespace Nutrigenius.Services
{
    public class DatabaseService
    {
        // Simulated database call
        public User GetUserById(int userId)
        {
            // Simulate fetching a user from the database
            return new User { Id = userId }; // Simulated user object
        }
    }

    public class User
    {
        public int Id { get; set; }
    }
}
