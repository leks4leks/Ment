using _3.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;

namespace _3.BLL.Acc
{
    public class UserAcc
    {
        public List<User> getUsers()
        {
            using (Entities ent = new Entities())
            {
                return ent.Users.ToList();
            }
        }

        public bool addUsers(string name, DateTime? bday)
        {
            using (Entities ent = new Entities())
            {
                ent.Users.Add(new User { Name = name, Birthdate = bday, Age = (DateTime.Today - (DateTime)bday).Days / 365 });
                ent.SaveChanges();
                return true;
            }
        }
        public bool delUsers(int id)
        {
            using (Entities ent = new Entities())
            {
                ent.Users.Remove(ent.Users.Where(_ => _.Id == id).FirstOrDefault());
                ent.SaveChanges();
                return true;
            }
        }

    }
}