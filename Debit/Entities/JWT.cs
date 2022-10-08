using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Debit.Entities
{
    public class JWT
    {
        public string AccessToken { get; set; }
        #nullable enable
        public string? RefreshToken { get; set; }
        #nullable enable
        //public JWT()
        //{

        //}
        public JWT(string AccessToken, string? RefreshToken)
        {
            this.AccessToken = AccessToken;
           
            this.RefreshToken = RefreshToken;
            
        }
    }
}
