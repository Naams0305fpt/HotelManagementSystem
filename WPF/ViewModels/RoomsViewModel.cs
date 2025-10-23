using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using Models.Entities;
using System.Collections.ObjectModel;
using System.Windows;
using PhamHuynhSumWPF.Helpers;
using PhamHuynhSumWPF.ViewModels.Base;

namespace PhamHuynhSumWPF.ViewModels
{
    public class RoomsViewModel : ViewModelBase
    {
        private readonly RoomService _service;
        private readonly RoomTypeRepository _roomTypeRepo = new(); // Dùng tạm repo

        public ObservableCollection<Room> Rooms { get; } = [];
        private Room? _selectedRoom;
        public Room? SelectedRoom { get => _selectedRoom; set => Set(ref _selectedRoom, value); }

        private string _keyword = string.Empty;
        public string Keyword { get => _keyword; set { Set(ref _keyword, value); Search(); } }

        public RelayCommand SearchCommand => new(_ => Search());
        public RelayCommand AddCommand => new(_ => Add());
        public RelayCommand EditCommand => new(_ => Edit(), _ => SelectedRoom != null);
        public RelayCommand DeleteCommand => new(_ => Delete(), _ => SelectedRoom != null);

        // --- CẬP NHẬT CONSTRUCTOR ---
        public RoomsViewModel()
        {
            var repo = new RoomRepository();
            var roomTypeRepo = new RoomTypeRepository();
            var bookingDetailRepo = new BookingDetailRepository(); // <<< THÊM DÒNG NÀY
            _service = new RoomService(repo, roomTypeRepo, bookingDetailRepo); // <<< CẬP NHẬT THAM SỐ
            Load();
        }
        // ------------------------------

        private void Load()
        {
            Rooms.Clear();
            foreach (var r in _service.GetAllRooms()) Rooms.Add(r);
        }

        private void Search()
        {
            Rooms.Clear();
            foreach (var r in _service.Search(Keyword)) Rooms.Add(r);
        }

        private void Add()
        {
            var allTypes = _service.GetAllRoomTypes();
            var vm = new EditRoomViewModel(allTypes);
            var dlg = new Views.Dialogs.EditRoomWindow
            {
                DataContext = vm,
                // --- THÊM 2 DÒNG NÀY ---
                RoomService = _service,
                IsEditMode = false
            };
            if (dlg.ShowDialog() == true)
            {
                Load();
            }
        }

        private void Edit()
        {
            if (SelectedRoom == null) return;
            var allTypes = _service.GetAllRoomTypes();
            var vm = new EditRoomViewModel(SelectedRoom, allTypes);
            var dlg = new Views.Dialogs.EditRoomWindow
            {
                DataContext = vm,
                // --- THÊM 2 DÒNG NÀY ---
                RoomService = _service,
                IsEditMode = true
            };
            if (dlg.ShowDialog() == true)
            {
                Load();
            }
        }

        private void Delete()
        {
            if (SelectedRoom == null) return;
            if (MessageBox.Show($"Delete {SelectedRoom.RoomNumber}?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var res = _service.Delete(SelectedRoom.RoomID);
                if (!res.Success) MessageBox.Show(res.Error);
                Load();
            }
        }
    }
}