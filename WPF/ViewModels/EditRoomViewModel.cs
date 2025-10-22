using Models.Entities;
using PhamHuynhSumWPF.ViewModels.Base;
using Models.Enums;
using System.Collections.Generic;

namespace PhamHuynhSumWPF.ViewModels
{
    public class EditRoomViewModel : ViewModelBase
    {
        public Room Entity { get; set; }

        // Danh sách các loại phòng để hiển thị trên ComboBox
        public IEnumerable<RoomType> RoomTypes { get; }

        // Constructor cho "Add"
        public EditRoomViewModel(IEnumerable<RoomType> roomTypes)
        {
            RoomTypes = roomTypes;
            Entity = new Room
            {
                RoomStatus = EntityStatus.Active
            };
        }

        // Constructor cho "Edit"
        public EditRoomViewModel(Room room, IEnumerable<RoomType> roomTypes)
        {
            RoomTypes = roomTypes;
            // Tạo bản sao (clone)
            Entity = new Room
            {
                RoomID = room.RoomID,
                RoomNumber = room.RoomNumber,
                RoomDescription = room.RoomDescription,
                RoomMaxCapacity = room.RoomMaxCapacity,
                RoomPricePerDate = room.RoomPricePerDate,
                RoomStatus = room.RoomStatus,
                RoomTypeID = room.RoomTypeID
            };
        }
    }
}