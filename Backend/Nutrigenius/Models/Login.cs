using System.ComponentModel.DataAnnotations;

namespace Nutrigenius.Models
{
    public class Login
    {
        [Key]
       // public int UserID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
