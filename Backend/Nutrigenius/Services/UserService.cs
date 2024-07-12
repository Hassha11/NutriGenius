namespace Nutrigenius.Services
{
    using Nutrigenius.Models;

    public class UserService
    {
        private readonly UserContext _userContext;
        private readonly DatabaseService _databaseService;

        public UserService(UserContext userContext, DatabaseService databaseService)
        {
            _userContext = userContext;
            _databaseService = databaseService;
        }

        public void LoadUserId(int userId)
        {
            var user = _databaseService.GetUserById(userId);
            if (user != null)
            {
                _userContext.UserId = user.Id;
            }
            else
            {
                // Handle case where user is not found
                Console.WriteLine("User not found");
            }
        }
    }
}
