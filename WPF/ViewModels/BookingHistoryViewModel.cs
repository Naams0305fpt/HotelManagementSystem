using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using Models.Entities;
using System.Collections.ObjectModel;
using PhamHuynhSumWPF.ViewModels.Base; // Sửa namespace nếu cần
using System; // <<< THÊM USING NÀY

namespace PhamHuynhSumWPF.ViewModels // Sửa namespace nếu cần
{
    public class BookingHistoryViewModel : ViewModelBase
    {
        private readonly BookingService _bookingService;
        private readonly int _customerId;

        public ObservableCollection<BookingReservation> Reservations { get; } = [];

        public BookingHistoryViewModel(int customerId)
        {
            _customerId = customerId;

            var reservationRepo = new BookingReservationRepository();
            var detailRepo = new BookingDetailRepository();
            var roomRepo = new RoomRepository();
            _bookingService = new BookingService(reservationRepo, detailRepo, roomRepo);

            LoadBookings();

            // --- BƯỚC 3: LẮNG NGHE SỰ KIỆN ---
            // Khi CustomerBookingViewModel gửi thông báo, hãy gọi lại hàm LoadBookings()
            CustomerBookingViewModel.OnBookingSuccess += (sender, e) => LoadBookings();
            // ---------------------------------
        }

        private void LoadBookings()
        {
            Reservations.Clear();
            var results = _bookingService.GetReservationsByCustomerId(_customerId);
            foreach (var res in results)
            {
                Reservations.Add(res);
            }
        }
    }
}