using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClient_Commentor.Models;
using WebClient_Commentor.DB;
using Antlr.Runtime.Tree;

namespace WebClient_Commentor.Controllers
{
    public class HomeController : Controller
    {
        DBAccessCars dbCars = new DBAccessCars();
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    
        public ActionResult Index()
        {

            DBAccessCars dbcars = new DBAccessCars();
            List<Cars> carsToDisplay = dbcars.getAllCars();
            Dashboard();
            
            return View(carsToDisplay);
        }

        public JsonResult SortBetweenHours(string start, string end)
        {
            List<Cars> cars = dbCars.getSortedCars(start, end);
            IEnumerable<int> selectCount = SelectCarCount(cars);
            IEnumerable<string> selectHour = SelectCurrentHours(cars);
            return Json(new { countSelect = selectCount, hourSelect = selectHour }, JsonRequestBehavior.AllowGet);
        }

        /*
        public JsonResult SortBetweenDays(start, end)
        {
            List<Cars> cars = dbCars.getSortedCars(start, end);
            IEnumerable<int> selectCount = SelectCarCount(cars);
            IEnumerable<string> selectDay = 
        }
        */


        /*
        public list<cars> sortbetweenhours(string start, string end)
        {
            list<cars> cars = dbcars.getsortedcars(start, end);
            return cars;
        }
        */

        public IEnumerable<int> SelectCarCount(List<Cars> cars)
        {
            IEnumerable<int> CarCount = cars.Select(x => x.CarCount);
            return CarCount;
        }

        public IEnumerable<string> SelectCurrentHours(List<Cars> cars)
        {
            List<string> CurrentHour = cars.Select(x => x.currentHour).ToList();
            List<string> CurrentDate = cars.Select(x => x.currentDate).ToList();
            IEnumerable<string> HourAndDate = new List<string>();

            int index = 0;
            while(index != CurrentHour.Count())
            {
                string hour = CurrentHour[index];
                string date = CurrentDate[index];
                string finalResult = "Kl: " + hour + ":00" + " - " + "Dato: " + date;
                HourAndDate = HourAndDate.Concat(new[] { finalResult });
                Console.WriteLine("");
                index++;
            }
            return HourAndDate;
        }

        public IEnumerable<string> SelectCurrentDays(List<Cars> cars)
        {
            IEnumerable<string> CurrentDate = cars.Select(x => x.currentDate);
            return CurrentDate;
        }

        public ActionResult Dashboard()
        {
            DBAccessCars dbcars = new DBAccessCars();

            List<Cars> cars = dbcars.getAllCars();
            //List<Car> cars = dbcars.getSortedCars("10", "13");
            List<String> Hours = new List<String>();

            /*
            var CarCount = cars.OrderBy(x => x.CarCount);
            var CurrentHour = cars.OrderBy(x => x.currentHour);
            */

            //måske til senere.
            //var CarCount = cars.Select(x => x.CarCount).OrderBy(x => x).ToArray()

            var CarCount = SelectCarCount(cars);
            var CurrentHour = SelectCurrentHours(cars);
            var CurrentDate = cars[cars.Count -1].currentDate;
            int Amount = 0;

            foreach (var item in CurrentHour)
            {
                Hours.Add(item);
                Amount++;
            }

            ViewBag.CARCOUNT = CarCount;
            ViewBag.CURRENTHOUR = Hours;
            ViewBag.CURRENTDATE = CurrentDate;
            ViewBag.AMOUNT = Amount;

            return View();

        }
    }
}