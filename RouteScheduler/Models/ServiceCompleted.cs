using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RouteScheduler.Models
{
    public class ServiceCompleted
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("BusinessOwner")]
        public int BusinessId { get; set; }
        public BusinessOwner BusinessOwner { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Display(Name = "Date Completed")]
        public DateTime DateCompleted { get; set; }
    }
}