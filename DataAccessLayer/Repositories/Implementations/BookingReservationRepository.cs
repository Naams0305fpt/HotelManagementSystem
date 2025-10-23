using DataAccessLayer.Common;
using DataAccessLayer.InMemory;
using DataAccessLayer.Repositories.Interfaces;
using Models.Entities;
using System.Collections.Generic;
using System.Linq; // Thêm
using System; // Thêm

namespace DataAccessLayer.Repositories.Implementations
{
    public class BookingReservationRepository : IBookingReservationRepository
    {
        private readonly InMemoryDb _db = InMemoryDb.Instance;

        public IEnumerable<BookingReservation> GetAll() => _db.BookingReservations;

        public BookingReservation? GetById(int id) => _db.BookingReservations.FirstOrDefault(r => r.BookingReservationID == id);

        public IEnumerable<BookingReservation> GetByCustomerId(int customerId)
            => _db.BookingReservations.Where(r => r.CustomerID == customerId);

        public IEnumerable<BookingReservation> GetByDateRange(DateTime start, DateTime end)
            => _db.BookingReservations.Where(r => r.BookingDate.Date >= start.Date && r.BookingDate.Date <= end.Date);

        public RepositoryResult<BookingReservation> Add(BookingReservation entity)
        {
            entity.BookingReservationID = _db.NextBookingReservationId;
            _db.BookingReservations.Add(entity);
            return RepositoryResult<BookingReservation>.Ok(entity);
        }

        public RepositoryResult<BookingReservation> Update(BookingReservation entity)
        {
            var existing = GetById(entity.BookingReservationID);
            if (existing == null) return RepositoryResult<BookingReservation>.Fail("Reservation not found");

            existing.BookingDate = entity.BookingDate;
            existing.TotalPrice = entity.TotalPrice;
            existing.BookingStatus = entity.BookingStatus;
            existing.CustomerID = entity.CustomerID;

            return RepositoryResult<BookingReservation>.Ok(existing);
        }

        public RepositoryResult<bool> Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return RepositoryResult<bool>.Fail("Reservation not found");

            // Cũng nên xóa các chi tiết (details) liên quan
            var details = _db.BookingDetails.Where(d => d.BookingReservationID == id).ToList();
            foreach (var detail in details)
            {
                _db.BookingDetails.Remove(detail);
            }

            _db.BookingReservations.Remove(existing);
            return RepositoryResult<bool>.Ok(true);
        }
    }
}