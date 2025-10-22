using BusinessLayer.Services;
using DataAccessLayer.Repositories.Implementations;
using Models.Entities;
using System.Windows;
using PhamHuynhSumWPF.Helpers;
using PhamHuynhSumWPF.ViewModels.Base;

namespace PhamHuynhSumWPF.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly CustomerService _service;

        // Dùng Entity để binding cho Edit, và Original để giữ bản gốc
        public Customer Entity { get; private set; }
        private readonly Customer _original;

        public RelayCommand SaveCommand => new(_ => Save());

        public ProfileViewModel(Customer loggedInCustomer)
        {
            var repo = new CustomerRepository();
            _service = new CustomerService(repo);

            _original = loggedInCustomer;
            // Tạo bản sao (clone) để chỉnh sửa, tránh ảnh hưởng
            Entity = new Customer
            {
                CustomerID = _original.CustomerID,
                CustomerFullName = _original.CustomerFullName,
                Telephone = _original.Telephone,
                EmailAddress = _original.EmailAddress,
                CustomerBirthday = _original.CustomerBirthday,
                CustomerStatus = _original.CustomerStatus,
                Password = _original.Password // Giữ lại mật khẩu
            };
        }

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