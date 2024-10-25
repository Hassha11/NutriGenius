namespace Nutrigenius.Services
{
    using Nutrigenius.Models;

    public class UserService
    {
        private readonly DatabaseService _databaseService;

        public UserService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public User LoadUser(int userId)
        {
            var user = _databaseService.GetUserById(userId);
            if (user != null)
            {
                // Return the loaded user or set it to a property
                return user;
            }
            else
            {
                // Handle case where user is not found
                Console.WriteLine("User not found");
                return null; // or throw an exception as needed
            }
        }
    }
}
