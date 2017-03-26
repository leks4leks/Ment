using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _3.BLL.Services;
using _3.PL.Models;

namespace _3.PL.Controllers
{
    public class UserController : Controller
    {
        public static UsModel UsModel = new UsModel();
        
        public ActionResult Index()
        {
            loadUsers();
            return View(UsModel);
        }

        public void loadUsers()
        {
            UserSer ser = new UserSer();
            UsModel.Users = ser.GetUser().Users;
        }

        [System.Web.Http.HttpPost]
        public ActionResult Del(int id)
        {
            UserSer ser = new UserSer();
            if (UsModel != null)
                ser.DelUser(id);

            loadUsers();
            return View("Index", UsModel);
        }
    }
}
