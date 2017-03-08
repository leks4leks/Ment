using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3.BLL.Models;

namespace _3.PL.Models
{
    public class MainModel
    {
        public List<UserModel> Users { get; set; }

        public UserModel AddedUser { get; set; }

        public List<RewardModel> Rewards { get; set; }

        public RewardModel AddedReward { get; set; }

    }
}