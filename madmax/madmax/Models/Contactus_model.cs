using System.ComponentModel.DataAnnotations;

namespace madmax.Models
{
    public class Contactus_model
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
