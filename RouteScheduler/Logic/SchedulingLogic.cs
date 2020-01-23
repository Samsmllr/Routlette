using Newtonsoft.Json;
using RouteScheduler.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RouteScheduler.Models
{
    public class SchedulingLogic
    {
        readonly ApplicationDbContext db = new ApplicationDbContext();
        private WebClient webClient = new WebClient();
        private APILogic gl = new APILogic();
        private static readonly HttpClient client = new HttpClient();


        public List<DateTime> ListTimes(int id, ServiceRequested service)
        {
            List<DateTime> preferredDaysList = new List<DateTime>() { service.PreferredDayOne, service.PreferredDayTwo, service.PreferredDayThree };
            List<DateTime> DateListModified = new List<DateTime>();
            BusinessOwner businessOwner = db.BusinessOwners.Where(b => b.BusinessId == id).FirstOrDefault();
            BusinessTemplate businessTemplate = db.BusinessTemplates.Where(t => t.TemplateId == service.TemplateId).FirstOrDefault();
            
            for(int i = 0; i < preferredDaysList.Count; i++)
            {
                bool serviceHappeningOnDay = serviceOnPreferredDays(businessOwner.BusinessId, preferredDaysList[i]);
                if (serviceHappeningOnDay == true)
                {

                }
                else
                {
                    DateTime startingDateAndHour = preferredDaysList[i].Add(businessOwner.DayStart);
                    DateTime endingDateAndHour = preferredDaysList[i].Add((businessOwner.DayEnd) - businessTemplate.ServiceLength);

                    List<DateTime> newDates = BusinessDaySchedule(startingDateAndHour, endingDateAndHour, 15);
                    DateListModified.AddRange(newDates);
                }
            }
            return DateListModified;
        }


        private bool serviceOnPreferredDays(int id, DateTime day)
        {
            DateTime dayIs = day;

            if (gl.GetEventsByIdAndDay(id, dayIs) == null)
            {
                return false;
            }
            return true;
        }

        private List<DateTime> BusinessDaySchedule(DateTime startingHour, DateTime endingHour, int timeIncriments)
        {
            List<DateTime> dateTimes = new List<DateTime>();
            DateTime addTime = startingHour;
            do
            {
                dateTimes.Add(addTime);
                addTime = addTime.AddMinutes(timeIncriments);

            } while (addTime <= endingHour);
            return dateTimes;
        }




        public List<DateTime> AvailableTimes(int id, ServiceRequested service)
        {
            List<DateTime> dateListInitial = new List<DateTime>() {service.PreferredDayOne, service.PreferredDayTwo, service.PreferredDayThree };
            List<EventsHolder> EventsList;
            List<DateTime> DateListModified = new List<DateTime>();
            BusinessOwner businessOwner = db.BusinessOwners.Where(b => b.BusinessId == id).FirstOrDefault();
            
            for(int i = 0; i < dateListInitial.Count; i++)
            {
                EventsList = gl.GetEventsByIdAndDay(id, dateListInitial[i]);
                DateTime time = dateListInitial[i].Date + businessOwner.DayStart;
                Date date = new Date();

                if (EventsList.Count <= 0)
                {
                    do
                    {
                        DateListModified.Add(time);
                        time = time.AddMinutes(15);
                    }
                    while (time + service.BusinessTemplate.ServiceLength <= dateListInitial[i].Date + businessOwner.DayEnd);
                }


                else
                {
                    foreach(var eventIs in EventsList)
                    {
                        do
                        {
                            if (eventIs.StartDate >= time + service.BusinessTemplate.ServiceLength)
                            {
                                DateListModified.Add(time);
                                time = time.AddMinutes(15);
                            }
                            else if (eventIs.StartDate < time + service.BusinessTemplate.ServiceLength)
                            {
                                time = time.Add(service.BusinessTemplate.ServiceLength);
                            }
                        } while (time < eventIs.StartDate);
                    }
                    if (time + service.BusinessTemplate.ServiceLength <= dateListInitial[i].Date + businessOwner.DayEnd)
                    {
                        do
                        {
                            DateListModified.Add(time);
                            time = time.AddMinutes(15);
                        }
                        while (time + service.BusinessTemplate.ServiceLength <= dateListInitial[i].Date + businessOwner.DayEnd);
                    }
                }
            
            }
            
             return DateListModified;
        }
        
    }
}