using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalarsGymSet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SalarsGymSet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Helper _helper = new Helper();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Get all gymSets
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var gymsets = _helper.GetGymSetForUser(User.Identity.Name);
                return View(gymsets);
            }
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="submit"></param>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(string submit, string difficulty, DateTime date)
        {

            if (submit.Equals("Sort"))
            {
                return View(_helper.GetGymSetByPriority(User.Identity.Name, difficulty));
            }
            else if (submit.Equals("Filter") && !string.IsNullOrEmpty(difficulty))
            {
                return View(_helper.GetFilteredGymSet(User.Identity.Name, difficulty));
            }
            //else if (submit.Equals("Show"))
            //{
            //    return View(_helper.GetTodosWithDate(User.Identity.Name, date));
            //}
            return Redirect("/Home");
        }


        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// create a gymset
        /// </summary>
        /// <param name="exercise"></param>
        /// <param name="set"></param>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public IActionResult Create(Gymset gymset)
        {
            var gymsets = _helper.GetGymSetForUser(User.Identity.Name);
            if (gymsets.Count >= 10)
            {
                TempData["textmsg"] = "<script>alert('you have reached your excersice limit.);</script>";
                return Redirect("/Home");
            }
            _helper.SaveGymSet(gymset);
            return Redirect("/Home");
        }

        public IActionResult Edit(string id)
        {
            return View(_helper.GetGymSetById(id));
        }

        [HttpPost]
        public IActionResult Edit(string id, string exercise, string set, string difficulty)
        {
            _helper.EditGymSet(id,exercise, set, difficulty);
            return Redirect($"/Home");
        }

        public IActionResult Delete(string id)
        {
            return View(_helper.GetGymSetById(id));
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            _helper.DeleteGymSetById(id);
            return Redirect("/Home");
        }

        public IActionResult PrivateTrainer(string submit, DateTime date)
        {
            if (User.Identity.IsAuthenticated)
            {
                Account account = _helper.GetAcoount(User.Identity.Name);
                if (account.privateTrainer == false)
                {
                    return View();
                }
                else
                {
                    TempData["errorMessage"] = "You can cant register as a private trainer twice, because you already have confirmed it";
                    return Redirect("/Home");
                }
            }
            else if (!User.Identity.IsAuthenticated)
            {
                TempData["textmsg"] = "<script>alert('you have to log in first.');</script>";
                return Redirect("/Home");
            }
            return Redirect("/Home");
        }
        [HttpPost]
        public IActionResult PrivateTrainer(PrivateTrainerCard privatetrainerCard)
        {
            if (privatetrainerCard.PrivateTrainerId.Any(c => char.IsDigit(c)))
            {
                TempData["textmsg"] = "<script>alert('The name must contain only alpha characters.');</script>";
                return View();
            }
            _helper.UpdatePrivateTrainerGymSet(User.Identity.Name);
            TempData["textmsg"] = "<script>alert(' Your private trainer id has been confirmed, you can now create unlimted exercises.');</script>";
            return Redirect("/Home");
        }

        public IActionResult About()
        {
            return View();

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
