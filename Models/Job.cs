//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HrmPractise02
{
    using System;
    using System.Collections.Generic;
    
    public partial class Job
    {
        public int Jid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Salary { get; set; }
        public Nullable<int> Uid { get; set; }
        public Nullable<int> LastDateOfApply { get; set; }
        public string Location { get; set; }
    
        public virtual User User { get; set; }
    }
}
