using _3.BLL.Acc;
using _3.BLL.Models;
using _3.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3.BLL.Services
{
    public class RewardSer
    {
        public ListReward GetReward()
        {
            RewardAcc us = new RewardAcc();
            var res = new ListReward();
            res.Rewards = new List<RewardModel>();
            foreach (var item in us.GetReward())
            {
                res.Rewards.Add(new RewardModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description_
                });
            }

            return res;
        }

        public bool AddReward(string title, string desc)
        {
            RewardAcc us = new RewardAcc();
            return us.AddReward(title, desc);           
        }

        public bool DelReward(int id)
        {
            RewardAcc us = new RewardAcc();
            return us.DelReward(id);
        }
    }
}