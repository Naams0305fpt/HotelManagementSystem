using BusinessLayer.Services;
using Models.Entities;
using System.Windows;
using PhamHuynhSumWPF.ViewModels;

namespace PhamHuynhSumWPF.Views.Dialogs
{
    public partial class EditRoomWindow : Window
    {
        public RoomService? RoomService { get; set; }
        public bool IsEditMode { get; set; } = false;
        public EditRoomWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Lấy entity từ DataContext
            if (DataContext is not EditRoomViewModel vm || RoomService == null)
            {
                MessageBox.Show("Error: Service not initialized.");
                return;
            }

            // Chạy logic Create hoặc Update
            DataAccessLayer.Common.RepositoryResult<Room> res;
            if (IsEditMode)
            {
                res = RoomService.Update(vm.Entity);
            }
            else
            {
                res = RoomService.Create(vm.Entity);
            }

            // --- Xử lý kết quả ---
            if (res.Success)
            {
                // Nếu thành công, set DialogResult = true để đóng cửa sổ
                this.DialogResult = true;
            }
            else
            {
                // Nếu thất bại, hiển thị lỗi
                MessageBox.Show(res.Error);
                // KHÔNG set DialogResult, cửa sổ sẽ KHÔNG đóng
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}