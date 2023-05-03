using DbHandler.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancePortal.DTO
{
    public class DTO
    {
      
    }
    public class BaseClass
    {
        public string IP { get; set; }

        [Required(ErrorMessage = "dto-0001")]
        public string DeviceID { get; set; }

        [Required(ErrorMessage = "err-0002")]
        public string OnBoardingChannel { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Platform { get; set; }
        public string Browser { get; set; }




    }
    public class LoginDTO
    {
        [Required]
      public string name { get; set; }
        [Required]
        public string password { get; set; }

        public BaseClass BaseClass { get; set; }
    
    }
}
