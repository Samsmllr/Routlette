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
            string ApiIs = ("https://maps.googleapis.com/maps/api/js?key=" + aPIKeys.ApiKey + "&callback=initMap");
            ViewData["ApiKey"] = ApiIs;
            //TODO: create portal for website
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

        // GET: BusinessOwner/Create
        public ActionResult CreateServiceTemplate()
        {
            BusinessTemplate template = new BusinessTemplate();
            return View(template);
        }

        // POST: BusinessOwner/Create
        [HttpPost]
        public ActionResult CreateServiceTemplate([Bind(Include = "JobName, Price, ServiceLength")] BusinessTemplate businessTemplate)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var businessId = db.businessOwners.Where(b => b.ApplicationId == userId).FirstOrDefault().BusinessId;

                businessTemplate.BusinessId = businessId;

                if (ModelState.IsValid)
                {
                    db.businessTemplates.Add(businessTemplate);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(businessTemplate);
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

        public ActionResult ViewServiceTemplates()
        {
            var UserResult = User.Identity.GetUserId();
            BusinessOwner currentUser = db.businessOwners.Where(b => b.ApplicationId == UserResult).FirstOrDefault();
            var serviceList = db.businessTemplates.Where(b => b.BusinessId == currentUser.BusinessId).ToList();
            return View(serviceList);
        }
    }
}