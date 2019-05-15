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


        public List<DateTime> EachDay(int id, DateTime date, TimeSpan duration, ServiceRequested service)
        {
            string GetEvents = webClient.DownloadString("http://localhost:58619/api/events");
            var obj = JsonConvert.DeserializeObject<dynamic>(GetEvents);
            List<DateTime> list = new List<DateTime>();
            DateTime DateIs = date;
            int NumberHold = 0;
            db.businessOwners.Where(b => b.BusinessId == id).FirstOrDefault();


            foreach()
            


            if (list.Count == 0)
            {
                for (int i = 0; i <= 95; i++)
                {
                    DateIs.AddMinutes(15);
                    list.Add(DateIs);
                }
            }
            else
            {
                do
                {
                    //if (DateIs < list[NumberHold].StartTime && DateIs + duration <= list[NumberHold].StartTime)
                    //{
                    //    list.Add(DateIs);
                    //}
                    //else if (DateIs >= list[NumberHold].EndTime && NumberHold < list.Count - 1)
                    //{
                    //    NumberHold += 1;
                    //}
                    //else
                    //{

                    //}
                    //DateIs.AddMinutes(15);
                } while (DateIs.Date == date.Date);

            }

            return list;
        }

    }
}