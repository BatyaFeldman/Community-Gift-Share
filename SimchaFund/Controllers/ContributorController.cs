using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimchaFund.Data;
using SimchaFund.Models;

namespace SimchaFund.Controllers
{
    public class ContributorController : Controller
    {
        
        public ActionResult ContributorIndex()
        {
            ContributorViewModel cvm = new ContributorViewModel();
            SimchaDb sdb = new SimchaDb(Properties.Settings.Default.ConStr);
            cvm.Contributors = sdb.GetContributors();
            return View(cvm);
        }

        [HttpPost]
        public ActionResult AddContributor(string Name, string PhoneNumber, decimal Balance, bool AlwaysInclude, DateTime Date)
        {
            SimchaDb sdb = new SimchaDb(Properties.Settings.Default.ConStr);
            int x = sdb.AddContributor(Name, PhoneNumber, AlwaysInclude, Date);

            sdb.AddDeposit(x, Balance, Date);

            return Redirect("/Contributor/ContributorIndex");
        }

        [HttpPost]
        public ActionResult EditContributor(int Id, string Name, string PhoneNumber, bool AlwaysInclude, DateTime Date)
        {
            SimchaDb sdb = new SimchaDb(Properties.Settings.Default.ConStr);
            sdb.EditContributor(Id, Name, PhoneNumber, AlwaysInclude, Date);
            return Redirect("/Contributor/ContributorIndex");

        }
        [HttpPost]
        public ActionResult AddDeposit(int contributorId, decimal amount, DateTime date)
        {
            SimchaDb sdb = new SimchaDb(Properties.Settings.Default.ConStr);
            sdb.AddDeposit(contributorId, amount, date);
            return Redirect("/Contributor/ContributorIndex");
        }

        public ActionResult History(int contributorId)
        {
            SimchaDb sdb = new SimchaDb(Properties.Settings.Default.ConStr);
            HistoryViewModel hvm = new HistoryViewModel();
            IEnumerable<History>contributorHistory=sdb.GetContributionsForContrib(contributorId);
            IEnumerable<History> depositHistory = sdb.GetDepositsForContrib(contributorId);
            Contributor contributor = sdb.GetContributorById(contributorId);
            hvm.History = contributorHistory.Concat(depositHistory).OrderBy(h=>h.Date.DayOfYear).ThenBy(h=>h.Date.TimeOfDay).ToList() ;
            hvm.Contributor = contributor;
            return View(hvm);
        }
    }
}