using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ParkingLot.Interfaces;
using ParkingLot.BusinessLogic;
using System.Collections.Generic;
using ParkingLot.Models;
using ParkingLot.Enums;
using FluentAssertions;
using System.Threading.Tasks;

namespace ParkingLot.Tests
{
    [TestClass]
    public class ParkingLotTests
    {
        List<List<List<ParkingSpot>>> layout = new List<List<List<ParkingSpot>>>()
            {
                new List<List<ParkingSpot>> ()
                {
                    new List<ParkingSpot> ()
                    {
                        new ParkingSpot() { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 2, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 3, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 10}
                    },
                    new List<ParkingSpot> ()
                    {
                        new ParkingSpot() { Floor = 2, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Compact, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 2, Row = 2, ParkingSpotTypes = ParkingSpotTypes.Compact, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 2, Row = 3, ParkingSpotTypes = ParkingSpotTypes.Compact, StartPosition = 1, SpotCount = 10}
                    },
                }
            };
        [TestMethod]
        public void TotalFreeSpotsAfterInitializingTest()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            int freeSpots = parkingLot.FreeSpots;
            freeSpots.Should().Be(60);
        }
        [TestMethod]
        public void FreeSpotsAndTotalSpotsMustBeSameAfterInitializingTest()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            int freeSpots = parkingLot.FreeSpots;
            int totalSpots = parkingLot.TotalSpots;
            freeSpots.Should().Be(totalSpots);
        }
        [TestMethod]
        public void GetOptimalSpotShouldReturnFirstPosition()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            var vehicle = new Vehicle() { VehicleNumber = "One", vehicleType = VehicleTypes.MotorCycle };
            var actual = parkingLot.GetOptimalParkingSpot(vehicle);
            var expected = new ParkingSpot() { Floor = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, Row = 1, SpotCount = 1, StartPosition = 1 };
            actual.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void GetOptimalSpotShouldReturnFirstPositionForCar()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            var vehicle = new Vehicle() { VehicleNumber = "One", vehicleType = VehicleTypes.Car };
            var actual = parkingLot.GetOptimalParkingSpot(vehicle);
            var expected = new ParkingSpot() { Floor = 2, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Compact, StartPosition = 1, SpotCount = 1 };
            actual.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void GetOptimalSpotShouldReturnHigherFirstPositionForCar()
        {
            List<List<List<ParkingSpot>>> newLayout = new List<List<List<ParkingSpot>>>()
            {
                new List<List<ParkingSpot>> ()
                {
                    new List<ParkingSpot> ()
                    {
                        new ParkingSpot() { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 2, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 3, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 10}
                    },
                    new List<ParkingSpot> ()
                    {
                        new ParkingSpot() { Floor = 2, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Large, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 2, Row = 2, ParkingSpotTypes = ParkingSpotTypes.Large, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 2, Row = 2, ParkingSpotTypes = ParkingSpotTypes.Large, StartPosition = 1, SpotCount = 10}
                    },
                }
            };
            var parkingLot = new ParkingLotCore(newLayout, new ParkingSpaceMapper());
            var vehicle = new Vehicle() { VehicleNumber = "One", vehicleType = VehicleTypes.Car };
            var actual = parkingLot.GetOptimalParkingSpot(vehicle);
            var expected = new ParkingSpot() { Floor = 2, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Large, StartPosition = 1, SpotCount = 1 };
            actual.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void GetOptimalSpotShouldReturnHigherFirstPositionForMotorCycle()
        {
            List<List<List<ParkingSpot>>> newLayout = new List<List<List<ParkingSpot>>>()
            {
                new List<List<ParkingSpot>> ()
                {
                    new List<ParkingSpot> ()
                    {
                        new ParkingSpot() { Floor = 2, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Large, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 2, Row = 2, ParkingSpotTypes = ParkingSpotTypes.Large, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 2, Row = 2, ParkingSpotTypes = ParkingSpotTypes.Large, StartPosition = 1, SpotCount = 10}
                    },
                }
            };
            var parkingLot = new ParkingLotCore(newLayout, new ParkingSpaceMapper());
            var vehicle = new Vehicle() { VehicleNumber = "One", vehicleType = VehicleTypes.Car };
            var actual = parkingLot.GetOptimalParkingSpot(vehicle);
            var expected = new ParkingSpot() { Floor = 2, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Large, StartPosition = 1, SpotCount = 1 };
            actual.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void GetOptimalSpotShouldReturnNull()
        {
            var mockSpaceMapper = new Mock<IParkingSpaceMapper>();
            mockSpaceMapper.Setup(m => m.GetSmallestParkingSpaceRequired(It.IsAny<Vehicle>()))
                .Returns(new ParkingSpaceRequirment() { ParkingSpot = ParkingSpotTypes.Large, ParkingSpotsCount = 15 });
            var parkingLot = new ParkingLotCore(layout, mockSpaceMapper.Object);
            var parkingSpot = parkingLot.GetOptimalParkingSpot(new Vehicle() { VehicleNumber = "1", vehicleType = VehicleTypes.Bus });
            parkingSpot.Should().BeNull();
        }
        [TestMethod]
        public void ParkVehicleShouldReturnTrue()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 2 };
            var vehicle = new Vehicle() { VehicleNumber = "est", vehicleType = VehicleTypes.MotorCycle };
            var actual = parkingLot.ParkVehicle(vehicle, spot);
            actual.Should().BeTrue();
        }
        [TestMethod]
        public void ParkVehicleShouldUpdateTheParkedVehicleStatus()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 3, SpotCount = 2 };
            ParkingSpot newVacant = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 5, SpotCount = 6 };
            ParkingSpot newVacant2 = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 2 };
            var vehicle = new Vehicle() { VehicleNumber = "est", vehicleType = VehicleTypes.MotorCycle };
            parkingLot.ParkVehicle(vehicle, spot).Should().BeTrue();
            parkingLot.GetParkingSpotStatus(newVacant).Should().Be(ParkingSpotStatus.Vacant);
            parkingLot.GetParkingSpotStatus(newVacant2).Should().Be(ParkingSpotStatus.Vacant);
            parkingLot.GetParkingSpotStatus(spot).Should().Be(ParkingSpotStatus.Occupied);
        }
        [TestMethod]
        public void UnParkVehicleShouldUpdateTheParkedVehicleStatus()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 3, SpotCount = 2 };
            ParkingSpot newVacant = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 5, SpotCount = 6 };
            ParkingSpot newVacant2 = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 2 };
            ParkingSpot newVacant3 = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 10 };
            var vehicle = new Vehicle() { VehicleNumber = "est", vehicleType = VehicleTypes.MotorCycle };
            parkingLot.ParkVehicle(vehicle, spot).Should().BeTrue();
            parkingLot.UnParkvehicle(vehicle);
            parkingLot.GetParkingSpotStatus(newVacant).Should().Be(ParkingSpotStatus.Vacant);
            parkingLot.GetParkingSpotStatus(newVacant2).Should().Be(ParkingSpotStatus.Vacant);
            parkingLot.GetParkingSpotStatus(spot).Should().Be(ParkingSpotStatus.Vacant);
            parkingLot.GetParkingSpotStatus(newVacant3).Should().Be(ParkingSpotStatus.Vacant);
        }
        [TestMethod]
        public void ParkShouldThrowException()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 2 };
            var vehicle = new Vehicle() { VehicleNumber = "est", vehicleType = VehicleTypes.MotorCycle };
            var actual = parkingLot.ParkVehicle(vehicle, spot);
            if (actual == true)
            {

                Action act = () => parkingLot.ParkVehicle(vehicle, spot);
                act.Should().Throw<InvalidOperationException>();
            }
        }

        [TestMethod]
        public void GetParkingStatusShouldReturnVacant()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 2 };
            var result = parkingLot.GetParkingSpotStatus(spot);
            result.Should().Be(ParkingSpotStatus.Vacant);
        }


    }
}
