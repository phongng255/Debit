using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Debit.Entities
{
    public class Login
    {
        [Required(ErrorMessage = "The Phone is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The password is required")]
        public string Password { get; set; }
        //public Login()
        //{

        //}
        public Login(string PhoneNumber, string Password)
        {
            this.PhoneNumber = PhoneNumber;
            this.Password = Password;
        }
    }
}
