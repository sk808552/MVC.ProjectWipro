using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using OnRoadVehicleBreakdownAssistance;

namespace OnRoadVehicleBreakdownAssistance.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Services
        public ActionResult Index()
        {
            string name = User.Identity.Name.ToString();
            return View(db.Service.Where(x => x.cust_srv_name == name).ToList());
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            ViewBag.ServiceTypes = ("Bike Towing","Car Towing","Fuel Provide","Wheel Change");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cust_srv_id,cust_srv_name,cust_srv_email,cust_srv_mob_no,cust_srv_type,cust_srv_date,cust_srv_status")] Service service)
        {
            if (ModelState.IsValid)
            {
                service.cust_srv_status = "Pending";
                db.Service.Add(service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(service);
        }


        [HttpGet]
        public ActionResult UserProfile()
        {
            string name = User.Identity.Name.ToString();
            var customer = db.Customer.Where(x => x.cust_full_name == name).ToList();
            if (customer.Count == 0)
            {
                return RedirectToAction("ProfileCreate");
            }
            return RedirectToAction("ProfileDetails");
        }

        public ActionResult ProfileCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProfileCreate([Bind(Include = "cust_id,cust_full_name,cust_email,cust_age,cust_mob_no")] Customer customer)
        {
            var obj = new WebRoleProvider();
            var roles = obj.GetRolesForUser(User.Identity.Name.ToString());

            if (ModelState.IsValid)
            {
                db.Customer.Add(customer);
                db.SaveChanges();
                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Customers");
                }
                return RedirectToAction("ProfileDetails", "Services");
            }
            return View(customer);
        }

        public ActionResult ProfileUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public ActionResult ProfileUpdate([Bind(Include = "cust_id,cust_full_name,cust_email,cust_age,cust_mob_no")] Customer customer)
        {
            var obj1 = new WebRoleProvider();
            var roles1 = obj1.GetRolesForUser(User.Identity.Name.ToString());

            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                if (roles1.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Customers");
                }
                return RedirectToAction("ProfileDetails", "Services");
            }
            return View(customer);
        }

        public ActionResult ProfileDetails()
        {
            string name = User.Identity.Name.ToString();
            var customer = db.Customer.Where(x => x.cust_full_name == name).ToList();
            return View(customer[0]);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
