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
    public class BusinessTemplatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BusinessTemplates
        public async Task<ActionResult> Index()
        {
            var currentPerson = User.Identity.GetUserId();
            BusinessOwner businessOwner = db.BusinessOwners.Where(b => b.ApplicationId == currentPerson).FirstOrDefault();
            var businessTemplates = db.BusinessTemplates.Where(b => b.BusinessId == businessOwner.BusinessId).Include(b => b.BusinessOwner);
            return View(await businessTemplates.ToListAsync());
        }

        // GET: BusinessTemplates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessTemplate businessTemplate = await db.BusinessTemplates.FindAsync(id);
            if (businessTemplate == null)
            {
                return HttpNotFound();
            }
            return View(businessTemplate);
        }

        // GET: BusinessTemplates/Create
        public ActionResult Create()
        {
            BusinessTemplate businessTemplate = new BusinessTemplate();
            ViewBag.BusinessId = new SelectList(db.BusinessOwners, "BusinessId", "FirstName");
            return View(businessTemplate);
        }

        // POST: BusinessTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "JobName,Price,ServiceLength")] BusinessTemplate businessTemplate)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var businessId = db.BusinessOwners.Where(b => b.ApplicationId == userId).FirstOrDefault().BusinessId;

                businessTemplate.BusinessId = businessId;

                if (ModelState.IsValid)
                {
                    db.BusinessTemplates.Add(businessTemplate);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(businessTemplate);
            }
        }

        // GET: BusinessTemplates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessTemplate businessTemplate = await db.BusinessTemplates.FindAsync(id);
            if (businessTemplate == null)
            {
                return HttpNotFound();
            }
            return View(businessTemplate);
        }

        // POST: BusinessTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TemplateId,BusinessId,JobName,Price,ServiceLength")] BusinessTemplate businessTemplate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessTemplate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessId = new SelectList(db.BusinessOwners, "BusinessId", "FirstName", businessTemplate.BusinessId);
            return View(businessTemplate);
        }

        // GET: BusinessTemplates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessTemplate businessTemplate = await db.BusinessTemplates.FindAsync(id);
            if (businessTemplate == null)
            {
                return HttpNotFound();
            }
            return View(businessTemplate);
        }

        // POST: BusinessTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BusinessTemplate businessTemplate = await db.BusinessTemplates.FindAsync(id);
            db.BusinessTemplates.Remove(businessTemplate);
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
