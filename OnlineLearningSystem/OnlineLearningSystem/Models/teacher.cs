﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineLearningSystem.Models
{
    public class teacher
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string course { get; set; }
        public string qualification { get; set; }
        public int schoolid { get; set; }
        public string usertype { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string schoolname { get; set; }


    }
}