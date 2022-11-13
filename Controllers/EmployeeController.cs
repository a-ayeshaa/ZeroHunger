using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger_Mid_.DB;

namespace ZeroHunger_Mid_.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        [HttpGet]
        public ActionResult CheckRequest(int id)
        {
            var db = new ZeroHunger_dbEntities1();
            var name = (from n in db.Employees
                        where n.id == id
                        select n).SingleOrDefault();
            ViewBag.id=id;
            ViewBag.name = name.name;
            var req=(from r in db.Requests
                     where r.employee_id==id
                     select r).ToList();

            return View(req);
        }
        [HttpPost]
        public ActionResult CheckRequest(int id,string sort)
        {
            var db = new ZeroHunger_dbEntities1();
            var name = (from n in db.Employees
                        where n.id == id
                        select n).SingleOrDefault();
            ViewBag.name=name.name;
            ViewBag.id=id;
            if (sort.Equals("All"))
            {
                var rq = (from r in db.Requests
                          where r.employee_id == id
                          select r).ToList();

                return View(rq);
            }
            var req = (from r in db.Requests
                       where r.status == sort && r.employee_id==id
                       select r).ToList();
            if (req == null)
            {
                TempData["msg"] = "No data found";
                return RedirectToAction("CheckRequest");
            }
            
            return View(req);
        }

        public ActionResult AcceptReq(int id)
        {
            var db = new ZeroHunger_dbEntities1();
            var req = (from r in db.Requests
                       where r.id == id
                       select r).SingleOrDefault();
            req.status = "Accepted";
            db.SaveChanges();   
            return RedirectToAction("ShowEmployee", "NGO");
        }
        public ActionResult DeclineReq(int id)
        {
            var db = new ZeroHunger_dbEntities1();
            var req = (from r in db.Requests
                       where r.id == id
                       select r).SingleOrDefault();
            req.status = "Rejected";
            db.SaveChanges();
            return RedirectToAction("ShowEmployee", "NGO");
        }
        public ActionResult CompleteReq(int id)
        {
            var db = new ZeroHunger_dbEntities1();
            var req = (from r in db.Requests
                       where r.id == id
                       select r).SingleOrDefault();
            req.status = "Completed";
            db.SaveChanges();
            return RedirectToAction("ShowEmployee", "NGO");
        }
    }
}