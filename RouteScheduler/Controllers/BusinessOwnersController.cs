using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using RouteScheduler.Logic;
using RouteScheduler.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RouteScheduler.Controllers
{
    public class BusinessOwnersController : Controller
    {
        private APIKeys aPIKeys = new APIKeys();
        private ApplicationDbContext db = new ApplicationDbContext();
        private GoogleLogic gl = new GoogleLogic();
        private SchedulingLogic sl = new SchedulingLogic();
        private WebClient webClient = new WebClient();

        // GET: BusinessOwner
        public ActionResult Index()
        {
            var UserId = User.Identity.GetUserId();
            BusinessOwner UserIs = db.BusinessOwners.Where(b => b.ApplicationId == UserId).FirstOrDefault();
            double lat = db.BusinessOwners.Where(b => b.ApplicationId == UserId).FirstOrDefault().Latitude;
            double lng = db.BusinessOwners.Where(b => b.ApplicationId == UserId).FirstOrDefault().Longitude;
            string ApiIs = ($"https://maps.googleapis.com/maps/api/js?key=" + aPIKeys.ApiKey + "&callback=initMap");
            ViewData["ApiKey"] = ApiIs;
            ViewData["Lat"] = lat;
            ViewData["Lng"] = lng;
            ViewData["NameIs"] = UserIs.FirstName;

            try
            {
                 List<EventsHolder> events = gl.GetEventsByIdAndDay(UserIs.BusinessId, DateTime.Now);
            return View(events);
            }
            catch
            {
                return View();
            }
        }





        public ActionResult TodaysRoute()
        {
            var currentPerson = User.Identity.GetUserId();
            var Longitude = db.BusinessOwners.Where(c => c.ApplicationId == currentPerson).FirstOrDefault().Longitude;
            var Latitude = db.BusinessOwners.Where(c => c.ApplicationId == currentPerson).FirstOrDefault().Latitude;
            string DisplayIs = ($"https://www.google.com/maps/embed/v1/view?zoom=16&center={Latitude},{Longitude}&key=" + aPIKeys.ApiKey);
            ViewData["DisplayIs"] = DisplayIs;
            return View();
        }

        public ActionResult Calendar()
        {
            
            return View();
        }


        public ActionResult AssignToSchedule(int? id)
        {
            ServiceRequested service = db.ServiceRequests.Where(s => s.RequestId == id).FirstOrDefault();
            EventsHolder events = new EventsHolder();
           

            events.CustomerId = service.Customer.CustomerId;
            events.UserId = service.BusinessTemplate.BusinessId;
            events.EventName = service.Customer.LastName + " " + service.BusinessTemplate.JobName;
            events.Latitude = service.Customer.Latitude;
            events.Longitude = service.Customer.Longitude;
            var DateListIs = sl.AvailableTimes(service.BusinessTemplate.BusinessId, service);
            ViewBag.DateList = new SelectList(DateListIs);
            ViewData["CustomerInformation"] = service.Customer;
            return View(events);
        }


        [HttpPost]
        public ActionResult AssignToSchedule([Bind(Include = "CustomerId,UserId,EventName,Latitude,Longitude,StartDate,EndDate")] EventsHolder events)
        {

            try
            {
                string json = JsonConvert.SerializeObject(events);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(events);
            }
        }



        public async Task<ActionResult> ScheduleeDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequested serviceRequested = await db.ServiceRequests.FindAsync(id);
            if (serviceRequested == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequested);
        }

        public ActionResult ViewServiceRequests()
        {
            var currentPerson = User.Identity.GetUserId();
            var serviceRequests = db.ServiceRequests.Where(e => e.BusinessTemplate.BusinessOwner.ApplicationId == currentPerson);
            return View(serviceRequests);
        }




        public ActionResult DisplayRoute()
        {
            return View();
        }

        // GET: BusinessOwner/Details/5
        public ActionResult Details()
        {
            var currentPerson = User.Identity.GetUserId();
            var currentUser = db.BusinessOwners.Where(x => currentPerson == x.ApplicationId).FirstOrDefault();
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
        public ActionResult Create([Bind(Include = "BusinessId,FirstName, LastName, Address, City, State, Zipcode, DayStart, DayEnd")] BusinessOwner businessOwner)
        {
            var Geocode = gl.GeocodeAddress(businessOwner.Address, businessOwner.City, businessOwner.State);
            

            try
            {
                businessOwner.ApplicationId = User.Identity.GetUserId();
                businessOwner.Latitude = Geocode[0];
                businessOwner.Longitude = Geocode[1];
                if (ModelState.IsValid)
                {

                    db.BusinessOwners.Add(businessOwner);
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
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessOwner businessOwner = await db.BusinessOwners.FindAsync(id);
            if (businessOwner == null)
            {
                return HttpNotFound();
            }
            return View(businessOwner);
        }

        // POST: BusinessOwner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ApplicationId, Latitude, Longitude, BusinessId, FirstName, LastName, Address, City, State, Zipcode, DayStart, DayEnd")] BusinessOwner businessOwner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessOwner).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(businessOwner);
        }
    }
}