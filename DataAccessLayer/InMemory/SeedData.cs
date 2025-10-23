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
            db.RoomTypes.AddRange(
            [
                new RoomType { RoomTypeID = 1, RoomTypeName = "Standard", TypeDescription = "Basic", TypenNote = "" },
                new RoomType { RoomTypeID = 2, RoomTypeName = "Deluxe", TypeDescription = "Bigger room", TypenNote = "" },
                new RoomType { RoomTypeID = 3, RoomTypeName = "Suite", TypeDescription = "Premium suite", TypenNote = "" },
            ]);

            // Rooms
            db.Rooms.AddRange(
            [
                new Room { RoomID = 1, RoomNumber = "101", RoomDescription = "City view", RoomMaxCapacity = 2, RoomStatus = EntityStatus.Active, RoomPricePerDate = 50, RoomTypeID = 1 },
                new Room { RoomID = 2, RoomNumber = "102", RoomDescription = "Garden view", RoomMaxCapacity = 3, RoomStatus = EntityStatus.Active, RoomPricePerDate = 70, RoomTypeID = 2 },
                new Room { RoomID = 3, RoomNumber = "201", RoomDescription = "Suite, balcony", RoomMaxCapacity = 4, RoomStatus = EntityStatus.Active, RoomPricePerDate = 120, RoomTypeID = 3 },
            ]);

            // Customers (bao gồm admin mô phỏng)
            db.Customers.AddRange(
            [
                new Customer { CustomerID = 1, CustomerFullName = "System Admin", Telephone = "0000000000", EmailAddress = "admin@FUMiniHotelSystem.com", CustomerBirthday = new DateTime(1990,1,1), CustomerStatus = EntityStatus.Active, Password = "@@abc123@@" },
                new Customer { CustomerID = 2, CustomerFullName = "Nguyen Van A", Telephone = "0912345678", EmailAddress = "a@example.com", CustomerBirthday = new DateTime(2000,5,10), CustomerStatus = EntityStatus.Active, Password = "123456" },
            ]);

            // --- CẬP NHẬT LOGIC SEED BOOKING ---

            // Tạo một BookingReservation (đơn đặt hàng)
            var reservation1 = new BookingReservation
            {
                BookingReservationID = 1,
                CustomerID = 2, // Nguyen Van A
                BookingDate = DateTime.Today.AddDays(-5), // Đặt 5 ngày trước
                TotalPrice = 100, // Sẽ được tính
                BookingStatus = BookingStatus.Confirmed
            };
            db.BookingReservations.Add(reservation1);

            // Tạo chi tiết cho đơn đặt hàng đó (chỉ đặt phòng 101)
            var detail1 = new BookingDetail
            {
                BookingReservationID = 1,
                RoomID = 1, // Phòng 101
                StartDate = DateTime.Today.AddDays(1), // Bắt đầu từ ngày mai
                EndDate = DateTime.Today.AddDays(3), // Trả phòng 2 ngày sau
                ActualPrice = 50 // Giá phòng tại thời điểm đặt
            };
            db.BookingDetails.Add(detail1);

            // Tính lại tổng giá (2 đêm * 50)
            reservation1.TotalPrice = (detail1.EndDate.Date - detail1.StartDate.Date).Days * detail1.ActualPrice;
        }
    }
}