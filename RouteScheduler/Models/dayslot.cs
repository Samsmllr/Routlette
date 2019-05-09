using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RouteScheduler.Models
{
    public class DaySlot
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "Time")]
        public string PartOfDay { get; set; }
    }
}