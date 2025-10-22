using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using Models.Entities;
using System.Collections.ObjectModel;
using PhamHuynhSumWPF.Helpers;
using PhamHuynhSumWPF.ViewModels.Base;

namespace PhamHuynhSumWPF.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        private readonly BookingService _bookingService;

        public ObservableCollection<Booking> Bookings { get; } = [];

        private DateTime _startDate = DateTime.Today.AddMonths(-1);
        public DateTime StartDate { get => _startDate; set => Set(ref _startDate, value); }

        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate { get => _endDate; set => Set(ref _endDate, value); }

        public RelayCommand LoadReportCommand => new(_ => LoadReport());

        public ReportViewModel()
        {
            var bookingRepo = new BookingRepository();
            var roomRepo = new RoomRepository();
            _bookingService = new BookingService(bookingRepo, roomRepo);
            LoadReport();
        }

        private void LoadReport()
        {
            Bookings.Clear();
            var results = _bookingService.GetBookingsForReport(StartDate, EndDate);
            foreach (var booking in results)
            {
                Bookings.Add(booking);
            }
        }
    }
}