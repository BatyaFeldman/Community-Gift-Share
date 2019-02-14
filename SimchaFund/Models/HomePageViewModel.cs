using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaFund.Data;

namespace SimchaFund.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<SimchaWithCount> SimchasWithCount { get; set; }
        public int ContributorCount { get; set; }
        public string Message { get; set; }
        
    }

   
}