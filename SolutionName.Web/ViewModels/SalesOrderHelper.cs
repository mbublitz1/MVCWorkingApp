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
                PONumber = salesOrder.PONumber,
                ObjectState = ObjectState.Unchanged
            };

            foreach (SalesOrderItem salesOrderItem in salesOrder.SalesOrderItems)
            {
                SalesOrderItemViewModel salesOrderItemViewModel = new SalesOrderItemViewModel
                {
                    SalesOrderItemId = salesOrderItem.SalesOrderItemId,
                    ProductCode = salesOrderItem.ProductCode,
                    Quantity = salesOrderItem.Quantity,
                    UnitPrice = salesOrderItem.UnitPrice,
                    ObjectState = salesOrderItem.ObjectState,
                    SalesOrderId = salesOrderItem.SalesOrderId

                };

                salesOrderViewModel.SalesOrderItems.Add(salesOrderItemViewModel);

            }

            return salesOrderViewModel;
        }

        public static SalesOrder CreateSalesOrderFromSalesOrderViewModel(SalesOrderViewModel salesOrderViewModel)
        {
            //Used to set the ID to -1 for child records since entity framework ignores negatives
            SalesOrder salesOrder = new SalesOrder
            {
                SalesOrderId = salesOrderViewModel.SalesOrderId,
                CustomerName = salesOrderViewModel.CustomerName,
                PONumber = salesOrderViewModel.PONumber,
                ObjectState = salesOrderViewModel.ObjectState
            };

            //Used to set the ID to -1 for child records since entity framework ignores negatives
            int temporarySalesOrderItemId = -1;

            foreach (SalesOrderItemViewModel salesOrderItemViewModel in salesOrderViewModel.SalesOrderItems)
            {
                SalesOrderItem salesOrderItem = new SalesOrderItem
                {
                    ProductCode = salesOrderItemViewModel.ProductCode,
                    Quantity = salesOrderItemViewModel.Quantity,
                    UnitPrice = salesOrderItemViewModel.UnitPrice,
                    ObjectState = salesOrderItemViewModel.ObjectState,
                };

                if (salesOrderItemViewModel.ObjectState != ObjectState.Added)
                {
                    salesOrderItem.SalesOrderItemId = salesOrderItemViewModel.SalesOrderItemId;
                }
                else
                {
                    salesOrderItem.SalesOrderItemId = temporarySalesOrderItemId;
                    temporarySalesOrderItemId--;
                }

                salesOrderItem.SalesOrderId = salesOrderViewModel.SalesOrderId;

                salesOrder.SalesOrderItems.Add(salesOrderItem);
            }

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