﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineLearningSystem.Models
{
    public class Assignments
    {
        public HttpPostedFileBase File { set; get; }
        public int id { get; set; }

        public string name { get; set; }

        public string resources { get; set; }
        public string sender { get; set; }

        public int receiver { get; set; }
        public DateTime enddate { get; set; }
        public DateTime startdate { get; set; }

        public string description { get; set; }

        public string filename { get; set; }

        public string Question1 { get; set; }

        public string Question2 { get; set; }

        public string Question3 { get; set; }
        public string Question4 { get; set; }
        public string Question5 { get; set; }


    }
}