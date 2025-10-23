using BusinessLayer.Validation;
using DataAccessLayer.Common;
using DataAccessLayer.Repositories.Interfaces;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Services
{
    public class RoomService(IRoomRepository repo,
                       IRoomTypeRepository roomTypeRepo,
                       IBookingDetailRepository bookingDetailRepo)
    {
        private readonly IRoomRepository _repo = repo;
        private readonly IRoomTypeRepository _roomTypeRepo = roomTypeRepo;
        // --- THAY THẾ REPO ---
        // private readonly IBookingRepository _bookingRepo; // <<< XÓA DÒNG NÀY
        private readonly IBookingDetailRepository _bookingDetailRepo = bookingDetailRepo; // <<< THÊM DÒNG NÀY

        // --- CẬP NHẬT LOGIC GetAvailableRooms ---
        public IEnumerable<Room> GetAvailableRooms(DateTime start, DateTime end)
        {
            if (start.Date >= end.Date)
            {
                return [];
            }

            // Lấy tất cả các chi tiết booking có xung đột thời gian
            var conflictingBookingDetails = _bookingDetailRepo.GetConflictingBookings(start, end);

            // Lấy ID của các phòng đã bị đặt
            var bookedRoomIds = conflictingBookingDetails.Select(b => b.RoomID).Distinct();

            var allRooms = GetAllRooms();

            // Trả về các phòng KHÔNG nằm trong danh sách đã bị đặt và ĐANG HOẠT ĐỘNG
            return [.. allRooms.Where(r => !bookedRoomIds.Contains(r.RoomID) && r.RoomStatus == Models.Enums.EntityStatus.Active)];
        }
        // ------------------------------------

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

        // --- CẬP NHẬT LOGIC SEARCH (Tối ưu hóa) ---
        public IEnumerable<Room> Search(string keyword)
        {
            return GetAllRooms()
                .Where(r => (r.RoomNumber?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
                         || (r.RoomDescription?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false));
        }
        // ---------------------------------------

        public RepositoryResult<Room> Create(Room r)
        {
            var error = Validate(r, isNew: true);
            if (error != null) return RepositoryResult<Room>.Fail(error);
            return _repo.Add(r);
        }

        public RepositoryResult<Room> Update(Room r)
        {
            var error = Validate(r, isNew: false);
            if (error != null) return RepositoryResult<Room>.Fail(error);
            return _repo.Update(r);
        }

        public RepositoryResult<bool> Delete(int id)
        {
            // --- THÊM LOGIC NGHIỆP VỤ ---
            var details = _bookingDetailRepo.GetAll().Where(d => d.RoomID == id);
            if (details.Any())
            {
                return RepositoryResult<bool>.Fail("Cannot delete room. This room has existing booking details.");
            }
            // ---------------------------
            return _repo.Delete(id);
        }

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