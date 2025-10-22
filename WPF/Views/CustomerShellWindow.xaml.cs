using Customer = Models.Entities.Customer;
using System.Windows;
using PhamHuynhSumWPF.ViewModels;

namespace PhamHuynhSumWPF.Views
{
    public partial class CustomerShellWindow : Window
    {
        private readonly Customer? _loggedInCustomer;

        public CustomerShellWindow()
        {
            InitializeComponent();
        }

        public CustomerShellWindow(Customer customer)
        {
            InitializeComponent();
            _loggedInCustomer = customer;

            if (_loggedInCustomer != null)
            {
                CustomerBookingTab.DataContext = new CustomerBookingViewModel(_loggedInCustomer.CustomerID);
                ProfileViewTab.DataContext = new ProfileViewModel(_loggedInCustomer);
                BookingHistoryTab.DataContext = new BookingHistoryViewModel(_loggedInCustomer.CustomerID);
            }
        }

        // --- THÊM PHƯƠNG THỨC NÀY ---
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Mở lại cửa sổ Login
            LoginWindow loginWindow = new();
            loginWindow.Show();

            // Đóng cửa sổ Customer hiện tại
            this.Close();
        }
    }
}