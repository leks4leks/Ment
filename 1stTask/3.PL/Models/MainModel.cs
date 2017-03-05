using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3.PL.Models
{
    public class MainModel
    {
        public List<UserModel> Users { get; set; }

        public UserModel AddedUser { get; set; }
    }
}