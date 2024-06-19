using System.ComponentModel.DataAnnotations;

namespace Nutrigenius.Models
{
    public class ForgotPassword
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
