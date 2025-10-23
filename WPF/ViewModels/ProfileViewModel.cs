using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using Models.Entities;
using System.Windows;
using PhamHuynhSumWPF.Helpers; // Sửa namespace nếu cần
using PhamHuynhSumWPF.ViewModels.Base; // Sửa namespace nếu cần
using Customer = Models.Entities.Customer;

namespace PhamHuynhSumWPF.ViewModels // Sửa namespace nếu cần
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly CustomerService _service;
        public Customer Entity { get; private set; }
        private readonly Customer _original;

        // --- BỎ COMMAND TỪ ĐÂY ---
        // public RelayCommand SaveCommand => new(_ => Save());

        public ProfileViewModel(Customer loggedInCustomer)
        {
            var repo = new CustomerRepository();
            var reservationRepo = new BookingReservationRepository();
            _service = new CustomerService(repo, reservationRepo);

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

        // --- ĐỔI THÀNH "PUBLIC" ---
        public void Save()
        {
            var res = _service.Update(Entity);
            if (res.Success)
            {
                // Cập nhật lại bản gốc nếu thành công
                _original.CustomerFullName = Entity.CustomerFullName;
                _original.Telephone = Entity.Telephone;
                _original.EmailAddress = Entity.EmailAddress;
                _original.CustomerBirthday = Entity.CustomerBirthday;
                _original.Password = Entity.Password; // Dòng này giờ sẽ chứa mật khẩu MỚI
                MessageBox.Show("Profile updated successfully!");
            }
            else
            {
                MessageBox.Show(res.Error);
            }
        }
    }
}