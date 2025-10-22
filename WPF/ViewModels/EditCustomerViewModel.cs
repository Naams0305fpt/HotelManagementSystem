using Models.Entities;
using System.ComponentModel;
using PhamHuynhSumWPF.ViewModels.Base;
using Models.Enums;

namespace PhamHuynhSumWPF.ViewModels
{
    public class EditCustomerViewModel : ViewModelBase
    {
        // Property này sẽ được binding (liên kết) với các ô TextBox/DatePicker trên View
        public Customer Entity { get; set; }

        // Constructor này dùng cho chứcn năng "Add" (Thêm mới)
        public EditCustomerViewModel()
        {
            Entity = new Customer
            {
                CustomerBirthday = DateTime.Now.AddYears(-20), // Giá trị mặc định
                CustomerStatus = EntityStatus.Active // 1 = Active [cite: 16]
            };
        }

        // Constructor này dùng cho chức năng "Edit" (Chỉnh sửa)
        public EditCustomerViewModel(Customer customer)
        {
            // Rất quan trọng: Phải tạo một bản sao (clone)
            // để nếu người dùng nhấn "Cancel",
            // dữ liệu gốc trong danh sách không bị thay đổi.
            Entity = new Customer
            {
                CustomerID = customer.CustomerID,
                CustomerFullName = customer.CustomerFullName,
                Telephone = customer.Telephone,
                EmailAddress = customer.EmailAddress,
                CustomerBirthday = customer.CustomerBirthday,
                CustomerStatus = customer.CustomerStatus,
                Password = customer.Password // Giữ lại mật khẩu cũ
            };
        }
    }
}