using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RouteScheduler.Models
{
    public class SchedulingLogic
    {
        ApplicationDbContext db = new ApplicationDbContext();






        public void AvailableSchedulingTimes(ServiceRequested serviceRequested)
        {
            //var AvailableDays = insert query days here
            int TimeSlots;
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
    }
}