using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data.Queries
{
    /// <summary>
    /// Класс для передачи дополнительных параметров при обновлении комнаты
    /// </summary>
    public class UpdateRoomQuery
    {
        public string NewName { get; }
        public int NewArea { get; set; }
        public int NewVoltage { get; set; }

        public UpdateRoomQuery(string newName = null,
            int newArea = 0,
            int newVoltage = 0)
        {
            NewName = newName;
            NewArea = newArea;
            NewVoltage = newVoltage;
        }
    }
}

