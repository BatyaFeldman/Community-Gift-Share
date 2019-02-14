using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaFund.Data;

namespace SimchaFund.Models
{
    public class HistoryViewModel
    {
        public Contributor Contributor { get; set; }
        public IEnumerable<History> History { get; set; }
    }
}