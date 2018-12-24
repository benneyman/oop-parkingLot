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
                        new ParkingSpot() { Floor = 2, Row = 2, ParkingSpotTypes = ParkingSpotTypes.Compact, StartPosition = 1, SpotCount = 10}
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
            var vehicle = new Vechicle() { VehicleNumber = "One",  VechicleType = VehicleTypes.MotorCycle };
            var actual = parkingLot.GetOptimalParkingSpot(vehicle);
            var expected = new ParkingSpot() { Floor = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, Row = 1, SpotCount = 1, StartPosition = 1 };
            actual.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void GetOptimalSpotShouldReturnFirstPositionForCar()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            var vehicle = new Vechicle() { VehicleNumber = "One", VechicleType = VehicleTypes.Car };
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
            var vehicle = new Vechicle() { VehicleNumber = "One", VechicleType = VehicleTypes.Car };
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
            var vehicle = new Vechicle() { VehicleNumber = "One", VechicleType = VehicleTypes.Car };
            var actual = parkingLot.GetOptimalParkingSpot(vehicle);
            var expected = new ParkingSpot() { Floor = 2, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Large, StartPosition = 1, SpotCount = 1 };
            actual.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void GetOptimalSpotShouldReturnNull()
        {
            var mockSpaceMapper = new Mock<IParkingSpaceMapper>();
            mockSpaceMapper.Setup(m => m.GetSmallestParkingSpaceRequired(It.IsAny<Vechicle>()))
                .Returns(new ParkingSpaceRequirment() { ParkingSpot = ParkingSpotTypes.Large, ParkingSpotsCount = 15 });
            var parkingLot = new ParkingLotCore(layout, mockSpaceMapper.Object);
            var parkingSpot = parkingLot.GetOptimalParkingSpot(new Vechicle() { VehicleNumber = "1", VechicleType = VehicleTypes.Bus });
            parkingSpot.Should().BeNull();
        }
        [TestMethod]
        public void ParkVehicleShouldReturnTrue()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 2 };
            var vechicle = new Vechicle() { VehicleNumber = "est", VechicleType = VehicleTypes.MotorCycle };
            var actual = parkingLot.ParkVehicle(vechicle, spot);
            actual.Should().BeTrue();

        }
        [TestMethod]
        public void ParkShouldThrowException()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 2 };
            var vechicle = new Vechicle() { VehicleNumber = "est", VechicleType = VehicleTypes.MotorCycle };
            var actual = parkingLot.ParkVehicle(vechicle, spot);
            if(actual == true)
            {
                
                Action act = () => parkingLot.ParkVehicle(vechicle, spot);
                act.Should().Throw<InvalidOperationException>();
            }
        }
        [TestMethod]
        public async void ParkMultipleTimesShouldThrowExceptionOnce()
        {
            var parkingLot = new ParkingLotCore(layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Motorcycle, StartPosition = 1, SpotCount = 2 };
            var vechicle = new Vechicle() { VehicleNumber = "est", VechicleType = VehicleTypes.MotorCycle };
            Action act = async () =>  await Task.Run(() => parkingLot.ParkVehicle(vechicle, spot));
            var result2 = await Task.Run(() => parkingLot.ParkVehicle(vechicle, spot));
        }
    }
}
