namespace Nutrigenius.Models
{
    public class BMI
    {
        public int UserID { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal Bmi { get; set; }
        public string Status { get; set; }
    }
}
