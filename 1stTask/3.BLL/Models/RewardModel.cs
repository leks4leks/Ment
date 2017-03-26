using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3.BLL.Models
{
    public class ListReward
    {
        public List<RewardModel> Rewards { get; set; }
    }
    public class RewardModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description ")]
        public string Description { get; set; }
        
        
    }
}