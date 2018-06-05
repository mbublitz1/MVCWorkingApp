using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SolutionName.Model;

namespace SolutionName.Web.ViewModels
{
    public class SalesOrderViewModel : IObjectWithState
    {
        public SalesOrderViewModel()
        {
            SalesOrderItems = new List<SalesOrderItemViewModel>();
            SalesOrderItemsToDelete = new List<int>();
        }

        public int SalesOrderId { get; set; }

        public string CustomerName { get; set; }

        public string PONumber { get; set; }


        public string MessageToClient { get; set; }
        public ObjectState ObjectState { get; set; }

        public List<SalesOrderItemViewModel> SalesOrderItems { get; set; }

        //Used to track which items are deleted
        public List<int> SalesOrderItemsToDelete { get; set; }
    }
}