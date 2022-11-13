using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger_Mid_.DB;

namespace ZeroHunger_Mid_.Controllers
{
    public class NGOController : Controller
    {
        // GET: NGO
        [HttpGet]
        public ActionResult Index()
        {
            var db = new ZeroHunger_dbEntities1();
            var req = db.Requests.ToList();
            foreach (var r in req)
            {
                if(r.expiredAt < DateTime.Now)
                {
                    r.status = "Expired";
                    db.SaveChanges();
                }
            }
            return View(req);
        }
        [HttpPost]
        public ActionResult Index(string sort)
        {
            var db = new ZeroHunger_dbEntities1();
            if(sort.Equals("All"))
            {
                return View(db.Requests.ToList());
            }
            var req = (from r in db.Requests
                       where r.status == sort
                       select r).ToList();
            if (req == null)
            {
                TempData["msg"] = "No data found";
                return RedirectToAction("Index");
            }
            return View(req);
        }
        

        [HttpGet]
        public ActionResult AssignEmployee(int id)
        {
            var db = new ZeroHunger_dbEntities1();
            var req = (from r in db.Requests
                       where r.id == id
                       select r).SingleOrDefault();
            ViewBag.employees = db.Employees.ToList();
            return View(req);
        }
        [HttpPost]
        public ActionResult AssignEmployee(Request request)
        {
            var db = new ZeroHunger_dbEntities1();
            var req = (from r in db.Requests
                       where r.id == request.id
                       select r).SingleOrDefault();
            req.employee_id = request.employee_id;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddEmployee(Employee e)
        {
            var db = new ZeroHunger_dbEntities1();
            db.Employees.Add(e);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShowEmployee()
        {
            var db = new ZeroHunger_dbEntities1();
            var emp = db.Employees.ToList();
            return View(emp);
        }

        public ActionResult Details(int id)
        {
            var db = new ZeroHunger_dbEntities1();
            var req = (from r in db.Items
                       where r.request_id == id
                       select r).ToList();
            return View(req);
        }
    }
}