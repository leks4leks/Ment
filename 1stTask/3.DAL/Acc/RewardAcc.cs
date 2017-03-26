using _3.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;

namespace _3.BLL.Acc
{
    public class RewardAcc
    {
        public List<Reward> GetReward()
        {
            using (Entities ent = new Entities())
            {
                return ent.Rewards.ToList();
            }
        }

        public bool AddReward(string title, string desc)
        {
            using (Entities ent = new Entities())
            {
                ent.Rewards.Add(new Reward { Title = title, Description_ = desc });
                ent.SaveChanges();
                return true;
            }
        }
        public bool DelReward(int id)
        {
            using (Entities ent = new Entities())
            {
                ent.Rewards.Remove(ent.Rewards.Where(_ => _.Id == id).FirstOrDefault());
                ent.SaveChanges();
                return true;
            }
        }

    }
}