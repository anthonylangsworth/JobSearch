//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobSearch.Serialization
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobOpening
    {
        public JobOpening()
        {
            this.Activities = new HashSet<Activity>();
            this.Contacts = new HashSet<Contact>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public string Url { get; set; }
    
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
