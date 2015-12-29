using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineLearningSystem.Models
{
    public class logindetail
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string school { get; set; }
    }
}