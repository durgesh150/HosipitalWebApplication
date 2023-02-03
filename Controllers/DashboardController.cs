using HosipitalWebApplication.Migration;
using System;
using System.Collections.Generic;
using PagedList;
using PagedList.Mvc;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HosipitalWebApplication.Controllers
{
    //This controller is doing the Operations like Display, update ,Delete ,pagination ,searching , sorting ;
    public class DashboardController : Controller
    {
        HosplitalDbContext dbobj = new HosplitalDbContext();
    [HandleError(View ="Error")]
        public ActionResult Display(string searchTerm, string sortBy, int page = 1)    // Display method to display data, Pagination ,sorting ,searching 
        {
            
            
            if (Session["Username"] == null)
            {
                Session["Invalid"] = "Please Login First";

                return RedirectToAction("login", "home");
            }
            ViewBag.sortName = sortBy=="Name"? "Name desc" : "Name";
            ViewBag.sortCity = sortBy =="City" ? "City desc" : "City";
            ViewBag.sortHealth = sortBy =="Health" ? "Health desc" : "Health";
            if (page < 1)
            {
                page = 1;
            }
            int totalitems = dbobj.Patientdatas.Where(x => x.IsDeleted == false).ToList().Count();
            int pageSize = 100;
            
            ViewData["currentPage"] = page;
            ViewData["pageSize"] = pageSize;
           
            var data = dbobj.Patientdatas.AsQueryable();
            ViewData["totalitems"] = data
     .Where(p => p.IsDeleted == false && (p.FirstName.StartsWith(searchTerm) || p.HealthIssues.StartsWith(searchTerm) || searchTerm == null || p.LastName.StartsWith(searchTerm) || p.Address.Contains(searchTerm) || p.City.StartsWith(searchTerm))).ToList().Count();
            totalitems =(int) ViewData["totalitems"];
              int datasinpage = (int)Math.Ceiling((double)totalitems / pageSize);
          
            ViewData["datasinpage"] = datasinpage;
            data = data
       .Where(p => p.IsDeleted == false && (p.FirstName.StartsWith(searchTerm) || p.HealthIssues.StartsWith(searchTerm) || searchTerm == null || p.LastName.StartsWith(searchTerm) || p.Address.Contains(searchTerm) || p.City.StartsWith(searchTerm)))
       .OrderBy(x => x.Id)
       .Skip((page - 1) * pageSize)
       .Take(pageSize);
          
            switch (sortBy)
            {
                case "Name desc":
                    data = data.OrderByDescending(x => x.FirstName);
                    break;
                case "Name":
                    data = data.OrderBy(x => x.FirstName);
                    break;
                case "City desc":
                    data = data.OrderByDescending(x => x.City);
                    break;
                case "City":
                    data = data.OrderBy(x => x.City);
                    break;
                case "Health desc":
                    data = data.OrderByDescending(x => x.HealthIssues);
                    break;
                case "Health":
                    data = data.OrderBy(x => x.HealthIssues);
                    break;
                default:
                    data = data.OrderBy(x=> x.Id);
                    break;
            }
            return View(data.ToList());

        }
        [HandleError(View = "Error")]
        public ActionResult Delete(int Id) // Delete User get method
        {
            if (Session["Username"] == null)
            {
                Session["Invalid"] = "Please Login First";
                return RedirectToAction("login", "home");
            }
            Patientdata patientData = dbobj.Patientdatas.Find(Id);
            patientData.IsDeleted = true;
            dbobj.SaveChanges();
            return RedirectToAction("Display");
        }
        [HandleError(View = "Error")]
        public ActionResult Update(int? id)                         // Update GET method to update patient
        {
            if (Session["Username"] == null)
            {
                Session["Invalid"] = "Please Login First";
                return RedirectToAction("login", "home");
            }
            var maxId = dbobj.Patientdatas.Max(x => x.Id);
            if ((id > 0 && id <= maxId))
            {
                var obj = dbobj.Patientdatas.Find(id);
                return View(obj);
            }
            return RedirectToAction("display", "dashboard");
        }
        [HandleError(View = "Error")]
        [HttpPost]
        public ActionResult Update(Patientdata model)                   // Update Post method to update patient
        {

            var maxId = dbobj.Patientdatas.Max(x => x.Id);
            if (Session["Username"] == null && model.Id < 0 && model.Id > maxId)
            {
                Session["Invalid"] = "Please Login First";
                return RedirectToAction("login", "home");
            }
            var obj = dbobj.Patientdatas.Find(model.Id);

            obj.FirstName = model.FirstName;
            obj.LastName = model.LastName;
            obj.Address = model.Address;
            obj.City = model.City;
            obj.Gender = model.Gender;
            obj.UpdatedByUserId = (int)Session["AdminId"];
            obj.HealthIssues = model.HealthIssues;
            if (ModelState.IsValid)
            { 
                dbobj.SaveChanges();
                return RedirectToAction("Display", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("City", "Please fill all the field first");
                return View();
            }
        }
    }
}