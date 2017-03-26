using _3.BLL.Acc;
using _3.BLL.Models;
using _3.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3.BLL.Services
{
 
    public class UserSer
    {
        public ListUsers GetUser()
        {
            UserAcc us = new UserAcc();
            var res = new ListUsers();
            res.Users = new List<UserModel>();
            foreach (var item in us.GetUsers())
            {
                res.Users.Add(new UserModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    BDay = item.Birthdate,
                    Age = item.Age
                });
            }

            return res;
        }

        public bool AddUser(string name, DateTime? bday)
        {
            UserAcc us = new UserAcc();
            return us.AddUsers(name, bday);           
        }

        public bool DelUser(int id)
        {
            UserAcc us = new UserAcc();
            return us.DelUsers(id);
        }
    }
}