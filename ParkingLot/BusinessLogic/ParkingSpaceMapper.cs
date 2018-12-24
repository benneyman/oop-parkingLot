using ParkingLot.Enums;
using ParkingLot.Interfaces;
using ParkingLot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLot.BusinessLogic
{
    public class ParkingSpaceMapper : IParkingSpaceMapper
    {
        public ParkingSpaceRequirment GetSmallestParkingSpaceRequired(Vehicle vehicle)
        {
            switch (vehicle.vehicleType)
            {
                case VehicleTypes.MotorCycle:
                    return new ParkingSpaceRequirment() { ParkingSpot = ParkingSpotTypes.Motorcycle, ParkingSpotsCount = 1 };
                case VehicleTypes.Car:
                    return new ParkingSpaceRequirment() { ParkingSpot = ParkingSpotTypes.Compact, ParkingSpotsCount = 1 };
                case VehicleTypes.Bus:
                    return new ParkingSpaceRequirment() { ParkingSpot = ParkingSpotTypes.Large, ParkingSpotsCount = 5 };
                default:
                    throw new ArgumentException($"vehicleType {vehicle.vehicleType} is invalid.");
            }
        }
    }
}
