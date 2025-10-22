using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using Models.Entities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using PhamHuynhSumWPF.Helpers;
using PhamHuynhSumWPF.ViewModels.Base;

namespace PhamHuynhSumWPF.ViewModels
{
    public class CustomerBookingViewModel : ViewModelBase
    {
        private readonly int _customerId;
        private readonly RoomService _roomService;
        private readonly BookingService _bookingService;

        // Thuộc tính cho DatePickers
        private DateTime _startDate = DateTime.Today.AddDays(1);
        public DateTime StartDate { get => _startDate; set => Set(ref _startDate, value); }

        private DateTime _endDate = DateTime.Today.AddDays(2);
        public DateTime EndDate { get => _endDate; set => Set(ref _endDate, value); }

        // Danh sách phòng trống
        public ObservableCollection<Room> AvailableRooms { get; } = [];

        // Phòng được chọn
        private Room? _selectedRoom;
        public Room? SelectedRoom { get => _selectedRoom; set => Set(ref _selectedRoom, value); }

        // Lệnh (Commands)
        public RelayCommand SearchCommand => new(_ => SearchAvailability());
        public RelayCommand BookNowCommand => new(_ => BookNow(), _ => SelectedRoom != null);

        public CustomerBookingViewModel(int customerId)
        {
            _customerId = customerId;

            // Khởi tạo các services
            var roomRepo = new RoomRepository();
            var roomTypeRepo = new RoomTypeRepository();
            var bookingRepo = new BookingRepository();

            _roomService = new RoomService(roomRepo, roomTypeRepo, bookingRepo);
            _bookingService = new BookingService(bookingRepo, roomRepo);

            SearchAvailability(); // Tải danh sách phòng trống lần đầu
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

            var newBooking = new Booking
            {
                CustomerID = _customerId,
                RoomID = SelectedRoom.RoomID,
                StartDate = StartDate,
                EndDate = EndDate,
                Status = Models.Enums.BookingStatus.Pending // Hoặc Confirmed nếu muốn
            };

            // Gọi BookingService để tạo và tính giá
            var result = _bookingService.Create(newBooking);

            if (result.Success && result.Data != null)
            {
                MessageBox.Show($"Booking successful! Total Price: {result.Data.TotalPrice:C}");
                // Sau khi đặt thành công, tìm lại phòng trống (phòng vừa đặt sẽ biến mất)
                SearchAvailability();
            }
            else
            {
                MessageBox.Show($"Booking failed: {result.Error}");
            }
        }
    }
}