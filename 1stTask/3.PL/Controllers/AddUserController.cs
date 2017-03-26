using _3.BLL.Services;
using _3.PL.Models;

using System.Web.Mvc;

namespace _3.PL.Controllers
{
    public class AddUserController : Controller
    {
        // GET: AddUser
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult Add(User model)
        {
            UserSer ser = new UserSer();
            if (model != null)
                ser.AddUser(model.Name, model.BDay);
            return Redirect("~/User");
        }
    }
}