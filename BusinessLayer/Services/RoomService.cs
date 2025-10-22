using BusinessLayer.Validation;
using DataAccessLayer.Common;
using DataAccessLayer.Repositories.Interfaces;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class RoomService(IRoomRepository repo,
                       IRoomTypeRepository roomTypeRepo,
                       IBookingRepository bookingRepo)
    {
        private readonly IRoomRepository _repo = repo;
        private readonly IRoomTypeRepository _roomTypeRepo = roomTypeRepo;
        private readonly IBookingRepository _bookingRepo = bookingRepo;

        public IEnumerable<Room> GetAvailableRooms(DateTime start, DateTime end)
        {
            if (start.Date >= end.Date)
            {
                return []; // Trả về danh sách trống nếu ngày không hợp lệ
            }

            // Lấy tất cả các booking có xung đột thời gian
            var conflictingBookings = _bookingRepo.GetAll()
                .Where(b => b.StartDate.Date < end.Date && b.EndDate.Date > start.Date)
                .ToList();

            // Lấy ID của các phòng đã bị đặt
            var bookedRoomIds = conflictingBookings.Select(b => b.RoomID).Distinct();

            // Lấy tất cả các phòng và join với RoomType
            var allRooms = GetAllRooms();

            // Trả về các phòng KHÔNG nằm trong danh sách đã bị đặt
            return [.. allRooms.Where(r => !bookedRoomIds.Contains(r.RoomID) && r.RoomStatus == Models.Enums.EntityStatus.Active)];
        }

        // ... (GetAllRooms, GetAllRoomTypes, Search giữ nguyên) ...
        public IEnumerable<Room> GetAllRooms()
        {
            var rooms = _repo.GetAll();
            var roomTypes = _roomTypeRepo.GetAll();
            foreach (var r in rooms)
            {
                r.RoomType = roomTypes.FirstOrDefault(rt => rt.RoomTypeID == r.RoomTypeID);
            }
            return rooms;
        }

        public IEnumerable<RoomType> GetAllRoomTypes()
        {
            return _roomTypeRepo.GetAll();
        }

        public IEnumerable<Room> Search(string keyword)
        {
            var k = keyword.ToLower();
            return GetAllRooms()
                .Where(r => (r.RoomNumber?.ToLower().Contains(k, StringComparison.CurrentCultureIgnoreCase) ?? false)
                         || (r.RoomDescription?.ToLower().Contains(k, StringComparison.CurrentCultureIgnoreCase) ?? false));
        }


        // --- CẬP NHẬT PHƯƠNG THỨC NÀY ---
        public RepositoryResult<Room> Create(Room r)
        {
            var error = Validate(r, isNew: true);
            if (error != null) return RepositoryResult<Room>.Fail(error);
            return _repo.Add(r);
        }

        // --- CẬP NHẬT PHƯƠNG THỨC NÀY ---
        public RepositoryResult<Room> Update(Room r)
        {
            var error = Validate(r, isNew: false);
            if (error != null) return RepositoryResult<Room>.Fail(error);
            return _repo.Update(r);
        }

        public RepositoryResult<bool> Delete(int id)
        {
            return _repo.Delete(id);
        }

        // --- CẬP NHẬT HOÀN TOÀN PHƯƠNG THỨC NÀY ---
        private string? Validate(Room r, bool isNew)
        {
            if (!Validators.NotEmpty(r.RoomNumber) || !Validators.MaxLength(r.RoomNumber, 50))
                return "RoomNumber is required (<=50).";

            var existingByRoomNumber = _repo.GetByRoomNumber(r.RoomNumber);
            if (isNew)
            {
                if (existingByRoomNumber != null)
                    return "Room Number already exists.";
            }
            else
            {
                if (existingByRoomNumber != null && existingByRoomNumber.RoomID != r.RoomID)
                    return "Room Number is already taken by another room.";
            }

            if (!Validators.MaxLength(r.RoomDescription, 220))
                return "RoomDescription max 220.";
            if (r.RoomMaxCapacity <= 0)
                return "RoomMaxCapacity must be > 0.";
            if (r.RoomPricePerDate < 0)
                return "RoomPricePerDate must be >= 0.";
            if (r.RoomTypeID <= 0)
                return "RoomType is required.";
            return null;
        }
    }
}