using System;
using System.Collections.Generic;
using System.Text;
using Models.Entities;
using Models.Enums;

namespace DataAccessLayer.InMemory
{
    public sealed class InMemoryDb
    {
        private static readonly Lock _lock = new();
        private static InMemoryDb? _instance;
        public static InMemoryDb Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new InMemoryDb();
                    }
                }
                return _instance;
            }
        }

        private InMemoryDb()
        {
            SeedData.Seed(this);
        }

        // --- CÁC DANH SÁCH ĐÃ CẬP NHẬT ---
        public List<Customer> Customers { get; } = [];
        public List<RoomType> RoomTypes { get; } = [];
        public List<Room> Rooms { get; } = [];
        // public List<Booking> Bookings { get; } = new(); // <<< XÓA DÒNG NÀY
        public List<BookingReservation> BookingReservations { get; } = []; // <<< THÊM DÒNG NÀY
        public List<BookingDetail> BookingDetails { get; } = []; // <<< THÊM DÒNG NÀY
        // ---------------------------------

        // --- CÁC HELPER ID ĐÃ CẬP NHẬT ---
        public int NextCustomerId => Customers.Count == 0 ? 1 : Customers[^1].CustomerID + 1;
        public int NextRoomTypeId => RoomTypes.Count == 0 ? 1 : RoomTypes[^1].RoomTypeID + 1;
        public int NextRoomId => Rooms.Count == 0 ? 1 : Rooms[^1].RoomID + 1;
        // public int NextBookingId => Bookings.Count == 0 ? 1 : Bookings[^1].BookingID + 1; // <<< XÓA
        public int NextBookingReservationId => BookingReservations.Count == 0 ? 1 : BookingReservations[^1].BookingReservationID + 1; // <<< THÊM
        // ---------------------------------
    }
}