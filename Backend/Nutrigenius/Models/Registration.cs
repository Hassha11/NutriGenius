using System.ComponentModel.DataAnnotations;

namespace Nutrigenius.Models
{
    public class Registration
    {
        [Key]
        //public int UserID { get; set; }

        public string Name { get; set; }
        
        public string Gender { get; set; }

        public string DOB { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPass { get; set; }

    }
}





