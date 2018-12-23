using ParkingLot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLot.Models
{
    public class ParkingSpaceRequirment
    {
        public ParkingSpotTypes ParkingSpot { get; set; }
        public int ParkingSpotsCount { get; set; }
    }
}
