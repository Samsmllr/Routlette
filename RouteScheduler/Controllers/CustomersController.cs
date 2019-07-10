using Microsoft.AspNet.Identity;
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
        

        // GET: Customer Details
        public ActionResult Index()
        {
            var currentPerson = User.Identity.GetUserId();
            var currentUser = db.Customers.Where(c => c.ApplicationId == currentPerson).FirstOrDefault();
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

            return View(businessTemplate);
        }

        public ActionResult ScheduleEvent(int? id)
        {
            ServiceRequested serviceRequest = new ServiceRequested();
            BusinessTemplate template = db.BusinessTemplates.Where(b => b.TemplateId == id).FirstOrDefault();
            ViewData["businessOwner"] = template.BusinessId;

        
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



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> CreateServiceRequest([Bind(Include = "RequestId,TemplateId,PreferredDayOne,PreferredDayTwo,PreferredDayThree")] ServiceRequested serviceRequested)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ServiceRequests.Add(serviceRequested);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("ViewRequestedServices");
        //    }

        //    ViewBag.TemplateId = new SelectList(db.BusinessTemplates, "TemplateId", "JobName", serviceRequested.TemplateId);
        //    return View(serviceRequested);
        //}

        //public async Task<ActionResult> ViewRequestedServices()
        //{
        //    var serviceRequests = db.ServiceRequests.Include(s => s.BusinessTemplate).Include(s => s.Customer);
        //    return View(await serviceRequests.ToListAsync());
        //}


        //public ActionResult ViewServices()
        //{
        //    var currentPerson = User.Identity.GetUserId();
        //    var currentUser = db.Customers.Where(c => c.ApplicationId == currentPerson).FirstOrDefault();
        //    ViewBag.TemplateId = new SelectList(db.BusinessTemplates, "TemplateId", "JobName");
        //    ServiceRequested service = new ServiceRequested();
        //    service.Customer = currentUser;
        //    return View(service);
        //}

        //public ActionResult RequestService()
        //{
        //    var ServicesAre = db.BusinessTemplates.ToList();
        //    return View(ServicesAre);
        //}

        //public ActionResult RequestServiceInformation(int id)
        //{
        //    var CurrentService = db.BusinessTemplates.Where(b => b.BusinessId == id).FirstOrDefault();
        //    return View(CurrentService);
        //}

        //[HttpPost]
        //public ActionResult RequestServiceInformation()
        //{
        //    ServiceRequested serviceRequested = new ServiceRequested();
        //    var currentPerson = User.Identity.GetUserId();
        //    var currentUser = db.Customers.Where(c => c.ApplicationId == currentPerson).FirstOrDefault();

        //   // serviceRequested.TemplateId;
        //    serviceRequested.CustomerId = currentUser.CustomerId;



        //    db.ServiceRequests.Add(serviceRequested);
        //    db.SaveChanges();
        //    return View();
        //}

    }
}
