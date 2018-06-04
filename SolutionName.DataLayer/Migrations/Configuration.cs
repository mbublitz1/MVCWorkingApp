using SolutionName.Model;

namespace SolutionName.DataLayer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SolutionName.DataLayer.SalesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SolutionName.DataLayer.SalesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.SalesOrders.AddOrUpdate(
                so => so.CustomerName,
                new SalesOrder
                {
                    CustomerName = "Adam", PONumber = "9876", SalesOrderItems =
                    {
                        new SalesOrderItem { ProductCode = "ABC123", Quantity = 10, UnitPrice = 1.23m },
                        new SalesOrderItem { ProductCode = "XYZ987", Quantity = 7, UnitPrice = 14.57m },
                        new SalesOrderItem { ProductCode = "SAMPLE", Quantity = 3, UnitPrice = 15.00m },
                    }
                },
                new SalesOrder { CustomerName = "Michael" },
                new SalesOrder { CustomerName = "David", PONumber = "Acme 9" }
            );
        }
    }
}
