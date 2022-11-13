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
    }
}