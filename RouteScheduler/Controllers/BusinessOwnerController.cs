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
    public class BusinessOwnerController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: BusinessOwner
        public ActionResult Index()
        {

            //TODO: create portal for website
            return View();
        }

        // GET: BusinessOwner/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
