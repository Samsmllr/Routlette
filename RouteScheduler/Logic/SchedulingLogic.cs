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





        public void AvailableSchedulingTimes(ServiceRequested serviceRequested)
        {
            //var AvailableDays = insert query days here
            List<DateTime> List = new List<DateTime>();

            //foreach Datetime that is equal to the hour, or equal to hour 15 minutes

            if(serviceRequested.PreferredDayOne == null)
            {
                for(int i = 0; i <= 2; i++)
                {

                }
            }
            
            //TODO: Filter by client days
            //TODO: Filter by client distance to other scheduled clients
            //TODO Filter by open availability of businessOwner
            
        }



        public List<DateTime> EachDay(int id, DateTime date, TimeSpan duration)
        {
            string GetEvents = webClient.DownloadString("http://localhost:58619/api/events");
            var obj = JsonConvert.DeserializeObject<dynamic>(GetEvents);
            List<DateTime> list = new List<DateTime>();
            DateTime DateIs = date;
            int NumberHold = 0;



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