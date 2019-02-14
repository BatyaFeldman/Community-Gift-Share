using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaFund.Data
{
    public class SimchaContributor
    {
       
            public int ContributorId { get; set; }
            public string Name { get; set; }
            public bool AlwaysInclude { get; set; }
            public decimal? Amount { get; set; }
            public decimal Balance { get; set; }
        
    }
}
