using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using Models.Entities;
using System.Windows;
using PhamHuynhSumWPF.Helpers;
using PhamHuynhSumWPF.ViewModels.Base;
using Customer = Models.Entities.Customer;

namespace PhamHuynhSumWPF.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly CustomerService _service;

        // Dùng Entity để binding cho Edit, và Original để giữ bản gốc
        public Customer Entity { get; private set; }
        private readonly Customer _original;

        public RelayCommand SaveCommand => new(_ => Save());

        // --- CẬP NHẬT CONSTRUCTOR ---
        public ProfileViewModel(Customer loggedInCustomer)
        {
            var repo = new CustomerRepository();
            var reservationRepo = new BookingReservationRepository(); // <<< THÊM DÒNG NÀY
            _service = new CustomerService(repo, reservationRepo); // <<< CẬP NHẬT THAM SỐ
            // ------------------------------

            _original = loggedInCustomer;
            Entity = new Customer
            {
                CustomerID = _original.CustomerID,
                CustomerFullName = _original.CustomerFullName,
                Telephone = _original.Telephone,
                EmailAddress = _original.EmailAddress,
                CustomerBirthday = _original.CustomerBirthday,
                CustomerStatus = _original.CustomerStatus,
                Password = _original.Password
            };
        }
        // ------------------------------

        private void Save()
        {
            var res = _service.Update(Entity);
            if (res.Success)
            {
                // Cập nhật lại bản gốc nếu thành công
                _original.CustomerFullName = Entity.CustomerFullName;
                _original.Telephone = Entity.Telephone;
                _original.EmailAddress = Entity.EmailAddress;
                _original.CustomerBirthday = Entity.CustomerBirthday;
                _original.Password = Entity.Password;
                MessageBox.Show("Profile updated successfully!");
            }
            else
            {
                MessageBox.Show(res.Error);
            }
        }
    }
}