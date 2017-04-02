using _3.BLL.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3.BLL.Models
{
    public class ListUsers
    {
        public List<UserModel> Users { get; set; }
    }

    public class UserModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Date of BDay")]
        public DateTime? BDay { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "FilePath")]
        public string FilePath { get; set; }

    }

  
}