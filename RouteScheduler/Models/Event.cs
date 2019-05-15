using RouteScheduler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RouteScheduler.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [ForeignKey("BusinessOwner")]
        public int BusinessId { get; set; }
        public BusinessOwner BusinessOwner { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public string EventName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


    }
}