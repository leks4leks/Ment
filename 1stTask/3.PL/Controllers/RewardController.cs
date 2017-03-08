using _3.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3.BLL.Models;
using _3.BLL.Services;

namespace _3.PL.Controllers
{
    public class RewardController : Controller
    {
        public static MainModel stModel;

        public ActionResult Index()
        {
            stModel = new MainModel();
            loadRewards();
            return View(stModel);
        }
        public void loadRewards()
        {
            RewardSer ser = new RewardSer();
            stModel.Rewards = ser.getReward();
        }

        [System.Web.Http.HttpPost]
        public ActionResult Add(MainModel model)
        {
            RewardSer ser = new RewardSer();
            if (model.AddedReward != null)
                ser.addReward(model.AddedReward.Title, model.AddedReward.Description);
            loadRewards();
            return View("Index", stModel);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Del(int id)
        {
            RewardSer ser = new RewardSer();
            if (stModel.Rewards != null)
                ser.delReward(id);
            loadRewards();
            return View("Index", stModel);
        }
    }
}