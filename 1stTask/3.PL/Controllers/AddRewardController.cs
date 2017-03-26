using _3.BLL.Services;
using _3.PL.Models;
using System.Web.Mvc;

namespace _3.PL.Controllers
{
    public class AddRewardController : Controller
    {        
        // GET: AddReward
        public ActionResult Index()
        {
            return View();
        }
        
        [System.Web.Http.HttpPost]
        public ActionResult Add(Reward model)
        {
            RewardSer ser = new RewardSer();
            if (model != null)
                ser.AddReward(model.Title, model.Description);
            return Redirect("~/Reward");
        }
    }
}