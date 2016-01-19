using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineLearningSystem.Models
{
    public class message
    {
        public int id { get; set; }
        public string sender { get; set; }
        [Required]
        public int receiver { get; set; }
        [Required]
        public string usertype { get; set; }
        public string receiver_name { get; set; }
        [DisplayFormat(DataFormatString="{0:d}",ApplyFormatInEditMode=true)]
        public DateTime sentdate { get; set; }
        [Required]
        public string message_body { get; set; }
        [Required]
        public int school_id { get; set; }
        public string message_subject { get; set; }
        public int sender_userid { get; set; }
    }
}