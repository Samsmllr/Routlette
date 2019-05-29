using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace RouteScheduler.Models
{
    public class SchedulingLogic
    {
        readonly ApplicationDbContext db = new ApplicationDbContext();
        private WebClient webClient = new WebClient();
        private GoogleLogic gl = new GoogleLogic();


        public void GetEvents(int id)
        {
            string getEvents = webClient.DownloadString($"http://localhost:58619/api/events");


        }


        public List<DateTime> AvailableTimes(int id, ServiceRequested service)
        {
            List<DateTime> dateListInitial = new List<DateTime>() {service.PreferredDayOne, service.PreferredDayTwo, service.PreferredDayThree };
            List<DateTime> DateListModified = new List<DateTime>();
            
            for(int i = 0; i < dateListInitial.Count; i++)
            {
                List<Event> EventList = db.Events.Where(e => e.BusinessId == id).ToList().Where(e => e.StartDate.Date == dateListInitial[i].Date).ToList();


            }
            //foreach (DateTime day in dateList)
            //{
            //    List<Event> dayEvents = db.Events.Where(e => e.StartDate.Date == day.Date).ToList();
            //    if(dayEvents.Count == 0)
            //    {

            //    }
            //    else
            //    {
            //        foreach (Event @event in dayEvents)
            //        {
            //            double distance = gl.DistanceBetweenTwoPlaces(@event.Latitude, @event.Longitude, service.Customer.Latitude, service.Customer.Longitude);
            //            if (distance >= 15)
            //            {
            //                dateList.Remove(day);
            //            }
            //        }
            //    }
            //}
             return DateListModified;
        }

        //private bool EventsArentTooFar(int id, DateTime date)
        //{
            
        //}
        
    }
}