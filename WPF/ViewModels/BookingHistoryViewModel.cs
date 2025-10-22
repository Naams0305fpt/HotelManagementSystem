using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using Models.Entities;
using System.Collections.ObjectModel;
using PhamHuynhSumWPF.ViewModels.Base;

namespace PhamHuynhSumWPF.ViewModels
{
    public class BookingHistoryViewModel : ViewModelBase
    {
        private readonly BookingService _bookingService;
        private readonly int _customerId;

        public ObservableCollection<Booking> Bookings { get; } = [];

        public BookingHistoryViewModel(int customerId)
        {
            _customerId = customerId;
            var bookingRepo = new BookingRepository();
            var roomRepo = new RoomRepository();
            _bookingService = new BookingService(bookingRepo, roomRepo);

            LoadBookings();
        }

        private void LoadBookings()
        {
            Bookings.Clear();
            var results = _bookingService.GetByCustomerId(_customerId);
            foreach (var booking in results)
            {
                Bookings.Add(booking);
            }
        }
    }
}