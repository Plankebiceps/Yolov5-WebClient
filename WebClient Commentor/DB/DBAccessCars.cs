using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClient_Commentor.Models;
using System.Data.SqlClient;
using System.Web.ModelBinding;
using System.Transactions;
using System.Windows.Forms;

namespace WebClient_Commentor.DB
{
    public class DBAccessCars
    {
        readonly string connectionString;

        public DBAccessCars()
        {
            connectionString = "data Source=.; database=CommentorDB; integrated security=true";
        }

        //Denne metode får alle køretøjer.
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

        //Gammel metode som ikke bruges, men sorteres efter valgt start time og slut time.
        public List<Cars> getSortedCarsHour(string StartHour, string EndHour)
        {
            List<Cars> foundCars = null;
            Cars readCars = null;
            Cars emptyCar = new Cars(0, 0, "Start", "");
            string queryString = "SELECT Cars.CarId, Dates.CurrentDate, Hours.HourId, Cars.CarAmount FROM Cars INNER JOIN Dates ON Cars.DateId=Dates.DateId INNER JOIN Hours On Cars.HourId=Hours.HourId WHERE Hours.HourId BETWEEN @StartHour AND @EndHour";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                try
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                return foundCars;
            }
        }

        //Denne metode vælger alle køretøjer, hvor der er angivet en start time, slut time, start dato, og slut dato, og sorter derefter.
        public List<Cars> getSortedCarsDayAndHours(string StartHour, string EndHour, string StartDate, string EndDate)
        {
            List<Cars> foundCars = null;
            Cars readCars = null;
            Cars emptyCar = new Cars(0, 0, "Start", "");
            string queryString = "SELECT Cars.CarId, Dates.CurrentDate, Hours.HourId, Cars.CarAmount FROM Cars INNER JOIN Dates ON Cars.DateId=Dates.DateId INNER JOIN Hours On Cars.HourId=Hours.HourId WHERE Hours.HourId BETWEEN @StartHour AND @EndHour AND Dates.CurrentDate BETWEEN @StartDate AND @EndDate";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                try
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            return foundCars;
        }
        //Denne metode får køretøjerne ud fra en valgt start dato til en slut dato, og sorter dem derefter.
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

        public int FindLatestCarId()
        {
            int carsIdToDelete = 0;
            string queryString = "SELECT MAX(CarId) FROM Cars";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                con.Open();

                carsIdToDelete = (int)readCommand.ExecuteScalar();
                
            }
            return carsIdToDelete;
        }

        //Denne metode benyttes kun til test, da den tester om noget kan slettes.
        public int InsertToDBToDelete()
        {
            int carsIdMade = 0;
            string queryString = "BEGIN TRANSACTION INSERT INTO Dates(CurrentDate, WeekNumber) VALUES('02 Nov 2020', 44); INSERT INTO Cars(DateId, HourId, CarAmount) VALUES(22, '21', 500); COMMIT";
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand readCommand = new SqlCommand(queryString, con))
                {
                    con.Open();
                    carsIdMade = readCommand.ExecuteNonQuery();
                }
                return carsIdMade;
            }
        }

        //Dog er denne metode benyttet til at slettes normalt, og ikke ligesom den ovenfor.
        public void DeleteFromDB(int toDelete)
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            try
            {
                using (TransactionScope scope = new TransactionScope())
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
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }
        }

        //Denne metode får alle biler, som sådan set nu er alle køretøjer som den henter fra seneste dato.
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

        //Denne metode henter de 7 seneste timer med biler.
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

        //En loop igennem uger.
        public List<Cars> LoopThroughWeeks()
        {
            List<Cars> foundCars = new List<Cars>();
            Cars emptyCar = new Cars(0, 0, "Start", "");
            foundCars.Add(emptyCar);
            List<string> Weeks = GetAllWeekNumbers();
            int index = 0;

            while (index < Weeks.Count())
                {
                int Index = Int32.Parse(Weeks[index]);
                    Cars car = SortByWeeks(Index);
                    foundCars.Add(car);
                    index++;
                }

            return foundCars;

        }

        //Sorter efter uger.
        public Cars SortByWeeks(int index)
        {
            Cars readCars = null;
            string queryString = "SELECT SUM(CarAmount) AS CarCount, WeekNumber as CurrentDate FROM Cars, Dates WHERE Cars.DateId = Dates.DateId and WeekNumber = @index Group By WeekNumber";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                SqlParameter AddWeek = new SqlParameter("@index", index);
                readCommand.Parameters.Add(AddWeek);
                con.Open();

                SqlDataReader carsReader = readCommand.ExecuteReader();

                if (carsReader.HasRows)
                {
                    while (carsReader.Read())
                    {
                        readCars = GetCarsWithIntFromReader(carsReader);
                    }
                }
            }
            return readCars;
        }

        //Får alle ugens numre.
        public List<string> GetAllWeekNumbers()
        {
            List<string> WeekNumbers = new List<string>();
            int number = 0;
            string queryString = "SELECT DISTINCT WeekNumber FROM Dates";
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand readCommand = new SqlCommand(queryString, con))
            {
                con.Open();

                SqlDataReader carsReader = readCommand.ExecuteReader();

                if (carsReader.HasRows)
                {
                    while (carsReader.Read())
                    {
                        number = GetWeekNumberFromTable(carsReader);
                        string myDate = number.ToString();
                        WeekNumbers.Add(myDate);
                    }
                }
            }
            return WeekNumbers;
        }

        public int GetWeekNumberFromTable(SqlDataReader dataReader)
        {
            int weekNumber;

            weekNumber = dataReader.GetInt32(dataReader.GetOrdinal("WeekNumber"));
 
            return weekNumber;
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

        public Cars GetCarsWithIntFromReader(SqlDataReader carsReader)
        {
            Cars foundCars;
            int tempCarCount;
            string tempcurrentdate;

            tempCarCount = carsReader.GetInt32(carsReader.GetOrdinal("CarCount"));
            int test = carsReader.GetInt32(carsReader.GetOrdinal("CurrentDate"));
            string myDate = test.ToString();
            tempcurrentdate = myDate;

            foundCars = new Cars(tempCarCount, tempcurrentdate, "");
            return foundCars;
        }
    }
}