using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineLearningSystem.Models
{
    public class Review
    {
        public int id { get; set; }
        public int studentid { get; set; }
        public int teacherid { get; set; }
        public int stars { get; set; }
    }
}