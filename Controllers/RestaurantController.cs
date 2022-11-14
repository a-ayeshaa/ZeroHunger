using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger_Mid_.DB;

namespace ZeroHunger_Mid_.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index()
        {
            var db = new ZeroHunger_dbEntities1();
            var res = db.Restaurants.ToList();
            return View(res);
        }
        [HttpGet]
        public ActionResult AddRestaurant()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRestaurant(Restaurant r)
        {
            var db = new ZeroHunger_dbEntities1();
            db.Restaurants.Add(r);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateReq(int id)
        {
            var db = new ZeroHunger_dbEntities1();
            var res = (from r in db.Restaurants
                       where r.id == id
                       select r).SingleOrDefault();
            return View(res);
        }
        [HttpPost]
        public ActionResult CreateReq(Request r, string[] items, int[] quantity)
        {
            var db = new ZeroHunger_dbEntities1();

            if (items[0].Equals(""))
            {
                TempData["msg"] = "Cannot create request with no items";
                return RedirectToAction("Index");
            }
            var req= new Request()
            {
                restaurant_id = r.restaurant_id,
                status = "Pending",
                requestAt = DateTime.Now,
                expiredAt = r.expiredAt,
            };
            db.Requests.Add(req);
            db.SaveChanges();
            for(int i=0;i<items.Length;i++)
            {
                if (!items[i].Equals(""))
                {
                    db.Items.Add(new Item()
                    {
                        request_id = req.id,
                        name = items[i],
                        quantity = quantity[i]
                    });
                    db.SaveChanges();
                }
                
            }
            return RedirectToAction("Index","NGO");
        }
        [HttpGet]
        public ActionResult CheckRequests(int id)
        {
            var db = new ZeroHunger_dbEntities1();
            var req = (from r in db.Requests
                       where r.restaurant_id == id
                       select r).ToList();
            var name = (from r in db.Restaurants
                        where r.id == id
                        select r).SingleOrDefault();
            ViewBag.name = name.name;
            ViewBag.id = id;
            return View(req);
        }

        [HttpPost]
        public ActionResult CheckRequests(int id, string sort)
        {
            var db = new ZeroHunger_dbEntities1();
            var name = (from n in db.Restaurants
                        where n.id == id
                        select n).SingleOrDefault();
            ViewBag.name = name.name;
            ViewBag.id = id;
            if (sort.Equals("All"))
            {
                var rq = (from r in db.Requests
                          where r.restaurant_id == id
                          select r).ToList();

                return View(rq);
            }
            var req = (from r in db.Requests
                       where r.status == sort && r.restaurant_id == id
                       select r).ToList();
            if (req == null)
            {
                TempData["msg"] = "No data found";
                return RedirectToAction("CheckRequests");
            }

            return View(req);
        }

        public ActionResult DeleteReq(int id)
        {
            var db = new ZeroHunger_dbEntities1();
            var i = (from req in db.Items
                     where req.request_id == id
                     select req).ToList();
            foreach (var item in i)
            {
                db.Items.Remove(item);
                db.SaveChanges();

            }
            var r = (from req in db.Requests
                     where req.id == id
                     select req).SingleOrDefault();
            db.Requests.Remove(r);
            db.SaveChanges();

            
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}