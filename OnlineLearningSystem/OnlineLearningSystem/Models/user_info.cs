using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineLearningSystem.Models
{
    public class user_info
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public string username { get; set; }

        public string password { get; set; }
        public int schoolid { get; set; }

        public int courseid { get; set; }

    }
}