using DataAccessLayer.Common;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Customer? GetById(int id);
        Customer? GetByEmail(string email);
        RepositoryResult<Customer> Add(Customer entity);
        RepositoryResult<Customer> Update(Customer entity);
        RepositoryResult<bool> Delete(int id);
    }
}
