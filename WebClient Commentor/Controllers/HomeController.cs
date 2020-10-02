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
            
            return View(carsToDisplay);
        }
    
    }
}