using DataAccessLayer.Common;
using DataAccessLayer.InMemory;
using DataAccessLayer.Repositories.Interfaces;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly InMemoryDb _db = InMemoryDb.Instance;

        public IEnumerable<Booking> GetAll() => _db.Bookings;

        public Booking? GetById(int id) => _db.Bookings.FirstOrDefault(b => b.BookingID == id);

        public IEnumerable<Booking> GetByDateRange(DateTime start, DateTime end)
        => _db.Bookings.Where(b => b.StartDate.Date >= start.Date && b.EndDate.Date <= end.Date);

        // --- THÊM PHƯƠNG THỨC NÀY ---
        public IEnumerable<Booking> GetByCustomerId(int customerId)
            => _db.Bookings.Where(b => b.CustomerID == customerId);
        // -----------------------------

        public RepositoryResult<Booking> Add(Booking entity)
        {
            entity.BookingID = _db.NextBookingId;
            _db.Bookings.Add(entity);
            return RepositoryResult<Booking>.Ok(entity);
        }

        public RepositoryResult<Booking> Update(Booking entity)
        {
            var existing = GetById(entity.BookingID);
            if (existing == null) return RepositoryResult<Booking>.Fail("Booking not found");

            existing.CustomerID = entity.CustomerID;
            existing.RoomID = entity.RoomID;
            existing.StartDate = entity.StartDate;
            existing.EndDate = entity.EndDate;
            existing.TotalPrice = entity.TotalPrice;
            existing.Status = entity.Status;

            return RepositoryResult<Booking>.Ok(existing);
        }

        public RepositoryResult<bool> Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return RepositoryResult<bool>.Fail("Booking not found");
            _db.Bookings.Remove(existing);
            return RepositoryResult<bool>.Ok(true);
        }
    }
}