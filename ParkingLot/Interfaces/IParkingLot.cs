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
        bool ParkVehicle(Vechicle vehicle, ParkingSpot parkingSpot);
        bool UnParkVechicle(Vechicle vechicle);
        ParkingSpot GetOptimalParkingSpot(Vechicle vechicle);
    }
}
