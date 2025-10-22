using BusinessLayer.Validation;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Common;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repo;
        public CustomerService(ICustomerRepository repo) { _repo = repo; }

        // --- PHƯƠNG THỨC MỚI ---
        public IEnumerable<Customer> GetAll()
        {
            return _repo.GetAll();
        }

        // --- PHƯƠNG THỨC MỚI ---
        public Customer? GetByEmail(string email)
        {
            return _repo.GetByEmail(email);
        }

        // --- PHƯƠNG THỨC MỚI ---
        public IEnumerable<Customer> Search(string keyword)
        {
            var k = keyword.ToLower();
            return _repo.GetAll()
                .Where(c => (c.CustomerFullName?.ToLower().Contains(k) ?? false)
                         || (c.EmailAddress?.ToLower().Contains(k) ?? false));
        }

        public RepositoryResult<Customer> Create(Customer c)
        {
            var error = Validate(c, isNew: true);
            if (error != null) return RepositoryResult<Customer>.Fail(error);
            return _repo.Add(c);
        }

        public RepositoryResult<Customer> Update(Customer c)
        {
            var error = Validate(c, isNew: false);
            if (error != null) return RepositoryResult<Customer>.Fail(error);
            return _repo.Update(c);
        }

        // --- PHƯƠNG THỨC MỚI ---
        public RepositoryResult<bool> Delete(int id)
        {
            var customer = _repo.GetById(id);
            if (customer == null) return RepositoryResult<bool>.Fail("Customer not found.");
            // (Bạn có thể thêm logic kiểm tra, ví dụ: không cho xóa nếu khách hàng có booking)
            return _repo.Delete(id);
        }

        // --- LOGIC VALIDATE ĐÃ ĐƯỢC CẬP NHẬT ---
        private string? Validate(Customer c, bool isNew)
        {
            if (!Validators.NotEmpty(c.CustomerFullName) || !Validators.MaxLength(c.CustomerFullName, 50))
                return "FullName is required (<=50).";
            if (!Validators.IsValidPhone(c.Telephone) || !Validators.MaxLength(c.Telephone, 12))
                return "Telephone must be 8-12 digits.";
            if (!Validators.IsValidEmail(c.EmailAddress) || !Validators.MaxLength(c.EmailAddress, 50))
                return "Invalid Email (<=50).";
            if (!Validators.MaxLength(c.Password, 50))
                return "Password max length 50.";

            // --- Logic kiểm tra Email đã được sửa ---
            var existingByEmail = _repo.GetByEmail(c.EmailAddress);
            if (isNew)
            {
                if (existingByEmail != null)
                    return "Email already exists.";
            }
            else // isNew == false (Updating)
            {
                // Email này đã tồn tại VÀ nó thuộc về một khách hàng KHÁC
                if (existingByEmail != null && existingByEmail.CustomerID != c.CustomerID)
                    return "Email is already taken by another customer.";
            }
            // --- Hết phần sửa logic Email ---

            if (c.CustomerBirthday > DateTime.Today)
                return "Birthday cannot be in the future.";
            return null;
        }
    }
}