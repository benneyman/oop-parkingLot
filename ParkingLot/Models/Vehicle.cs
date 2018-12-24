using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingLot.Enums;

namespace ParkingLot.Models
{
    public class Vehicle
    {
        public string VehicleNumber { get; set; }
        public VehicleTypes vehicleType { get; set; }
    }
}
