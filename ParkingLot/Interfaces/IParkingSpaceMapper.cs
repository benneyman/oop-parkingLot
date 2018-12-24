using ParkingLot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLot.Interfaces
{
    public interface IParkingSpaceMapper
    {
        ParkingSpaceRequirment GetSmallestParkingSpaceRequired(Vehicle vehicle);
    }
}
