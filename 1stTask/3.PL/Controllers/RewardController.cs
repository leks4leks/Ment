using System.Web.Mvc;
using _3.BLL.Services;
using _3.PL.Models;

namespace _3.PL.Controllers
{
    public class RewardController : Controller
    {
        public static ReModel ReModel = new ReModel();

        public ActionResult Index()
        {
            loadRewards();
            return View(ReModel);
        }
        public void loadRewards()
        {
            RewardSer ser = new RewardSer();
            ReModel.Rewards = ser.GetReward().Rewards;
        }

        [System.Web.Http.HttpPost]
        public ActionResult Del(int id)
        {
            RewardSer ser = new RewardSer();
            if (ReModel.Rewards != null)
                ser.DelReward(id);
            loadRewards();
            return View("Reward", "Index", ReModel);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Edit(int id)
        {
            RewardSer ser = new RewardSer();
            if (ReModel != null)
                ser.GetReward(id);

            return View("Index", "WorkWithReward", ser);
        }
    }
}