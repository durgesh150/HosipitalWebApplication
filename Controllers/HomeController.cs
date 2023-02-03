
using HosipitalWebApplication.Migration;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using PagedList.Mvc;
using System.Web.Mvc;
using System.Web.Security;

namespace HosipitalWebApplication.Controllers
{
    //This controller is used for signout,signin & signin;
    public class HomeController : Controller
    {
        HosplitalDbContext dbobj = new HosplitalDbContext();
        // GET: Home
        [HandleError(View = "Error")]
        public ActionResult Logout()                             //Logout  from page
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
        public ActionResult Login()                             //Login GET method
        {

            return View();
        }
        [HandleError(View = "Error")]
        [HttpPost]
        public ActionResult Login(Appuser model)                //Login post method
        {
            try
            {

                var access = dbobj.Appusers.FirstOrDefault(u => u.Username == model.Username);
                if (access != null)
                {
                    bool isAdmin = access.Isadmin;
                    Session["Isadmin"] = isAdmin;
                        Session["Username"] = model.Username;
                        Session["AdminId"] = access.Id;
                        byte[] inputtedPasswordBytes = System.Text.Encoding.UTF8.GetBytes(model.password);
                        byte[] decryptedPasswordBytes = ProtectedData.Unprotect(Convert.FromBase64String(access.password), null, DataProtectionScope.CurrentUser);
                        string decryptedPassword = System.Text.Encoding.UTF8.GetString(decryptedPasswordBytes);
                        if (model.password == decryptedPassword)
                        {
                            Session["username"] = model.Username;
                            return RedirectToAction("Display", "Dashboard", model);
                        }
                    
                    ModelState.AddModelError("Password", "Invalid username or password");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Username", "Username is required");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("Password", "Invalid username or password");
                return View();
            }

        }
        [HandleError(View = "Error")]
        public ActionResult Signup()     //Signup GET method
        {
            if (Session["Username"] == null)
            {
                Session["Invalid"] = "Please Login First";
                return RedirectToAction("login", "home");
            }
            return View();
        }
        [HandleError(View = "Error")]
        [HttpPost]
        public ActionResult Signup(Patientdata model)       //Signup Post method
        {
            if (Session["Username"] == null)
            {
                Session["Invalid"] = "Please Login First";
                return RedirectToAction("login", "home");
            }
            var maxId = dbobj.Patientdatas.Max(x => x.Id);
            var newId = maxId + 1;
            model.Id = newId;
            model.CreatedOnDateTime = DateTime.Now;
            model.CreatedByUserId = (int)Session["AdminId"];
            //model.CreatedByUserId = 1;
            model.IsDeleted = false;
            model.UpdatedByUserId = 0;
            if (ModelState.IsValid)
            {

                dbobj.Patientdatas.Add(model);
                dbobj.SaveChanges();
                return RedirectToAction("Display", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("LastVisitDateTime", "Username already exist");
                return View();
            }
        }

    }
}