using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RouteScheduler.Models;
using Microsoft.AspNet.Identity;

namespace RouteScheduler.Controllers
{
    public class ServiceRequestedsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ServiceRequesteds
        public async Task<ActionResult> Index()
        {
            var currentPerson = User.Identity.GetUserId();
            BusinessOwner businessOwner = db.BusinessOwners.Where(b => b.ApplicationId == currentPerson).FirstOrDefault();
            var serviceRequests = db.ServiceRequests.Where(s => s.BusinessTemplate.BusinessId == businessOwner.BusinessId).Include(s => s.BusinessTemplate).Include(s => s.Customer);
            return View(await serviceRequests.ToListAsync());
        }

        // GET: ServiceRequesteds/Details/5
        public async Task<ActionResult> Details(int? id)
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

        // GET: ServiceRequesteds/Create
        public ActionResult Create()
        {
            ViewBag.TemplateId = new SelectList(db.BusinessTemplates, "TemplateId", "JobName");
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName");
            ViewBag.DayId = new SelectList(db.DaySlots, "id", "PartOfDay");
            return View();
        }

        // POST: ServiceRequesteds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RequestId,TemplateId,PreferredDayOne,PreferredDayTwo,PreferredDayThree,PreferredTime")] ServiceRequested serviceRequested)
        {
            if (ModelState.IsValid)
            {
                db.ServiceRequests.Add(serviceRequested);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TemplateId = new SelectList(db.BusinessTemplates, "TemplateId", "JobName", serviceRequested.TemplateId);
            return View(serviceRequested);
        }

        // GET: ServiceRequesteds/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.TemplateId = new SelectList(db.BusinessTemplates, "TemplateId", "JobName", serviceRequested.TemplateId);
            return View(serviceRequested);
        }

        // POST: ServiceRequesteds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RequestId,TemplateId,PreferredDayOne,PreferredDayTwo,PreferredDayThree,PreferredTime")] ServiceRequested serviceRequested)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceRequested).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TemplateId = new SelectList(db.BusinessTemplates, "TemplateId", "JobName", serviceRequested.TemplateId);
            return View(serviceRequested);
        }

        // GET: ServiceRequesteds/Delete/5
        public async Task<ActionResult> Delete(int? id)
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

        // POST: ServiceRequesteds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceRequested serviceRequested = await db.ServiceRequests.FindAsync(id);
            db.ServiceRequests.Remove(serviceRequested);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
