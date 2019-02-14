using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimchaFund.Data;
using SimchaFund.Models;

namespace SimchaFund.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult SimchaIndex()
        {
            HomePageViewModel vm = new HomePageViewModel();
            if (TempData["SimchaAdded"] != null)
            {
                vm.Message = (string)TempData["SimchaAdded"];
            }
            if (TempData["ContributionsUpdated"] != null)
            {
                vm.Message = (string)TempData["ContributionsUpdated"];
            }

            SimchaDb sdb = new SimchaDb(Properties.Settings.Default.ConStr);
            IEnumerable<Simcha> simchas = sdb.GetSimchas();
           List<SimchaWithCount> simchasWithCount = new List<SimchaWithCount>();
            foreach(Simcha s in simchas)
            {
                SimchaWithCount swc = new SimchaWithCount();
                swc.Simcha = s;
                swc.ContributionsPerSimchaCount = sdb.CountContributionsPerSimcha(s.Id);
                simchasWithCount.Add(swc);
            }
            vm.SimchasWithCount = simchasWithCount;
            vm.ContributorCount = sdb.GetContributorCount();
            return View(vm);
        }

        [HttpPost]
        public ActionResult AddSimcha(Simcha simcha)
        {
            SimchaDb sdb = new SimchaDb(Properties.Settings.Default.ConStr);
            sdb.AddSimcha(simcha);
            TempData["SimchaAdded"] = $"{simcha.Name} simcha added";
            return Redirect("/Home/SimchaIndex");
        }

        public ActionResult ContributionsForSimcha(int simchaId)
        {
            ContributionsPageViewModel cpvm = new ContributionsPageViewModel();
            SimchaDb sdb = new SimchaDb(Properties.Settings.Default.ConStr);
            Simcha simcha = sdb.GetSimchaById(simchaId);
            IEnumerable<SimchaContributor> contributors = sdb.GetSimchaContributors(simchaId);
            cpvm.Simcha = simcha;
            cpvm.ContributorsForSimcha = contributors;
            return View(cpvm);
        }

       

        [HttpPost]
        public ActionResult UpdateContributions(int simchaId, IEnumerable<ContributionInclusion> includedContributions)
        {
            SimchaDb sdb = new SimchaDb(Properties.Settings.Default.ConStr);
            sdb.UpdateContributions(simchaId, includedContributions);
            TempData["ContributionsUpdated"] = $"Contributions for Simcha {simchaId} have been updated";
            return Redirect("/Home/SimchaIndex");
        }

        public ActionResult Email(int simchaId)
        {
            SimchaDb sdb = new SimchaDb(Properties.Settings.Default.ConStr);
            IEnumerable<String>EmailList=sdb.EmailListForSimcha(simchaId);
            return View(EmailList);

        }

    }
}