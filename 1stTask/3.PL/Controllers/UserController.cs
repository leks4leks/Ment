using _3.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace _3.PL.Controllers
{
    public class UserController : Controller
    {
        public MainModel model;
        public ActionResult Index()
        {
            model = new MainModel();
            model.Users = new List<UserModel>();
            model.Users.Add(new UserModel { Id = 1, Name = "Aleks", BDay = new DateTime(1993, 3, 15), Age = 24 });
            
            return View(model);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Add(MainModel model)
        {
            if (model.Users == null)
                model.Users = new List<UserModel>();

            model.Users.Add(new UserModel { Id = 2, Name = "Aleks 2", BDay = new DateTime(1991, 3, 15), Age = 26 });

            return View("Index", model);
        }


    }
}
