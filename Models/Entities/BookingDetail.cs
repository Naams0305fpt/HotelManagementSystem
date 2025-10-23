using System;

namespace Models.Entities
{
    public class BookingDetail
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ActualPrice { get; set; } // Giá thực tế của phòng này

        // Khóa ngoại kép (Composite Key)
        public int BookingReservationID { get; set; }
        public int RoomID { get; set; }

        // Thuộc tính điều hướng
        public BookingReservation? BookingReservation { get; set; }
        public Room? Room { get; set; }
    }
}