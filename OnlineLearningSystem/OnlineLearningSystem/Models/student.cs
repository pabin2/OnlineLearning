using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineLearningSystem.Models
{
    public class student
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string course { get; set; }
        public string sex { get; set; }
        public int teacherid { get; set; }
        public string usertype { get; set; }
        public DateTime signupdate { get; set; }
        public DateTime enddate { get; set; }



    }
}