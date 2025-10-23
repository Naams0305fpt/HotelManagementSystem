using DataAccessLayer.Common;
using DataAccessLayer.Repositories.Interfaces;
using Models.Entities;
using Models.Enums; // <<< THÊM
using System;
using System.Collections.Generic;
using System.Linq; // <<< THÊM
using System.Text;

namespace BusinessLayer.Services
{
    // Lớp này được viết lại hoàn toàn
    public class BookingService(IBookingReservationRepository reservationRepo,
                          IBookingDetailRepository detailRepo,
                          IRoomRepository roomRepo)
    {
        private readonly IBookingReservationRepository _reservationRepo = reservationRepo;
        private readonly IBookingDetailRepository _detailRepo = detailRepo;
        private readonly IRoomRepository _roomRepo = roomRepo;

        // Dùng cho Customer: Xem lịch sử
        public IEnumerable<BookingReservation> GetReservationsByCustomerId(int customerId)
        {
            // Lấy tất cả các đơn đặt hàng của khách
            var reservations = _reservationRepo.GetByCustomerId(customerId).OrderByDescending(r => r.BookingDate).ToList();

            // Lấy tất cả chi tiết
            var allDetails = _detailRepo.GetAll().ToList();

            // Gán chi tiết vào đơn đặt hàng
            foreach (var res in reservations)
            {
                res.BookingDetails = [.. allDetails.Where(d => d.BookingReservationID == res.BookingReservationID)];
            }
            return reservations;
        }

        // Dùng cho Admin: Báo cáo
        public IEnumerable<BookingReservation> GetReservationsForReport(DateTime start, DateTime end)
        {
            // Lấy các đơn đặt hàng trong khoảng ngày (dựa trên BookingDate)
            var reservations = _reservationRepo.GetByDateRange(start, end).OrderByDescending(r => r.TotalPrice).ToList();
            var allDetails = _detailRepo.GetAll().ToList();

            // Gán chi tiết vào
            foreach (var res in reservations)
            {
                res.BookingDetails = [.. allDetails.Where(d => d.BookingReservationID == res.BookingReservationID)];
            }
            return reservations;
        }

        // Dùng cho Customer: Đặt 1 phòng
        public RepositoryResult<BookingReservation> CreateSingleRoomBooking(int customerId, int roomId, DateTime startDate, DateTime endDate)
        {
            // 1. Validation
            var room = _roomRepo.GetById(roomId);
            if (room == null) return RepositoryResult<BookingReservation>.Fail("Selected room does not exist.");

            if (startDate.Date >= endDate.Date) return RepositoryResult<BookingReservation>.Fail("End Date must be after Start Date.");

            // 2. Kiểm tra phòng có trống không
            var conflictingBookings = _detailRepo.GetConflictingBookings(startDate, endDate);
            if (conflictingBookings.Any(b => b.RoomID == roomId))
            {
                return RepositoryResult<BookingReservation>.Fail("Room is not available for the selected dates.");
            }

            // 3. Tính toán
            int nights = (endDate.Date - startDate.Date).Days;
            decimal actualPrice = room.RoomPricePerDate;
            decimal totalPrice = nights * actualPrice;

            // 4. Tạo Reservation (Đơn hàng)
            var reservation = new BookingReservation
            {
                CustomerID = customerId,
                BookingDate = DateTime.Now,
                TotalPrice = totalPrice,
                BookingStatus = BookingStatus.Confirmed // Tự động xác nhận
            };

            // 5. Lưu Reservation (để lấy ID)
            var resResult = _reservationRepo.Add(reservation);
            if (!resResult.Success) return resResult; // Trả về lỗi nếu không add được

            if (resResult.Data == null)
            {
                return RepositoryResult<BookingReservation>.Fail("Failed to create reservation (data was null).");
            }

            // 6. Tạo Detail (Chi tiết)
            var detail = new BookingDetail
            {
                BookingReservationID = resResult.Data.BookingReservationID,
                RoomID = roomId,
                StartDate = startDate,
                EndDate = endDate,
                ActualPrice = actualPrice
            };

            // 7. Lưu Detail
            var detailResult = _detailRepo.Add(detail);
            if (!detailResult.Success)
            {
                // Rollback: Nếu lưu detail thất bại, xóa Reservation đã tạo
                _reservationRepo.Delete(resResult.Data.BookingReservationID);
                return RepositoryResult<BookingReservation>.Fail($"Failed to add booking detail: {detailResult.Error}");
            }

            // 8. Trả về Reservation đã tạo thành công
            return resResult;
        }
    }
}