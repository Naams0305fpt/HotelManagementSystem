using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer.Validation
{
    public static class Validators
    {
        public static bool IsValidEmail(string email)
        => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");


        public static bool IsValidPhone(string phone)
        => Regex.IsMatch(phone, @"^[0-9]{8,12}$");


        public static bool NotEmpty(string value) => !string.IsNullOrWhiteSpace(value);


        public static bool MaxLength(string value, int max)
        => string.IsNullOrEmpty(value) || value.Length <= max;


        public static bool InRange<T>(T value, T min, T max) where T : IComparable<T>
        => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
    }
}
