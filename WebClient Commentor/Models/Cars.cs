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
    public String currentDate { get; set; }
    public String currentHour { get; set; }

    public Cars(int carId, int carCount, String currentdate)
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