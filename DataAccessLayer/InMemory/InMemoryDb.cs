using System;
using System.Collections.Generic;
using System.Text;
using Models.Entities;
using Models.Enums;

namespace DataAccessLayer.InMemory
{
    public sealed class InMemoryDb
    {
        private static readonly object _lock = new();
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


        public List<Customer> Customers { get; } = new();
        public List<RoomType> RoomTypes { get; } = new();
        public List<Room> Rooms { get; } = new();
        public List<Booking> Bookings { get; } = new();


        // Auto-increment helpers
        public int NextCustomerId => Customers.Count == 0 ? 1 : Customers[^1].CustomerID + 1;
        public int NextRoomTypeId => RoomTypes.Count == 0 ? 1 : RoomTypes[^1].RoomTypeID + 1;
        public int NextRoomId => Rooms.Count == 0 ? 1 : Rooms[^1].RoomID + 1;
        public int NextBookingId => Bookings.Count == 0 ? 1 : Bookings[^1].BookingID + 1;
    }
}
