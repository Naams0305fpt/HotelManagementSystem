using DataAccessLayer.Common;
using DataAccessLayer.InMemory;
using DataAccessLayer.Repositories.Interfaces;
using Models.Entities;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly InMemoryDb _db = InMemoryDb.Instance;


        public IEnumerable<Customer> GetAll() => _db.Customers.Where(c => true);


        public Customer? GetById(int id) => _db.Customers.FirstOrDefault(c => c.CustomerID == id);


        public Customer? GetByEmail(string email) => _db.Customers.FirstOrDefault(c => c.EmailAddress == email);


        public RepositoryResult<Customer> Add(Customer entity)
        {
            entity.CustomerID = _db.NextCustomerId;
            _db.Customers.Add(entity);
            return RepositoryResult<Customer>.Ok(entity);
        }


        public RepositoryResult<Customer> Update(Customer entity)
        {
            var existing = GetById(entity.CustomerID);
            if (existing == null) return RepositoryResult<Customer>.Fail("Customer not found");


            existing.CustomerFullName = entity.CustomerFullName;
            existing.Telephone = entity.Telephone;
            existing.EmailAddress = entity.EmailAddress;
            existing.CustomerBirthday = entity.CustomerBirthday;
            existing.CustomerStatus = entity.CustomerStatus;
            existing.Password = entity.Password;


            return RepositoryResult<Customer>.Ok(existing);
        }


        public RepositoryResult<bool> Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return RepositoryResult<bool>.Fail("Customer not found");
            _db.Customers.Remove(existing);
            return RepositoryResult<bool>.Ok(true);
        }
    }
}
