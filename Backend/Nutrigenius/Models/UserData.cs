using System.ComponentModel.DataAnnotations;

namespace Nutrigenius.Models

{
    public class UserData
    {
        public int Age { get; set; }
        public double BMI { get; set; }
        public string Diabetes { get; set; }
        public string Cholesterol { get; set; }
        public string ThyroidDiseases { get; set; }
        public string HeartDiseases { get; set; }
        public string Depression { get; set; }
    }
}
