using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaFund.Data
{
   public class Contributor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool AlwaysInclude { get; set; }
        public decimal Balance { get; internal set; }
        public DateTime Date { get; set; }
    }
}
