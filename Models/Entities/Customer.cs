using Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerFullName { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty; // max 12
        public string EmailAddress { get; set; } = string.Empty; // max 50
        public DateTime CustomerBirthday { get; set; }
        public EntityStatus CustomerStatus { get; set; } = EntityStatus.Active;
        public string Password { get; set; } = string.Empty; // max 50
    }
}
