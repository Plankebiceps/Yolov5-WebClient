using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClient_Commentor.Models;
using WebClient_Commentor.DB;

namespace WebClient_Commentor.Controllers
{
    public class HomeController : Controller
    {
        

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

            DBacessCars dbcars = new DBacessCars();
            List<Cars> carsToDisplay = dbcars.getAllCars();
            Dashboard();
            
            return View(carsToDisplay);
        }

        public ActionResult GetList(string Start, string End)
        {
            DBacessCars dbcars = new DBacessCars();
            List<Cars> cars = dbcars.getSortedCars("10", "13");
            return PartialView(@"~/Views/Home/Index.cshtml", cars);
        }

        public ActionResult Dashboard()
        {
            DBacessCars dbcars = new DBacessCars();

            List<Cars> cars = dbcars.getAllCars();
            //List<Car> cars = dbcars.getSortedCars("10", "13");
            List<String> Hours = new List<String>();
            var CarCount = cars.Select(x => x.CarCount).Distinct();
            var CurrentHour = cars.Select(x => x.currentHour).Distinct();
            var CurrentDate = cars[0].currentDate;
            int Amount = 0;

            foreach (var item in CurrentHour)
            {
                Hours.Add(item + ":00");
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