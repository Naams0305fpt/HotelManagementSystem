using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.InMemory
{
    internal static class SeedData
    {
        public static void Seed(InMemoryDb db)
        {
            // RoomTypes
            db.RoomTypes.AddRange(new[]
            {
                new RoomType { RoomTypeID = 1, RoomTypeName = "Standard", TypeDescription = "Basic", TypenNote = "" },
                new RoomType { RoomTypeID = 2, RoomTypeName = "Deluxe", TypeDescription = "Bigger room", TypenNote = "" },
                new RoomType { RoomTypeID = 3, RoomTypeName = "Suite", TypeDescription = "Premium suite", TypenNote = "" },
            });


            // Rooms
            db.Rooms.AddRange(new[]
            {
                new Room { RoomID = 1, RoomNumber = "101", RoomDescription = "City view", RoomMaxCapacity = 2, RoomStatus = EntityStatus.Active, RoomPricePerDate = 50, RoomTypeID = 1 },
                new Room { RoomID = 2, RoomNumber = "102", RoomDescription = "Garden view", RoomMaxCapacity = 3, RoomStatus = EntityStatus.Active, RoomPricePerDate = 70, RoomTypeID = 2 },
                new Room { RoomID = 3, RoomNumber = "201", RoomDescription = "Suite, balcony", RoomMaxCapacity = 4, RoomStatus = EntityStatus.Active, RoomPricePerDate = 120, RoomTypeID = 3 },
            });


            // Customers (bao gồm admin mô phỏng)
            db.Customers.AddRange(new[]
            {
                new Customer { CustomerID = 1, CustomerFullName = "System Admin", Telephone = "0000000000", EmailAddress = "admin@FUMiniHotelSystem.com", CustomerBirthday = new DateTime(1990,1,1), CustomerStatus = EntityStatus.Active, Password = "@@abc123@@" },
                new Customer { CustomerID = 2, CustomerFullName = "Nguyen Van A", Telephone = "0912345678", EmailAddress = "a@example.com", CustomerBirthday = new DateTime(2000,5,10), CustomerStatus = EntityStatus.Active, Password = "123456" },
            });


            // Bookings mẫu
            db.Bookings.AddRange(new[]
            {
                new Booking { BookingID = 1, CustomerID = 2, RoomID = 1, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(2), TotalPrice = 100, Status = BookingStatus.Confirmed },
            });
        }
    }
}
