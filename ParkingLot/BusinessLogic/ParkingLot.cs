using ParkingLot.Interfaces;
using ParkingLot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using ParkingLot.Enums;
using System.Collections.Concurrent;
using System.Threading;

namespace ParkingLot.BusinessLogic
{
    public class ParkingLotCore : IParkingLot
    {
        private ImmutableSortedSet<ParkingSpot> freeParkingSpots;
        private ConcurrentDictionary<string, ParkingSpot> parkedVehicles;
        private readonly IEnumerable<List<List<ParkingSpot>>> parkingLotLayout;
        private readonly IParkingSpaceMapper parkingSpaceMapper;
        private int _freeSpots = 0;
        public int FreeSpots => _freeSpots;
        private int _totalSpots = 0;
        public int TotalSpots => _totalSpots;

        public ParkingLotCore(IEnumerable<List<List<ParkingSpot>>> parkingLotLayout, IParkingSpaceMapper parkingSpaceMapper)
        {
            var comparer = Comparer<ParkingSpot>.Create((x, y) =>
                x.Floor == y.Floor ?
                    x.Row == y.Row ?
                        x.StartPosition.CompareTo(y.StartPosition)
                        : x.Row.CompareTo(y.Row)
                    : x.Floor.CompareTo(y.Floor)
            );
            freeParkingSpots = ImmutableSortedSet.Create<ParkingSpot>(comparer);
            parkedVehicles = new ConcurrentDictionary<string, ParkingSpot>();
            this.parkingLotLayout = parkingLotLayout;
            this.parkingSpaceMapper = parkingSpaceMapper;
            InitializeParkingLot();
        }

        private void InitializeParkingLot()
        {
            foreach (var floor in parkingLotLayout)
            {
                foreach (var row in floor)
                {
                    foreach (var spot in row)
                    {
                        freeParkingSpots = freeParkingSpots.Add(spot);
                        Interlocked.Add(ref _totalSpots, spot.SpotCount);
                        Interlocked.Add(ref _freeSpots, spot.SpotCount);
                    }
                }
            }
        }

        public ParkingSpot GetOptimalParkingSpot(Vechicle vechicle)
        {
            ParkingSpaceRequirment requiredSpace = parkingSpaceMapper.GetSmallestParkingSpaceRequired(vechicle);
            var vacantSpot = freeParkingSpots.FirstOrDefault(m => m.ParkingSpotTypes >= requiredSpace.ParkingSpot
            && m.SpotCount >= requiredSpace.ParkingSpotsCount
            );
            if (vacantSpot != null)
            {
                vacantSpot.SpotCount = Math.Min(vacantSpot.SpotCount, requiredSpace.ParkingSpotsCount);
            }
            return vacantSpot;
        }

        public bool ParkVehicle(Vechicle vehicle, ParkingSpot parkingSpot)
        {
            if (parkedVehicles.ContainsKey(vehicle.VehicleNumber))
            {
                throw new InvalidOperationException($"Vehicle with number {vehicle.VehicleNumber} is already parked");
            }
            ParkingSpot vacantSpot = freeParkingSpots.FirstOrDefault(spot => spot.Floor == parkingSpot.Floor
            && spot.Row == parkingSpot.Row
            && spot.ParkingSpotTypes == parkingSpot.ParkingSpotTypes
            && spot.StartPosition <= parkingSpot.StartPosition
            && spot.SpotCount >= parkingSpot.SpotCount
             );
            if (vacantSpot == null)
                throw new KeyNotFoundException("The spot could not be found");

            freeParkingSpots = freeParkingSpots.Remove(vacantSpot);
            parkedVehicles.TryAdd(vehicle.VehicleNumber, parkingSpot);
            if (parkingSpot.StartPosition > vacantSpot.StartPosition)
            {
                var newSpot = vacantSpot;
                newSpot.SpotCount = parkingSpot.StartPosition - vacantSpot.StartPosition;
                freeParkingSpots = freeParkingSpots.Add(newSpot);
            }
            if (vacantSpot.SpotCount > parkingSpot.SpotCount)
            {
                var newSpot = vacantSpot;
                newSpot.StartPosition = parkingSpot.StartPosition + parkingSpot.SpotCount;
                freeParkingSpots = freeParkingSpots.Add(newSpot);
            }
            Interlocked.Add(ref _freeSpots, parkingSpot.SpotCount * -1);
            return true;
        }

        public bool UnParkVechicle(Vechicle vechicle)
        {
            parkedVehicles.TryGetValue(vechicle.VehicleNumber, out ParkingSpot currentSpot);
            if (currentSpot == null)
                throw new ArgumentException($"Vechicle {vechicle.VehicleNumber} is not parked");

            freeParkingSpots = freeParkingSpots.Remove(currentSpot);
            var leftSpot = freeParkingSpots.FirstOrDefault(spot => spot.Floor == currentSpot.Floor
             && spot.Row == currentSpot.Row
             && spot.ParkingSpotTypes == currentSpot.ParkingSpotTypes
             && spot.StartPosition + spot.SpotCount == currentSpot.StartPosition
            );

            if (leftSpot != null)
            {
                currentSpot.StartPosition = leftSpot.StartPosition;
                currentSpot.SpotCount = currentSpot.SpotCount + leftSpot.SpotCount;
            }
            var rightSpot = freeParkingSpots.FirstOrDefault(spot => spot.Floor == currentSpot.Floor
             && spot.Row == currentSpot.Row
             && spot.ParkingSpotTypes == currentSpot.ParkingSpotTypes
             && spot.StartPosition == currentSpot.StartPosition + currentSpot.SpotCount
            );

            if (rightSpot != null)
            {
                currentSpot.SpotCount = currentSpot.SpotCount + rightSpot.SpotCount;
            }
            freeParkingSpots = freeParkingSpots.Add(currentSpot);
            return true;
        }

    }
}
