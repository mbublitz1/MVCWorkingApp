using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SolutionName.Model;

namespace SolutionName.Web.ViewModels
{
    public static class SalesOrderHelper
    {

        public static SalesOrderViewModel CreateSalesOrderViewModelFromSalesOrder(SalesOrder salesOrder)
        {
            SalesOrderViewModel salesOrderViewModel = new SalesOrderViewModel
            {
                SalesOrderId = salesOrder.SalesOrderId,
                CustomerName = salesOrder.CustomerName,
                PONumber = salesOrder.PONumber
            };

            return salesOrderViewModel;
        }

        public static SalesOrder CreateSalesOrderFromSalesOrderViewModel(SalesOrderViewModel salesOrderViewModel)
        {
            SalesOrder salesOrder = new SalesOrder
            {
                SalesOrderId = salesOrderViewModel.SalesOrderId,
                CustomerName = salesOrderViewModel.CustomerName,
                PONumber = salesOrderViewModel.PONumber
            };

            return salesOrder;
        }

        public static string GetMessageToClient(ObjectState objectState, string customerName)
        {
            string messageToClient = string.Empty;
            switch (objectState)
            {
                case ObjectState.Added:
                    messageToClient = string.Format("A sales order for {0} has been added to the database.", customerName);
                    break;
                case ObjectState.Modified:
                    messageToClient = string.Format("The customer name for this sals order has been updated to {0} in the database", customerName);
                    break;

            }

            return messageToClient;
        }
    }
}