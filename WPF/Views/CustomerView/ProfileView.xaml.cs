using PhamHuynhSumWPF.ViewModels; // Sửa namespace nếu cần
using System.Windows;
using System.Windows.Controls;

namespace PhamHuynhSumWPF.Views.CustomerView // Sửa namespace nếu cần
{
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            InitializeComponent();
        }

        // --- THÊM PHƯƠNG THỨC NÀY ---
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy ViewModel từ DataContext
            if (DataContext is not ProfileViewModel viewModel) return;

            // --- ĐÂY LÀ BƯỚC QUAN TRỌNG ---
            // Cập nhật mật khẩu trong ViewModel bằng giá trị từ PasswordBox
            // TRƯỚC KHI gọi hàm Save
            if (!string.IsNullOrEmpty(txtPassword.Password))
            {
                viewModel.Entity.Password = txtPassword.Password;
            }
            // ------------------------------

            // Gọi hàm Save() công khai của ViewModel
            viewModel.Save();
        }
    }
}