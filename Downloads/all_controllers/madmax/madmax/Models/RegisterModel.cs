using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace madmax.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ApartmentName { get; set; }
        public string Flatno { get; set; }
        public string Pincode { get; set; }
    }
}