using _3.BLL.Models;
using _3.BLL.Services;
using _3.PL.Models;
using System;
using System.IO;
using System.Linq;

using System.Web.Mvc;

namespace _3.PL.Controllers
{
    public class WorkWithUserController : Controller
    {
        // GET: AddUser
        public ActionResult Index(long? id)
        {
            UserSer ser = new UserSer();
            User model = new User();
            if (id != null && id != 0)
            {
               var user = ser.GetUser(id).Users.FirstOrDefault();
                model.Id = user.Id;
                model.Name = user.Name;
                model.Age = user.Age;
                model.BDay = user.BDay;
                model.FilePath = user.FilePath;
            }
            return View(model);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Add(User model)
        {
            UserSer ser = new UserSer();
            
            if (model != null && ser.GetUser(model.Id).Users.Count == 0)
                ser.AddUser(model.Name, model.BDay);
            else
                ser.UpdateUser(model);
            return Redirect("~/User");
        }

        [System.Web.Http.HttpPost]
        public ActionResult AddUserImage(User model)
        {
            if (model.Id == 0)
                return null;

            UserSer ser = new UserSer();

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
            
            ser.AddUserImage(model);
            return Redirect("~/User");
        }

        
    }
}