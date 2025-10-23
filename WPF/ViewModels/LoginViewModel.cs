using System;
using System.Windows;
using PhamHuynhSumWPF.ViewModels.Base;
using PhamHuynhSumWPF.Helpers;
using PhamHuynhSumWPF.Views;
using DataAccessLayer.Repositories.Implementations;
using BusinessLayer.Services;
using Models.Entities; // <-- THÊM USING NÀY

namespace PhamHuynhSumWPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _email = string.Empty;
        public string Email { get => _email; set => Set(ref _email, value); }

        private string _password = string.Empty;
        public string Password { get => _password; set => Set(ref _password, value); }

        private string _errorMessage = string.Empty;
        public string ErrorMessage { get => _errorMessage; set => Set(ref _errorMessage, value); }

        public event EventHandler? RequestClose;

        public RelayCommand LoginCommand => new(_ => DoLogin());

        private readonly CustomerService _customerService;

        // --- CẬP NHẬT CONSTRUCTOR ---
        public LoginViewModel()
        {
            var customerRepo = new CustomerRepository();
            var reservationRepo = new BookingReservationRepository(); // <<< THÊM DÒNG NÀY
            _customerService = new CustomerService(customerRepo, reservationRepo); // <<< CẬP NHẬT THAM SỐ
        }
        // ------------------------------

        private void DoLogin()
        {
            ErrorMessage = string.Empty;

            if (Email.Equals(Config.AdminEmail, StringComparison.OrdinalIgnoreCase)
            && Password == Config.AdminPassword)
            {
                // Admin không cần truyền dữ liệu
                NavigateTo(new AdminShellWindow());
                return;
            }

            var user = _customerService.GetByEmail(Email);
            if (user != null && user.Password == Password)
            {
                // --- SỬA Ở ĐÂY ---
                // Truyền đối tượng Customer đã đăng nhập vào cửa sổ CustomerShellWindow
                NavigateTo(new CustomerShellWindow(user));
                // -----------------
                return;
            }

            ErrorMessage = "Invalid email or password";
        }

        private void NavigateTo(Window window)
        {
            window.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}