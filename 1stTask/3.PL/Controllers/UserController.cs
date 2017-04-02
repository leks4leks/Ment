using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _3.BLL.Services;
using _3.PL.Models;
using System.IO;

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

        [System.Web.Http.HttpPost]
        public ActionResult Edit(int id)
        {
            UserSer ser = new UserSer();
            if (UsModel != null)
                ser.GetUser(id);
            
            return View("Index", "WorkWithUser", ser);
        }

        public FilePathResult Download()
        {
            string name = "~/Files/data.txt";
            System.IO.File.Delete(Server.MapPath(name));

            FileInfo info = new FileInfo(name);
            if (!info.Exists)
            {
                UserSer ser = new UserSer();
                using (StreamWriter writer = new StreamWriter(Server.MapPath(name), true))
                {
                    foreach(var item in ser.GetUser().Users)
                        writer.WriteLine(item.Name + "  -  " + item.BDay + "  -  " + (item.BDay != null ? ((DateTime.Today - (DateTime)(item.BDay)).Days / 365).ToString() : ""));

                }
            }

            return File(name, "text/plain");

        }
    }
}
