﻿using System.ComponentModel.DataAnnotations;

namespace Nutrigenius.Models

{
    public class UserData
    {
        public int Age { get; set; }
        public double BMI { get; set; }
        public int Diabetes { get; set; }
        public int Cholesterol { get; set; }
        public int ThyroidDiseases { get; set; }
        public int HeartDiseases { get; set; }
        public int Depression { get; set; }
    }
}
