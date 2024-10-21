using System;
using System.ComponentModel.DataAnnotations;

namespace pavani1.Models
{
    public class HouseRental
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "House number is required")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "House rent is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid rent amount")]
        public decimal HouseRent { get; set; }

        public DateTime Date { get; set; }
    }
}
