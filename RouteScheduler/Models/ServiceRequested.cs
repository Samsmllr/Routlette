using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RouteScheduler.Models
{
    public class ServiceRequested
    {
        [Key]
        [Display(Name = "ServiceRequestId")]
        public int RequestId { get; set; }

        public int TemplateId { get; set; }
        [ForeignKey("TemplateId")]
        public virtual BusinessTemplate BusinessTemplate { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [Display(Name = "Preferred Day")]
        [DataType(DataType.Date)]
        public DateTime PreferredDayOne { get; set; }

        [Display(Name = "Preferred Day")]
        [DataType(DataType.Date)]
        public DateTime PreferredDayTwo { get; set; }

        [Display (Name = "Preferred Day")]
        [DataType(DataType.Date)]
        public DateTime PreferredDayThree { get; set; }
    }
}