using Movies.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.ViewModels
{
    public class AddUserToRoleViewModel
    {
        /*        public AddUserToRoleViewModel()
                {
                    Users = new List<ApplicationUser>();
                }*/
        public string Id { get; set; }
        [Required]
        public string Username { get; set; }
/*        public string UserId { get; set; }

        public List<ApplicationUser> Users { get; set; }
        public bool IsSelected { get; set; }*/
    }
}
