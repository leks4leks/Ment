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
        public List<UserModel> getUser()
        {
            UserAcc us = new UserAcc();
            var res = new List<UserModel>();
            foreach (var item in us.getUsers())
            {
                res.Add(new UserModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    BDay = item.Birthdate,
                    Age = item.Age
                });
            }

            return res;
        }

        public bool addUser(string name, DateTime? bday)
        {
            UserAcc us = new UserAcc();
            return us.addUsers(name, bday);           
        }

        public bool delUser(int id)
        {
            UserAcc us = new UserAcc();
            return us.delUsers(id);
        }
    }
}