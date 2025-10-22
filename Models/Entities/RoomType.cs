using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class RoomType
    {
        public int RoomTypeID { get; set; }
        public string RoomTypeName { get; set; } = string.Empty; // max 50
        public string TypeDescription { get; set; } = string.Empty; // max 250
        public string TypenNote { get; set; } = string.Empty; // max 250 (theo đề bài)
    }
}
