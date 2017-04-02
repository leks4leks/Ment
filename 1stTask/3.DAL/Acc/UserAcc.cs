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
        public List<User> GetUsers(long? id = null)
        {
            using (Entities ent = new Entities())
            {
                return ent.Users.Where(_ => id == null ||  _.Id == id).ToList();
            }
        }

        public bool AddUsers(string name, DateTime? bday)
        {
            using (Entities ent = new Entities())
            {
                ent.Users.Add(new User { Name = name, Birthdate = bday, Age = (DateTime.Today - (DateTime)bday).Days / 365 });
                ent.SaveChanges();
                return true;
            }
        }
        public bool UpdateUsers(long id, string name, DateTime? bday)
        {
            using (Entities ent = new Entities())
            {
                ent.Users.Where(_ => _.Id == id).FirstOrDefault().Name = name;
                ent.Users.Where(_ => _.Id == id).FirstOrDefault().Birthdate = bday;
                ent.SaveChanges();
                return true;
            }
        }
        public bool AddUsersImage(long id, string filePart)
        {
            using (Entities ent = new Entities())
            {
                ent.Users.Where(_ => _.Id == id).FirstOrDefault().FilePath = filePart;
                ent.SaveChanges();
                return true;
            }
        }
        public bool DelUsers(int id)
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