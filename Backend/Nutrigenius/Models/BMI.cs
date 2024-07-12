namespace Nutrigenius.Models
{
    public class BMI
    {
        public int UserId { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal Bmi { get; set; }
        public string Status { get; set; }

    }
}
