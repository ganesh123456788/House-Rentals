using System.Collections.Generic;
using System.Data.Entity;

namespace smtp_contactus.Models
{
	public class PaymentDbContext : DbContext
	{
		public PaymentDbContext() : base("name=DefaultConnection") { }

		public DbSet<Payment> Payments { get; set; } // Maps to your payments table
	}
}