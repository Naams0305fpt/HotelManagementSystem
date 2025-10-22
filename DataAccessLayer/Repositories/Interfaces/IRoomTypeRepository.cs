using DataAccessLayer.Common;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IRoomTypeRepository
    {
        IEnumerable<RoomType> GetAll();
        RoomType? GetById(int id);
        RepositoryResult<RoomType> Add(RoomType entity);
        RepositoryResult<RoomType> Update(RoomType entity);
        RepositoryResult<bool> Delete(int id);
    }
}
