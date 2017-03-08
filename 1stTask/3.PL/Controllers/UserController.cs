using _3.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using _3.BLL.Models;
using _3.BLL.Services;

namespace _3.PL.Controllers
{
    public class UserController : Controller
    {
        public static MainModel stModel;
        
        public ActionResult Index()
        {
            stModel = new MainModel();
            loadUsers();
            return View(stModel);
        }

        public void loadUsers()
        {
            UserSer ser = new UserSer();
            stModel.Users = ser.getUser();
        }

        [System.Web.Http.HttpPost]
        public ActionResult Add(MainModel model)
        {
            UserSer ser = new UserSer();
            if (model.AddedUser != null)
                ser.addUser(model.AddedUser.Name, model.AddedUser.BDay);
            loadUsers();
            return View("Index", stModel);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Del(int id)
        {
            UserSer ser = new UserSer();
            if (stModel.Users != null)
                ser.delUser(id);

            loadUsers();
            return View("Index", stModel);
        }
    }
}
