using Newtonsoft.Json;
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
        private GoogleLogic gl = new GoogleLogic();
        private static readonly HttpClient client = new HttpClient();


        public List<DateTime> AvailableTimes(int id, ServiceRequested service)
        {
            List<DateTime> dateListInitial = new List<DateTime>() {service.PreferredDayOne, service.PreferredDayTwo, service.PreferredDayThree };
            List<DateTime> DateListModified = new List<DateTime>();
            
            for(int i = 0; i < dateListInitial.Count; i++)
            {
                //querry

                //where querry date == dateListInitial[i].Date and int id == querry id
                //foreach var singleevent in querried events return to datelistinbetween
            
            }
            
             return DateListModified;
        }
        
    }
}