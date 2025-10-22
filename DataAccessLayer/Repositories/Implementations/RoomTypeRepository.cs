using DataAccessLayer.Common;
using DataAccessLayer.InMemory;
using DataAccessLayer.Repositories.Interfaces;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories.Implementations
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly InMemoryDb _db = InMemoryDb.Instance;


        public IEnumerable<RoomType> GetAll() => _db.RoomTypes;


        public RoomType? GetById(int id) => _db.RoomTypes.FirstOrDefault(r => r.RoomTypeID == id);


        public RepositoryResult<RoomType> Add(RoomType entity)
        {
            entity.RoomTypeID = _db.NextRoomTypeId;
            _db.RoomTypes.Add(entity);
            return RepositoryResult<RoomType>.Ok(entity);
        }


        public RepositoryResult<RoomType> Update(RoomType entity)
        {
            var existing = GetById(entity.RoomTypeID);
            if (existing == null) return RepositoryResult<RoomType>.Fail("RoomType not found");
            existing.RoomTypeName = entity.RoomTypeName;
            existing.TypeDescription = entity.TypeDescription;
            existing.TypenNote = entity.TypenNote;
            return RepositoryResult<RoomType>.Ok(existing);
        }


        public RepositoryResult<bool> Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return RepositoryResult<bool>.Fail("RoomType not found");
            _db.RoomTypes.Remove(existing);
            return RepositoryResult<bool>.Ok(true);
        }
    }
}
