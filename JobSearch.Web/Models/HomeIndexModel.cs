using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobSearch.Web.Models
{
    public class HomeIndexModel
    {
        [Required]
        public IEnumerable<string> Activities;
    }
}