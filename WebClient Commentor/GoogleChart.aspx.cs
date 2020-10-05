using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebClient_Commentor.Models;

namespace WebClient_Commentor
{
    public partial class GoogleChart : System.Web.UI.Page
    {
        string queryString = "SELECT carAmount FROM Cars";
        readonly string connectionString;
        public GoogleChart()
        {
            connectionString = "data Source=.; database=CommentorDB; integrated security=true";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetChartData();
        }
        
        public Cars GetCarFromReader(SqlDataReader carReader)
        {
            Cars foundCar;
            int tempCarId;
            int tempCarAmount;
            string tempCurrentDate;

            tempCarId = carReader.GetInt32(carReader.GetOrdinal("carId"));
            tempCarAmount = carReader.GetInt32(carReader.GetOrdinal("carAmount"));
            tempCurrentDate = carReader.GetString(carReader.GetOrdinal("currentDate"));
            foundCar = new Cars(tempCarId, tempCarAmount, tempCurrentDate);
            return foundCar;
        }

        public List<Cars> AmountOfCars()
        {
            List<Cars> foundCars = null;
            Cars readCar = null;
            List<Cars> data = new List<Cars>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand readCommand = new SqlCommand(queryString, con))
                {
                    con.Open();
                    SqlDataReader carReader = readCommand.ExecuteReader();

                    if (carReader.HasRows)
                    {
                        foundCars = new List<Cars>();
                        while (carReader.Read())
                        {
                            readCar = GetCarFromReader(carReader);
                            foundCars.Add(readCar);
                        }
                    }
                }
                return foundCars;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object[] GetChartData()
        {
            List<Cars> data = new List<Cars>();

            var chartData = new object[data.Count + 1];
            chartData[0] = new object[]
            {
                "Car Amount"
            };
            int j = 0;
            foreach (var i in data)
            {
                j++;
                chartData[j] = new object[] {i.CarCount};
            }
            return chartData;
        }
    }
}