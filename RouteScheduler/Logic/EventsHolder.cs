using RouteScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RouteScheduler.Logic
{
    public class EventsHolder
    {
            public int UserId { get; set; }
            public Customer Customer { get; set; }
            public string EventName { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

    }
}