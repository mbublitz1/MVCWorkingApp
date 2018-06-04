using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolutionName.Model;

namespace SolutionName.DataLayer
{
    public class SalesOrderItemConfiguration : EntityTypeConfiguration<SalesOrderItem>
    {
        public SalesOrderItemConfiguration()
        {
            Property(sol => sol.ProductCode).HasMaxLength(15).IsRequired();
            Property(sol => sol.Quantity).IsRequired();
            Property(sol => sol.UnitPrice).IsRequired();
            Ignore(sol => sol.ObjectState);
        }
    }
}
