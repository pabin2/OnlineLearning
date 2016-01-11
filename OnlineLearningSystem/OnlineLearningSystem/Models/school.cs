using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineLearningSystem.Models
{
    public class School
    {
        public int id { get; set; }
        public string SchoolName { get; set; }
        public string Location { get; set; }
        public Int64 Contact { get; set; }



    }
}