using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;  
using OnRoadVehicleBreakdownAssistance.Models;

namespace OnRoadVehicleBreakdownAssistance.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Membership model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new DBEntities())
                {                    

                    var roleObj = new WebRoleProvider();

                    string[] roles = roleObj.GetRolesForUser(model.Username);

                    bool isValidUser = context.User.Any(x => x.Username == model.Username && x.Password == model.Password);
                    if (isValidUser == true && roles.Contains("Admin"))
                    {
                        FormsAuthentication.SetAuthCookie(model.Username, false);
                        return RedirectToAction("Index", "Customers");
                    }
                    else if(isValidUser == true && roles.Contains("Customer"))
                    {
                        FormsAuthentication.SetAuthCookie(model.Username, false);
                        return RedirectToAction("Create", "Services");
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(model.Username, false);
                        return RedirectToAction("Create", "Services");
                    }
                    ModelState.AddModelError("Username", "Invalid Username/Password");
                    return View();
                }
            }
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User model, String Checkbox)
        {        
            if (ModelState.IsValid)
            {
                using (var context = new DBEntities())
                {
                    bool isExist = context.User.Any(x => x.Username == model.Username);

                    if (isExist)
                    {
                        ModelState.AddModelError("Username", "Username already exist");
                        return View();
                    }
                    if(Checkbox is null)
                    {
                        ModelState.AddModelError("Checkbox", "Please accept the Terms");
                        return View();
                    }
                    context.User.Add(model);
                    context.SaveChanges();
                    return RedirectToAction("Login");
                }              
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Reset()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reset(User model, String Confirmpassword)
        {
            if (ModelState.IsValid)
            {
                using (var context = new DBEntities())
                {
                    bool isExistUser = context.User.Any(x => x.Username == model.Username);

                    if (!isExistUser)
                    {
                        ModelState.AddModelError("Username", "Incorrect Username");
                        return View();
                    }

                    if (Confirmpassword is null)
                    {
                        ModelState.AddModelError("Confirmpassword", "The Password field is required");
                        return View();
                    }

                    if (!(model.Password == Confirmpassword))
                    {
                        ModelState.AddModelError("Confirmpassword", "Password did not match");
                        return View();
                    }

                    var employee = context.User.FirstOrDefault(x => x.Username == model.Username);
                    if (employee != null)
                    {
                        employee.Password = model.Password;
                    }
                    context.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
    }
}