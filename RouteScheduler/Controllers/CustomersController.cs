﻿using Microsoft.AspNet.Identity;
using RouteScheduler.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RouteScheduler.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private APILogic gl = new APILogic();
        private APIKeys aPIKeys = new APIKeys();
        

        // GET: Customer Details
        public ActionResult Index()
        {
            //var currentPerson = User.Identity.GetUserId();
            //var currentUser = db.Customers.Where(c => c.ApplicationId == currentPerson).FirstOrDefault();
            //return View(currentUser);
            var UserId = User.Identity.GetUserId();
            Customer UserIs = db.Customers.Where(b => b.ApplicationId == UserId).FirstOrDefault();
            double lat = db.Customers.Where(b => b.ApplicationId == UserId).FirstOrDefault().Latitude;
            double lng = db.Customers.Where(b => b.ApplicationId == UserId).FirstOrDefault().Longitude;
            string ApiIs = ($"https://maps.googleapis.com/maps/api/js?key=" + aPIKeys.ApiKey + "&callback=initMap");
            ViewData["ApiKey"] = ApiIs;
            ViewData["Lat"] = lat;
            ViewData["Lng"] = lng;
            ViewData["NameIs"] = UserIs.FirstName;
            List<ServiceRequested> UserRequests = new List<ServiceRequested>();

            try
            {
                List<ServiceRequested> events = db.ServiceRequests.Where(r => r.CustomerId == UserIs.CustomerId).ToList();
                return View(events);
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            Customer customer = new Customer();
            return View(customer);
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "CustomerId,FirstName,LastName,Address,City,State,ZipCode")] Customer customer)
        {
            var Geocode = gl.GeocodeAddress(customer.Address, customer.City, customer.State);

            try
            {
                customer.ApplicationId = User.Identity.GetUserId();
                customer.Latitude = Geocode[0];
                customer.Longitude = Geocode[1];
                if (ModelState.IsValid)
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(customer);
            }
        }
            

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            var customerIs = db.Customers.Where(c => c.CustomerId == id).FirstOrDefault();
            return View(customerIs);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "CustomerId,FirstName,LastName,Address,City,State,ZipCode")] Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult ViewBusinessOwners()
        {
            List<BusinessOwner> businessOwnerList = db.BusinessOwners.ToList();
            return View(businessOwnerList);
        }



    public ActionResult ViewBusinessOwnerDetails(int? id)
        {

            ViewBag.BusinessOwner = db.BusinessOwners.Where(b => b.BusinessId == id).FirstOrDefault();
            List<BusinessTemplate> businessTemplate = db.BusinessTemplates.Where(t => t.BusinessId == id).Include(b => b.BusinessOwner).ToList();
            BusinessOwner businessOwner = db.BusinessOwners.Where(b => b.BusinessId == id).FirstOrDefault();
            string DetailsAre = businessOwner.BusinessDetails;
            ViewBag.Details = DetailsAre;
            return View(businessTemplate);
        }

        public ActionResult ScheduleEvent(int? id)
        {
            ServiceRequested serviceRequest = new ServiceRequested();
            BusinessTemplate template = db.BusinessTemplates.Where(b => b.TemplateId == id).FirstOrDefault();
            ViewData["businessOwner"] = template.BusinessId;
            ViewBag.Details = template.JobDetails;

        
            var currentPerson = User.Identity.GetUserId();
            var currentUser = db.Customers.Where(c => c.ApplicationId == currentPerson).FirstOrDefault();
            serviceRequest.BusinessTemplate = template;
            serviceRequest.Customer = currentUser;
            serviceRequest.CustomerId = currentUser.CustomerId;
            serviceRequest.TemplateId = template.TemplateId;
            return View(serviceRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ScheduleEvent([Bind(Include = "RequestId,TemplateId,CustomerId,PreferredDayOne,PreferredDayTwo,PreferredDayThree")] ServiceRequested serviceRequested)
        {
            serviceRequested.Customer = db.Customers.Where(c => c.CustomerId == serviceRequested.CustomerId).FirstOrDefault();
            serviceRequested.BusinessTemplate = db.BusinessTemplates.Where(b => b.TemplateId == serviceRequested.TemplateId).FirstOrDefault();
            try
            {
                if (ModelState.IsValid)
                {
                    db.ServiceRequests.Add(serviceRequested);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(serviceRequested);
            }
            catch
            {

                return View(serviceRequested);
            }

        }

    }
}
