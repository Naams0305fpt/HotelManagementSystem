using DataAccessLayer.Common;
using Models.Entities;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IBookingReservationRepository
    {
        IEnumerable<BookingReservation> GetAll();
        BookingReservation? GetById(int id);
        IEnumerable<BookingReservation> GetByCustomerId(int customerId);
        IEnumerable<BookingReservation> GetByDateRange(DateTime start, DateTime end);
        RepositoryResult<BookingReservation> Add(BookingReservation entity);
        RepositoryResult<BookingReservation> Update(BookingReservation entity);
        RepositoryResult<bool> Delete(int id);
    }
}