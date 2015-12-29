using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineLearningSystem.Models
{
    public class Assignments
    {
        public int id { get; set; }

        public string assignment_name { get; set; }

        public string assignment_resources { get; set; }
        public string sender { get; set; }

        public int receiver { get; set; }
        public DateTime enddate { get; set; }
        public DateTime startdate { get; set; }

        public string description { get; set; }

        public string filename { get; set; }


    }
}