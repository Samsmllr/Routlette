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

        [ForeignKey("BusinessTemplate")]
        public int TemplateId { get; set; }
        public BusinessTemplate BusinessTemplate { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}