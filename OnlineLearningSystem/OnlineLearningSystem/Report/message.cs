//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineLearningSystem.Report
{
    using System;
    using System.Collections.Generic;
    
    public partial class message
    {
        public int id { get; set; }
        public string sender { get; set; }
        public int receiver { get; set; }
        public string usertype { get; set; }
        public Nullable<System.DateTime> senddate { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public int schoolid { get; set; }
    }
}