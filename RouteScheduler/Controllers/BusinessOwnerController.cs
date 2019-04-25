using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RouteScheduler.Controllers
{
    public class BusinessOwnerController : Controller
    {
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
            return View();
        }

        // POST: BusinessOwner/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: BusinessOwner/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BusinessOwner/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: BusinessOwner/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BusinessOwner/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
