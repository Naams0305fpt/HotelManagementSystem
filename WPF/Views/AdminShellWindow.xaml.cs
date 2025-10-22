using System.Windows; // Đảm bảo có using này

namespace PhamHuynhSumWPF.Views
{
    public partial class AdminShellWindow : Window
    {
        public AdminShellWindow()
        {
            InitializeComponent();
        }

        // --- THÊM PHƯƠNG THỨC NÀY ---
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Mở lại cửa sổ Login
            LoginWindow loginWindow = new();
            loginWindow.Show();

            // Đóng cửa sổ Admin hiện tại
            this.Close();
        }
    }
}