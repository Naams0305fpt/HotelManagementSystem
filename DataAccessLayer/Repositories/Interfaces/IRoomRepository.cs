using DataAccessLayer.Common;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetAll();
        Room? GetById(int id);
        Room? GetByRoomNumber(string roomNumber); // <<< THÊM DÒNG NÀY
        RepositoryResult<Room> Add(Room entity);
        RepositoryResult<Room> Update(Room entity);
        RepositoryResult<bool> Delete(int id);
    }
}