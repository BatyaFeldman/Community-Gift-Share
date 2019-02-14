using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaFund.Data;

namespace SimchaFund.Models
{
    public class ContributorViewModel
    {
        public IEnumerable<Contributor> Contributors { get; set; }
    }
}