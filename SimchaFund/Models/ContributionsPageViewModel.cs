using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaFund.Data;

namespace SimchaFund.Models
{
    public class ContributionsPageViewModel
    {
        public IEnumerable<SimchaContributor> ContributorsForSimcha { get; set; }
        public Simcha Simcha { get; set; }
    }
}