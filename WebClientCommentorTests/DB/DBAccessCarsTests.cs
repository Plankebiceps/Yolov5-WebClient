using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebClient_Commentor.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient_Commentor.Models;
using System.Data.SqlClient;

namespace WebClient_Commentor.DB.Tests
{
    [TestClass()]
    public class DBAccessCarsTests
    {
        string connectionString;
        Cars carsToFind;
        DBAccessCars findCars;

        [TestInitialize]
        public void InitializeBeforeEachMethod()
        {
            carsToFind = new Cars();
            findCars = new DBAccessCars();
        }
        [TestMethod()]
        public void DBAccessCarsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void getAllCarsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void getSortedCarsHourTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void getSortedCarsDayAndHoursTest()
        {
            //Arrange Cars
            carsToFind.CarId = 1;
            carsToFind.DateId = 1;
            carsToFind.CurrentHour = "07";
            carsToFind.CarCount = 78;

            //Act
            IEnumerable<Cars> foundTestCars = findCars.getSortedCarsDayAndHours("07", "07", "12 Oct 2020", "12 Oct 2020");

            //Assert
            Assert.IsNotNull(foundTestCars);
        }

        [TestMethod()]
        public void getSortedCarsDayTest()
        {
            //Arrange Cars
            carsToFind.DateId = 3;
            carsToFind.CarCount = 1503;

            //Act
            IEnumerable<Cars> foundTestCars = findCars.getSortedCarsDay("12 Oct 2020","12 Oct 2020");

            //Assert
            Assert.IsNotNull(foundTestCars);
        }

        [TestMethod()]
        public void DeleteFromDBTest()
        {
            //Arrange
            findCars.InsertToDBToDelete();
            int testDeletion = findCars.FindLatestCarId();

            //Act
            findCars.DeleteFromDB(testDeletion);
                
            //Assert
            Assert.AreNotEqual(testDeletion, findCars.FindLatestCarId());
        }

        [TestMethod()]
        public void GetAllCarsByLatestDateTest()
        {
            //Arrange
            carsToFind.CarId = 276;
            carsToFind.DateId = 22;
            carsToFind.CurrentHour = "22";
            carsToFind.CarCount = 500;

            //Act
            IEnumerable<Cars> foundTestCars = findCars.GetAllCarsByLatestDate();

            //Assert
            Assert.IsNotNull(foundTestCars);
        }

        [TestMethod()]
        public void Get7LatestCarsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetCarsFromReaderTest()
        {
            Assert.Fail();
        }
    }
}