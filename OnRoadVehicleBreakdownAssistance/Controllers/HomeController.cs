using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnRoadVehicleBreakdownAssistance.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Account
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult News()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

    }
}