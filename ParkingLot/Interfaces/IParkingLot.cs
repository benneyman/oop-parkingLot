using ParkingLot.Enums;
using ParkingLot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLot.Interfaces
{
    public interface IParkingLot
    {
        int FreeSpots { get; }
        bool ParkVehicle(Vehicle vehicle, ParkingSpot parkingSpot);
        bool UnParkvehicle(Vehicle vehicle);
        ParkingSpot GetOptimalParkingSpot(Vehicle vehicle);
        ParkingSpotStatus GetParkingSpotStatus(ParkingSpot spot);
    }
}
