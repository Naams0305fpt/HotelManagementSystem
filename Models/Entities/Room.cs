using Models.Enums;

namespace Models.Entities
{
    public class Room
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; } = string.Empty; // max 50
        public string RoomDescription { get; set; } = string.Empty; // max 220
        public int RoomMaxCapacity { get; set; }
        public EntityStatus RoomStatus { get; set; } = EntityStatus.Active;
        public decimal RoomPricePerDate { get; set; }


        // FK
        public int RoomTypeID { get; set; }
        public RoomType? RoomType { get; set; }

    }
}
