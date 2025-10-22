using DataAccessLayer.Common;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAll();
        Booking? GetById(int id);
        IEnumerable<Booking> GetByDateRange(DateTime start, DateTime end);
        IEnumerable<Booking> GetByCustomerId(int customerId); // <-- THÊM DÒNG NÀY
        RepositoryResult<Booking> Add(Booking entity);
        RepositoryResult<Booking> Update(Booking entity);
        RepositoryResult<bool> Delete(int id);
    }
}