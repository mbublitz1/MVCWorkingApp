using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SolutionName.Model;

namespace SolutionName.DataLayer
{
    public partial class SalesContext : DbContext
    {
        public SalesContext()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<SalesOrder> SalesOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
