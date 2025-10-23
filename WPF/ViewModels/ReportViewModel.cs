using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using Models.Entities;
using PhamHuynhSumWPF.Helpers;
using PhamHuynhSumWPF.ViewModels.Base;
using System;
using System.Collections.ObjectModel;

namespace PhamHuynhSumWPF.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        private readonly BookingService _bookingService;

        // --- THAY ĐỔI KIỂU DỮ LIỆU ---
        public ObservableCollection<BookingReservation> Reservations { get; } = [];

        private DateTime _startDate = DateTime.Today.AddMonths(-1);
        public DateTime StartDate { get => _startDate; set => Set(ref _startDate, value); }

        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate { get => _endDate; set => Set(ref _endDate, value); }

        public RelayCommand LoadReportCommand => new(_ => LoadReport());

        public ReportViewModel()
        {
            // Khởi tạo service mới
            var reservationRepo = new BookingReservationRepository();
            var detailRepo = new BookingDetailRepository();
            var roomRepo = new RoomRepository();
            _bookingService = new BookingService(reservationRepo, detailRepo, roomRepo);

            LoadReport();
        }

        private void LoadReport()
        {
            Reservations.Clear();
            // Gọi phương thức service mới
            var results = _bookingService.GetReservationsForReport(StartDate, EndDate);
            foreach (var res in results)
            {
                Reservations.Add(res);
            }
        }
    }
}