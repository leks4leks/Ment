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
        public List<RewardModel> getReward()
        {
            RewardAcc us = new RewardAcc();
            var res = new List<RewardModel>();
            foreach (var item in us.getReward())
            {
                res.Add(new RewardModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description_
                });
            }

            return res;
        }

        public bool addReward(string title, string desc)
        {
            RewardAcc us = new RewardAcc();
            return us.addReward(title, desc);           
        }

        public bool delReward(int id)
        {
            RewardAcc us = new RewardAcc();
            return us.delReward(id);
        }
    }
}