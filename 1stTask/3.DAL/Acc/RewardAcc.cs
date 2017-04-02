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
        public List<Reward> GetReward(long? id = null)
        {
            using (Entities ent = new Entities())
            {
                return ent.Rewards.Where(_ => id ==null || _.Id == id).ToList();
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
        public bool UpdateReward(long id, string desc, string title)
        {
            using (Entities ent = new Entities())
            {
                ent.Rewards.Where(_ => _.Id == id).FirstOrDefault().Description_ = desc;
                ent.Rewards.Where(_ => _.Id == id).FirstOrDefault().Title = title;
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
        public bool AddRewardImage(long id, string filePart)
        {
            using (Entities ent = new Entities())
            {
                ent.Rewards.Where(_ => _.Id == id).FirstOrDefault().FilePath = filePart;
                ent.SaveChanges();
                return true;
            }
        }

    }
}