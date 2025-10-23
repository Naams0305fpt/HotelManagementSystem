using DataAccessLayer.Common;
using DataAccessLayer.InMemory;
using DataAccessLayer.Repositories.Interfaces;
using Models.Entities;
using System.Collections.Generic;
using System.Linq; // Thêm
using System; // Thêm

namespace DataAccessLayer.Repositories.Implementations
{
    public class BookingDetailRepository : IBookingDetailRepository
    {
        private readonly InMemoryDb _db = InMemoryDb.Instance;

        public IEnumerable<BookingDetail> GetAll() => _db.BookingDetails;

        public IEnumerable<BookingDetail> GetByReservationId(int reservationId)
            => _db.BookingDetails.Where(d => d.BookingReservationID == reservationId);

        public IEnumerable<BookingDetail> GetConflictingBookings(DateTime start, DateTime end)
            => _db.BookingDetails.Where(b => b.StartDate.Date < end.Date && b.EndDate.Date > start.Date);

        public RepositoryResult<BookingDetail> Add(BookingDetail entity)
        {
            // Kiểm tra khóa (giả định)
            var existing = _db.BookingDetails.FirstOrDefault(d =>
                d.BookingReservationID == entity.BookingReservationID && d.RoomID == entity.RoomID);

            if (existing != null)
                return RepositoryResult<BookingDetail>.Fail("Booking detail for this room already exists in the reservation.");

            _db.BookingDetails.Add(entity);
            return RepositoryResult<BookingDetail>.Ok(entity);
        }
    }
}