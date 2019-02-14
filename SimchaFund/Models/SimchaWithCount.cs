using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaFund.Data;

namespace SimchaFund.Models
{
    public class SimchaWithCount
    {
        
            public Simcha Simcha { get; set; }
            public int ContributionsPerSimchaCount { get; set; }
        
    }
}