using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLot.Models
{
    public class ParkingLotStatus
    {
        public int TotalParkingSpots { get; set; }
        public int OccupiedSpots { get; set; }
        public int FreeSpots { get; set; }
    }
}
