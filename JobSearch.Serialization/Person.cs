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
    
    public partial class Person
    {
        public Person()
        {
            this.JobOpeningPersons = new HashSet<JobOpeningPerson>();
        }
    
        public int Id { get; set; }
        public string Company { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
    
        public virtual ICollection<JobOpeningPerson> JobOpeningPersons { get; set; }
    }
}
