using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClient_Commentor.Models
{
    public class Cars
    {
    public int CarId { get; set; }
    public int CarCount { get; set; }
    public string CurrentDate { get; set; }
    public string CurrentHour { get; set; }
    public int DateId { get; set; }

    public Cars(int carId, int carCount, string currentdate, string currenthour)
        {
            CarId = carId;
            CarCount = carCount;
            CurrentDate = currentdate;
            CurrentHour = currenthour;
        }

        public Cars(int carCount, string currentdate, string currentHour, int dateId)
        {
            CarCount = carCount;
            CurrentDate = currentdate;
            CurrentHour = currentHour;
            DateId = dateId;
        }

        public Cars(int carCount, string currentdate, string currentHour)
        {
            CarCount = carCount;
            CurrentDate = currentdate;
            CurrentHour = currentHour;
        }

        public Cars()
        {

        }

    }
}