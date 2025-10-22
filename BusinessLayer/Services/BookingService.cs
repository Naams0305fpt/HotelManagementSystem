using DataAccessLayer.Common;
using DataAccessLayer.Repositories.Interfaces;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IRoomRepository _roomRepo;

        public BookingService(IBookingRepository repo, IRoomRepository roomRepo)
        {
            _repo = repo; _roomRepo = roomRepo;
        }

        // --- PHƯƠNG THỨC MỚI ---
        public IEnumerable<Booking> GetByCustomerId(int customerId)
        {
            return _repo.GetByCustomerId(customerId).OrderByDescending(b => b.StartDate);
        }

        // --- PHƯƠNG THỨC MỚI ---
        public IEnumerable<Booking> GetBookingsForReport(DateTime start, DateTime end)
        {
            return _repo.GetByDateRange(start, end).OrderByDescending(b => b.TotalPrice);
        }

        public RepositoryResult<Booking> Create(Booking b)
        {
            var e = Validate(b);
            if (e != null) return RepositoryResult<Booking>.Fail(e);

            var room = _roomRepo.GetById(b.RoomID);
            if (room == null) return RepositoryResult<Booking>.Fail("Room not found");
            var nights = (b.EndDate.Date - b.StartDate.Date).Days;
            if (nights <= 0) return RepositoryResult<Booking>.Fail("EndDate must be after StartDate");

            b.TotalPrice = room.RoomPricePerDate * nights;
            return _repo.Add(b);
        }

        public RepositoryResult<Booking> Update(Booking b)
        {
            var e = Validate(b);
            if (e != null) return RepositoryResult<Booking>.Fail(e);

            var room = _roomRepo.GetById(b.RoomID);
            if (room == null) return RepositoryResult<Booking>.Fail("Room not found");
            var nights = (b.EndDate.Date - b.StartDate.Date).Days;
            if (nights <= 0) return RepositoryResult<Booking>.Fail("EndDate must be after StartDate");
            b.TotalPrice = room.RoomPricePerDate * nights;

            return _repo.Update(b);
        }

        private string? Validate(Booking b)
        {
            if (b.CustomerID <= 0) return "CustomerID invalid";
            if (b.RoomID <= 0) return "RoomID invalid";
            if (b.StartDate.Date > b.EndDate.Date) return "StartDate must be before EndDate";
            return null;
        }
    }
}