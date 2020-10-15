using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClient_Commentor.Models;
using System.Data.SqlClient;

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

            string queryString = "SELECT Cars.CarId, Cars.CarAmount, Dates.CurrentDate, Hours.HourId from Cars INNER JOIN Dates ON Cars.DateId=Dates.DateId INNER JOIN Hours ON Cars.HourId=Hours.HourId";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                con.Open();

                SqlDataReader carsReader = readCommand.ExecuteReader();

                if (carsReader.HasRows)
                {
                    foundCars = new List<Cars>();
                    while (carsReader.Read())
                    {
                        readCars = GetCarsFromReader(carsReader);
                        foundCars.Add(readCars);
                    }
                }
            }
            return foundCars;
        }

        public List<Cars> getSortedCars(string StartHour, string EndHour)
        {
            List<Cars> foundCars = null;
            Cars readCars = null;
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
                    while (carsReader.Read())
                    {
                        readCars = GetCarsFromReader(carsReader);
                        foundCars.Add(readCars);
                    }
                }
            }
            return foundCars;
        }


        public Cars GetCarsFromReader(SqlDataReader carsReader)
        {
            Cars foundCars;
            int tempCarId;
            int tempCarCount;
            String tempcurrentdate;
            String tempcurrenthour;

            tempCarId = carsReader.GetInt32(carsReader.GetOrdinal("CarId"));
            tempCarCount = carsReader.GetInt32(carsReader.GetOrdinal("CarAmount"));
            tempcurrentdate = carsReader.GetString(carsReader.GetOrdinal("CurrentDate"));
            tempcurrenthour = carsReader.GetString(carsReader.GetOrdinal("HourId"));

            foundCars = new Cars(tempCarId, tempCarCount, tempcurrentdate, tempcurrenthour);
            return foundCars;
        }
    
    }
}