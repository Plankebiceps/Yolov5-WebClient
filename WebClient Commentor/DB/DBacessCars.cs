using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClient_Commentor.Models;
using System.Data.SqlClient;

namespace WebClient_Commentor.DB
{
    public class DBacessCars
    {
        readonly string connectionString;

        public DBacessCars() {
            connectionString = "data Source=.; database=CommentorDB; integrated security=true";
}

        public List<Cars> getAllCars()
        {
            List<Cars> foundCars = null;
            Cars readCars = null;

            string queryString = "select Cars.carid, Cars.caramount, Dates.CurrentDate from Cars Inner Join Dates ON Cars.DateId=Dates.DateId";

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
    
    
    public Cars GetCarsFromReader(SqlDataReader carsReader)
        {
            Cars foundCars;
            int tempCarId;
            int tempCarCount;
            String tempcurrentdate;

            tempCarId = carsReader.GetInt32(carsReader.GetOrdinal("CarId"));
            tempCarCount = carsReader.GetInt32(carsReader.GetOrdinal("CarAmount"));
            tempcurrentdate = carsReader.GetString(carsReader.GetOrdinal("CurrentDate"));

            foundCars = new Cars(tempCarId, tempCarCount, tempcurrentdate);
            return foundCars;
        }
    
    
    
    }
}