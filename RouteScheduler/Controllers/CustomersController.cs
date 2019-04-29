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
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Customer Details
        public ActionResult Index()
        {
            var currentPerson = User.Identity.GetUserId();
            var currentUser = db.customers.Where(c => c.ApplicationId == currentPerson).FirstOrDefault();
            return View(currentUser);
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
            customer.ApplicationId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            var customerIs = db.customers.Where(c => c.CustomerId == id).FirstOrDefault();
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

        public ActionResult RequestService()
        {
            var ServicesAre = db.businessTemplates.ToList();
            return View(ServicesAre);
        }

        public ActionResult RequestServiceInformation(int id)
        {
            var CurrentService = db.businessTemplates.Where(b => b.BusinessId == id).FirstOrDefault();
            return View(CurrentService);
        }

        [HttpPost]
        public ActionResult RequestServiceInformation()
        {
            ServiceRequested serviceRequested = new ServiceRequested();
            var currentPerson = User.Identity.GetUserId();
            var currentUser = db.customers.Where(c => c.ApplicationId == currentPerson).FirstOrDefault();

           // serviceRequested.TemplateId;
            serviceRequested.CustomerId = currentUser.CustomerId;



            db.serviceRequests.Add(serviceRequested);
            db.SaveChanges();
            return View();
        }

        public ActionResult CompletedServices()
        {
            return View();
        }

        public ActionResult MyRequests()
        {
            return View();
        }
    }
}
