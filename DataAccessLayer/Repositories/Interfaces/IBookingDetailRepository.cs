using DataAccessLayer.Common;
using Models.Entities;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IBookingDetailRepository
    {
        IEnumerable<BookingDetail> GetAll();
        IEnumerable<BookingDetail> GetByReservationId(int reservationId);
        IEnumerable<BookingDetail> GetConflictingBookings(DateTime start, DateTime end);
        RepositoryResult<BookingDetail> Add(BookingDetail entity);
        // Cập nhật và Xóa cho BookingDetail thường phức tạp hơn (dựa trên composite key)
        // Hiện tại chúng ta chỉ cần Add
    }
}