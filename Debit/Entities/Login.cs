using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Debit.Entities
{
    public class Login
    {
        [Required(ErrorMessage = "The email is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The password is required")]
        public string Password { get; set; }
        //public Login()
        //{

        //}
        public Login(string Email, string Password)
        {
            this.UserName = Email;
            this.Password = Password;
        }
    }
}
