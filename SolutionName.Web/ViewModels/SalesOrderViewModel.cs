using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Server: You cannot create a saes order unless you supply the customer's name. ")]
        [StringLength(30, ErrorMessage = "Server: Customer names must be 30 characters or shorter. ")]
        public string CustomerName { get; set; }
        [StringLength(10, ErrorMessage = "Server: PO Numbers must be 10 characters or shorter.")]
        public string PONumber { get; set; }


        public string MessageToClient { get; set; }
        public ObjectState ObjectState { get; set; }

        public List<SalesOrderItemViewModel> SalesOrderItems { get; set; }

        //Used to track which items are deleted
        public List<int> SalesOrderItemsToDelete { get; set; }
    }
}