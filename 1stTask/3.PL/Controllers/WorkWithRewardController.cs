using _3.BLL.Services;
using _3.PL.Models;
using System;
using System.IO;
using System.Web.Mvc;
using System.Linq;

namespace _3.PL.Controllers
{
    public class WorkWithRewardController : Controller
    {        
        // GET: AddReward
        public ActionResult Index(long? id)
        {
            RewardSer ser = new RewardSer();
            Reward model = new Reward();
            if (id != null && id != 0)
            {
                var user = ser.GetReward(id).Rewards.FirstOrDefault();
                model.Id = user.Id;
                model.Description = user.Description;
                model.Title = user.Title;
                model.FilePath = user.FilePath;
            }
            return View(model);
        }
        
        [System.Web.Http.HttpPost]
        public ActionResult Add(Reward model)
        {
            RewardSer ser = new RewardSer();
            if (model != null && ser.GetReward(model.Id).Rewards.Count == 0)
                ser.AddReward(model.Title, model.Description);
            else
                ser.UpdateReward(model);

            return Redirect("~/Reward");
        }

        [System.Web.Http.HttpPost]
        public ActionResult AddRewardImage(Reward model)
        {
            if (model.Id == 0)
                return null;

            RewardSer ser = new RewardSer();

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    model.FilePath = Path.GetFileName((Guid.NewGuid()).ToString()) + "." + Path.GetFileName(file.FileName).Split('.').ToList().Last();
                    var path = Path.Combine(Server.MapPath("~/Images/"), model.FilePath);
                    file.SaveAs(path);
                }
            }

            ser.AddRewardImage(model);
            return Redirect("~/Reward");
        }
    }
}