using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Contracts.Models.Rooms
{
    public class EditRoomRequest
    {
        public string? NewName { get; set; }
        public int NewArea { get; set; }
        public int NewVoltage { get; set; }
    }
}
