using DataAccessLayer.Common;
using DataAccessLayer.InMemory;
using DataAccessLayer.Repositories.Interfaces;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories.Implementations
{
    public class RoomRepository : IRoomRepository
    {
        private readonly InMemoryDb _db = InMemoryDb.Instance;

        public IEnumerable<Room> GetAll() => _db.Rooms;

        public Room? GetById(int id) => _db.Rooms.FirstOrDefault(r => r.RoomID == id);

        // --- THÊM PHƯƠNG THỨC MỚI NÀY ---
        public Room? GetByRoomNumber(string roomNumber)
        {
            return _db.Rooms.FirstOrDefault(r => r.RoomNumber.Equals(roomNumber, StringComparison.OrdinalIgnoreCase));
        }
        // ---------------------------------

        public RepositoryResult<Room> Add(Room entity)
        {
            entity.RoomID = _db.NextRoomId;
            _db.Rooms.Add(entity);
            return RepositoryResult<Room>.Ok(entity);
        }

        public RepositoryResult<Room> Update(Room entity)
        {
            var existing = GetById(entity.RoomID);
            if (existing == null) return RepositoryResult<Room>.Fail("Room not found");

            existing.RoomNumber = entity.RoomNumber;
            existing.RoomDescription = entity.RoomDescription;
            existing.RoomMaxCapacity = entity.RoomMaxCapacity;
            existing.RoomStatus = entity.RoomStatus;
            existing.RoomPricePerDate = entity.RoomPricePerDate;
            existing.RoomTypeID = entity.RoomTypeID;

            return RepositoryResult<Room>.Ok(existing);
        }

        public RepositoryResult<bool> Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return RepositoryResult<bool>.Fail("Room not found");
            _db.Rooms.Remove(existing);
            return RepositoryResult<bool>.Ok(true);
        }
    }
}