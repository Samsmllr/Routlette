using Microsoft.AspNet.Identity;
using RouteScheduler.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RouteScheduler.Controllers
{
    public class BusinessOwnersController : Controller
    {
        private APIKeys aPIKeys = new APIKeys();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BusinessOwner
        public ActionResult Index()
        {
            string ApiIs = ("https://www.google.com/maps/embed/v1/view?zoom=10&center=43.0389,-87.9065&key=" + aPIKeys.ApiKey);
            ViewData["ApiKey"] = ApiIs;
            //TODO: create portal for website
            return View();
        }

        public ActionResult TodaysRoute()
        {
            var currentPerson = User.Identity.GetUserId();
            var Longitude = db.businessOwners.Where(c => c.ApplicationId == currentPerson).FirstOrDefault().Longitude;
            var Latitude = db.businessOwners.Where(c => c.ApplicationId == currentPerson).FirstOrDefault().Latitude;
            string DisplayIs = ($"https://www.google.com/maps/embed/v1/view?zoom=16&center={Latitude},{Longitude}&key=" + aPIKeys.ApiKey);
            ViewData["DisplayIs"] = DisplayIs;
            return View();
        }

        // GET: BusinessOwner/Details/5
        public ActionResult Details()
        {
            var currentPerson = User.Identity.GetUserId();
            var currentUser = db.businessOwners.Where(x => currentPerson == x.ApplicationId).FirstOrDefault();
            return View(currentUser);
        }

        // GET: BusinessOwner/Create
        public ActionResult Create()
        {
            BusinessOwner owner = new BusinessOwner();
            return View(owner);
        }

        // POST: BusinessOwner/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "BusinessId,FirstName, LastName, Address, City, State, Zipcode")] BusinessOwner businessOwner)
        {
            try
            {
                businessOwner.ApplicationId = User.Identity.GetUserId();
                if (ModelState.IsValid)
                {
                    db.businessOwners.Add(businessOwner);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(businessOwner);
            }
        }


        // GET: BusinessOwner/Edit/5
        public ActionResult Edit(int? id)
        {
            var businessIs = db.businessOwners.Where(b => b.BusinessId == id).FirstOrDefault();
            return View(businessIs);
        }

        // POST: BusinessOwner/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "BusinessId, FirstName, LastName, Address, City, State, Zipcode")] BusinessOwner businessOwner)
        {
            try
            {
                db.Entry(businessOwner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}