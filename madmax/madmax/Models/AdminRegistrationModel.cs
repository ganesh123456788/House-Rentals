using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace madmax.Models
{
    public class AdminRegistrationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Flatno { get; set; }
        public string AdharNumber { get; set; }
        public DateTime JoiningDate { get; set; }
        public string ApartmentName { get; set; }
        public string Details { get; set; }
        
    }
}