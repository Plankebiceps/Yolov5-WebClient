using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClient_Commentor.Models;
using System.Data.SqlClient;
using System.Web.ModelBinding;

namespace WebClient_Commentor.DB
{
    public class DBAccessCars
    {
        readonly string connectionString;

        public DBAccessCars()
        {
            connectionString = "data Source=.; database=CommentorDB; integrated security=true";
        }

        public List<Cars> getAllCars()
        {
            List<Cars> foundCars = null;
            Cars readCars = null;
            Cars emptyCar = new Cars(0, 0, "Start", "");

            string queryString = "SELECT Cars.CarId, Cars.CarAmount, Dates.CurrentDate, Hours.HourId from Cars INNER JOIN Dates ON Cars.DateId=Dates.DateId INNER JOIN Hours ON Cars.HourId=Hours.HourId";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                con.Open();

                SqlDataReader carsReader = readCommand.ExecuteReader();

                if (carsReader.HasRows)
                {
                    foundCars = new List<Cars>();
                    foundCars.Add(emptyCar);
                    while (carsReader.Read())
                    {
                        readCars = GetCarsFromReader(carsReader, true);
                        foundCars.Add(readCars);
                    }
                }
            }
            return foundCars;
        }

        public List<Cars> getSortedCarsHour(string StartHour, string EndHour)
        {
            List<Cars> foundCars = null;
            Cars readCars = null;
            Cars emptyCar = new Cars(0, 0, "Start", "");
            string queryString = "SELECT Cars.CarId, Dates.CurrentDate, Hours.HourId, Cars.CarAmount FROM Cars INNER JOIN Dates ON Cars.DateId=Dates.DateId INNER JOIN Hours On Cars.HourId=Hours.HourId WHERE Hours.HourId BETWEEN @StartHour AND @EndHour";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                SqlParameter AddStartHour = new SqlParameter("@StartHour", StartHour);
                readCommand.Parameters.Add(AddStartHour);
                SqlParameter AddEndHour = new SqlParameter("@EndHour", EndHour);
                readCommand.Parameters.Add(AddEndHour);
                con.Open();

                SqlDataReader carsReader = readCommand.ExecuteReader();

                if (carsReader.HasRows)
                {
                    foundCars = new List<Cars>();
                    foundCars.Add(emptyCar);
                    while (carsReader.Read())
                    {
                        readCars = GetCarsFromReader(carsReader, true);
                        foundCars.Add(readCars);
                    }
                }
            }
            return foundCars;
        }

        public List<Cars> getSortedCarsDayAndHours(string StartHour, string EndHour, string StartDate, string EndDate)
        {
            List<Cars> foundCars = null;
            Cars readCars = null;
            Cars emptyCar = new Cars(0, 0, "Start", "");
            string queryString = "SELECT Cars.CarId, Dates.CurrentDate, Hours.HourId, Cars.CarAmount FROM Cars INNER JOIN Dates ON Cars.DateId=Dates.DateId INNER JOIN Hours On Cars.HourId=Hours.HourId WHERE Hours.HourId BETWEEN @StartHour AND @EndHour AND Dates.CurrentDate BETWEEN @StartDate AND @EndDate";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                SqlParameter AddStartHour = new SqlParameter("@StartHour", StartHour);
                readCommand.Parameters.Add(AddStartHour);
                SqlParameter AddEndHour = new SqlParameter("@EndHour", EndHour);
                readCommand.Parameters.Add(AddEndHour);
                SqlParameter AddStartDate = new SqlParameter("@StartDate", StartDate);
                readCommand.Parameters.Add(AddStartDate);
                SqlParameter AddEndDate = new SqlParameter("@EndDate", EndDate);
                readCommand.Parameters.Add(AddEndDate);
                con.Open();

                SqlDataReader carsReader = readCommand.ExecuteReader();

                if (carsReader.HasRows)
                {
                    foundCars = new List<Cars>();
                    foundCars.Add(emptyCar);
                    while (carsReader.Read())
                    {
                        readCars = GetCarsFromReader(carsReader, true);
                        foundCars.Add(readCars);
                    }
                }
            }
            return foundCars;
        }

        public List<Cars> getSortedCarsDay(string StartDate, string EndDate)
        {
            List<Cars> foundCars = null;
            Cars readCars = null;
            Cars emptyCar = new Cars(0, 0, "Start", "");
            string queryString = "SELECT Cars.DateId AS DateId, Dates.CurrentDate AS CurrentDate, SUM(CarAmount) AS CarCount FROM Cars INNER JOIN Dates ON Cars.DateId = Dates.DateId WHERE Dates.CurrentDate BETWEEN @StartDate AND @EndDate GROUP BY Cars.DateId, Dates.CurrentDate";
            using(SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                SqlParameter AddStartDate = new SqlParameter("@StartDate", StartDate);
                readCommand.Parameters.Add(AddStartDate);
                SqlParameter AddEndDate = new SqlParameter("@EndDate", EndDate);
                readCommand.Parameters.Add(AddEndDate);
                con.Open();

                SqlDataReader carsReader = readCommand.ExecuteReader();

                if (carsReader.HasRows)
                {
                    foundCars = new List<Cars>();
                    foundCars.Add(emptyCar);
                    while (carsReader.Read())
                    {
                        readCars = GetCarsFromReader(carsReader, false);
                        foundCars.Add(readCars);
                    }
                }
            }
            return foundCars;
        }

        public void DeleteFromDB(int toDelete)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmdDeleteCars = con.CreateCommand())
                {
                    cmdDeleteCars.CommandText = "DELETE FROM Cars WHERE Cars.CarId=@CarId";
                    cmdDeleteCars.Parameters.AddWithValue("CarId", toDelete);
                    cmdDeleteCars.ExecuteNonQuery();
                }
            }
        }

        public List<Cars> GetAllCarsByLatestDate()
        {
            List<Cars> foundCars = null;
            Cars readCars = null;
            string queryString = "SELECT Cars.CarId, Dates.CurrentDate, Hours.HourId, Cars.CarAmount FROM Cars INNER JOIN Dates ON Cars.DateId=Dates.DateId INNER JOIN Hours On Cars.HourId=Hours.HourId WHERE Dates.DateId=(SELECT max(Dates.DateId) FROM Dates)";
            Cars emptyCar = new Cars(0, 0, "Start", "");

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                con.Open();

                SqlDataReader carsReader = readCommand.ExecuteReader();

                if (carsReader.HasRows)
                {
                    foundCars = new List<Cars>();
                    foundCars.Add(emptyCar);
                    while (carsReader.Read())
                    {
                        readCars = GetCarsFromReader(carsReader, true);
                        foundCars.Add(readCars);
                    }
                }
            }
            return foundCars;
        }

        public List<Cars> Get7LatestCars()
        {
            List<Cars> foundCars = null;
            Cars readCars = null;
            string queryString = "WITH SortedSeven AS (SELECT TOP 7 CarId, Dates.CurrentDate, Hours.HourId, CarAmount FROM Cars INNER JOIN Dates ON Cars.DateId=Dates.DateId INNER JOIN Hours On Cars.HourId=Hours.HourId ORDER BY CarId DESC) SELECT * FROM SortedSeven ORDER BY CarId";
            Cars emptyCar = new Cars(0, 0, "Start", "");

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                con.Open();

                SqlDataReader carsReader = readCommand.ExecuteReader();

                if (carsReader.HasRows)
                {
                    foundCars = new List<Cars>();
                    foundCars.Add(emptyCar);
                    while (carsReader.Read())
                    {
                        readCars = GetCarsFromReader(carsReader, true);
                        foundCars.Add(readCars);
                    }
                }
            }
            return foundCars;
        }

        public Cars GetCarsFromReader(SqlDataReader carsReader, bool check)
        {
            Cars foundCars;
            int tempCarId;
            int tempCarCount;
            string tempcurrentdate;
            string tempcurrenthour;
            int tempDateId;
            
            if (check)
            {
                
                tempCarId = carsReader.GetInt32(carsReader.GetOrdinal("CarId"));
                tempCarCount = carsReader.GetInt32(carsReader.GetOrdinal("CarAmount"));
                tempcurrentdate = carsReader.GetString(carsReader.GetOrdinal("CurrentDate"));
                tempcurrenthour = carsReader.GetString(carsReader.GetOrdinal("HourId"));

                foundCars = new Cars(tempCarId, tempCarCount, tempcurrentdate, tempcurrenthour);
            }
            else
            {
                tempCarCount = carsReader.GetInt32(carsReader.GetOrdinal("CarCount"));
                tempcurrentdate = carsReader.GetString(carsReader.GetOrdinal("CurrentDate"));
                tempcurrenthour = "";
                tempDateId = carsReader.GetInt32(carsReader.GetOrdinal("DateId"));

                foundCars = new Cars(tempCarCount, tempcurrentdate, tempcurrenthour, tempDateId);
            }
            return foundCars;
        }
    }
}