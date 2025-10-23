using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using Models.Entities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using PhamHuynhSumWPF.Helpers; // Sửa namespace nếu cần
using PhamHuynhSumWPF.ViewModels.Base; // Sửa namespace nếu cần

namespace PhamHuynhSumWPF.ViewModels // Sửa namespace nếu cần
{
    public class CustomerBookingViewModel : ViewModelBase
    {
        // --- BƯỚC 1: KHAI BÁO SỰ KIỆN TĨNH ---
        public static event EventHandler? OnBookingSuccess;
        // ------------------------------------

        private readonly int _customerId;
        private readonly RoomService _roomService;
        private readonly BookingService _bookingService;

        // ... (Các thuộc tính và Command giữ nguyên) ...
        private DateTime _startDate = DateTime.Today.AddDays(1);
        public DateTime StartDate { get => _startDate; set => Set(ref _startDate, value); }
        private DateTime _endDate = DateTime.Today.AddDays(2);
        public DateTime EndDate { get => _endDate; set => Set(ref _endDate, value); }
        public ObservableCollection<Room> AvailableRooms { get; } = [];
        private Room? _selectedRoom;
        public Room? SelectedRoom { get => _selectedRoom; set => Set(ref _selectedRoom, value); }
        public RelayCommand SearchCommand => new(_ => SearchAvailability());
        public RelayCommand BookNowCommand => new(_ => BookNow(), _ => SelectedRoom != null);

        public CustomerBookingViewModel(int customerId)
        {
            _customerId = customerId;

            var roomRepo = new RoomRepository();
            var roomTypeRepo = new RoomTypeRepository();
            var detailRepo = new BookingDetailRepository();
            var reservationRepo = new BookingReservationRepository();

            _roomService = new RoomService(roomRepo, roomTypeRepo, detailRepo);
            _bookingService = new BookingService(reservationRepo, detailRepo, roomRepo);

            SearchAvailability();
        }

        private void SearchAvailability()
        {
            AvailableRooms.Clear();
            if (StartDate.Date >= EndDate.Date)
            {
                MessageBox.Show("End Date must be after Start Date.");
                return;
            }

            var rooms = _roomService.GetAvailableRooms(StartDate, EndDate);
            foreach (var room in rooms)
            {
                AvailableRooms.Add(room);
            }
        }

        private void BookNow()
        {
            if (SelectedRoom == null) return;

            var result = _bookingService.CreateSingleRoomBooking(
                _customerId,
                SelectedRoom.RoomID,
                StartDate,
                EndDate);

            if (result.Success && result.Data != null)
            {
                MessageBox.Show($"Booking successful! Reservation ID: {result.Data.BookingReservationID}\nTotal Price: {result.Data.TotalPrice:C}");
                SearchAvailability();

                // --- BƯỚC 2: KÍCH HOẠT SỰ KIỆN ---
                // Gửi thông báo "vừa đặt phòng xong" cho các ViewModel khác
                OnBookingSuccess?.Invoke(this, EventArgs.Empty);
                // --------------------------------
            }
            else
            {
                MessageBox.Show($"Booking failed: {result.Error}");
            }
        }
    }
}