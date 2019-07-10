using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RouteScheduler.Logic;
using RouteScheduler.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
        private APILogic gl = new APILogic();
        private SchedulingLogic sl = new SchedulingLogic();
        private WebClient webClient = new WebClient();
        private HttpClient client = new HttpClient();

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

        public ActionResult ScheduleeDetails(int? id)
        {
            try
            {
                var currentPerson = User.Identity.GetUserId();
                BusinessOwner UserIs = db.BusinessOwners.Where(b => b.ApplicationId == currentPerson).FirstOrDefault();
                Customer customer = db.Customers.Where(c => c.CustomerId == id).FirstOrDefault();
                List<EventsHolder> eventList = gl.GetEventsByIdAndDay(UserIs.BusinessId, DateTime.Now);
                    if (eventList != null)
                {
                    EventsHolder eventIs = eventList.Where(e => e.CustomerId == customer.CustomerId).FirstOrDefault();
                    ViewData["EventStart"] = eventIs.StartDate.TimeOfDay;
                    ViewData["EventEnd"] = eventIs.EndDate.TimeOfDay;
                    ViewData["EventName"] = eventIs.EventName;
                    ViewData["Lat"] = customer.Latitude;
                    ViewData["Lng"] = customer.Longitude;
                }
                return View(customer);
            }
            catch
            {
                return View("Index");
            }
        }

        public ActionResult AssignToSchedule(int? id)
        {
            ServiceRequested service = db.ServiceRequests.Where(s => s.RequestId == id).FirstOrDefault();
            EventsHolder events = new EventsHolder();

            var UserIs = User.Identity.GetUserId();
            BusinessOwner UserIdIs = db.BusinessOwners.Where(b => b.ApplicationId == UserIs).FirstOrDefault();

            Customer customer = db.Customers.Where(c => c.CustomerId == service.CustomerId).FirstOrDefault();
            BusinessTemplate businessTemplate = db.BusinessTemplates.Where(b => b.TemplateId == service.TemplateId).FirstOrDefault();

            events.CustomerId = service.CustomerId;
            events.UserId = UserIdIs.BusinessId;
            events.EventName = customer.LastName + " " + businessTemplate.JobName;
            events.Latitude = customer.Latitude;
            events.Longitude = customer.Longitude;
            var DateListIs = sl.AvailableTimes(businessTemplate.BusinessId, service);
            ViewBag.DateList = new SelectList(DateListIs);
            ViewData["CustomerInformation"] = customer;
            return View(events);
        }


        [HttpPost]
        public async Task<ActionResult> AssignToScheduleAsync([Bind(Include = "CustomerId,UserId,EventName,Latitude,Longitude,StartDate,EndDate")] EventsHolder events)
        {

            try
            {


                string Url = aPIKeys.eventHolder;
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, Url))
                {
                    var json = JsonConvert.SerializeObject(events);
                    using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (var response = await client
                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                            .ConfigureAwait(false))
                        {
                            response.EnsureSuccessStatusCode();
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(events);
            }
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
        public ActionResult Create([Bind(Include = "BusinessId,FirstName, LastName, Address, City, State, Zipcode, DayStart, DayEnd, BusinessName, BusinessDetails")] BusinessOwner businessOwner)
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
        public async Task<ActionResult> Edit([Bind(Include = "BusinessId, ApplicationId, FirstName, LastName, Address, City, State, Zipcode, DayStart, DayEnd")] BusinessOwner businessOwner)
        {
            var Geocode = gl.GeocodeAddress(businessOwner.Address, businessOwner.City, businessOwner.State);
            if (ModelState.IsValid)
            {
                businessOwner.Latitude = Geocode[0];
                businessOwner.Longitude = Geocode[1];
                db.Entry(businessOwner).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(businessOwner);
        }
    }
}