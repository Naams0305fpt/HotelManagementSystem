using Models.Enums;
using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public class BookingReservation
    {
        public int BookingReservationID { get; set; }
        public DateTime BookingDate { get; set; } // Ngày khách hàng thực hiện đặt
        public decimal TotalPrice { get; set; } // Tổng tiền của tất cả các phòng
        public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;

        // Khóa ngoại tới Customer
        public int CustomerID { get; set; }
        public Customer? Customer { get; set; }

        // Mối quan hệ một-nhiều
        // Một Reservation có nhiều Detail
        public ICollection<BookingDetail> BookingDetails { get; set; } = [];
    }
}