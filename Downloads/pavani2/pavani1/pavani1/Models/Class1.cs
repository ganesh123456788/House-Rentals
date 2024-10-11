using System.Collections.Generic;
using System.Data.Entity;

namespace pavani1.Models
{
    public class HouseContext : DbContext
    {
        public DbSet<houselist> Houselists { get; set; }

        public HouseContext() : base("name=HouseConnectionString")
        {
        }
    }
}
