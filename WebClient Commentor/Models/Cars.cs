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
    public DateTime currentDate { get; set; }
    public DateTime currentHour { get; set; }

    public Cars(int carId, int carCount, DateTime currentdate)
        {
            CarId = carId;
            CarCount = carCount;
            currentDate = currentdate;

        }

        public Cars()
        {

        }

    }
}