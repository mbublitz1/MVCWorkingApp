using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using SolutionName.Model;

namespace SolutionName.Web.ViewModels
{
    public class SalesOrderItemViewModel : IObjectWithState
    {
        public int SalesOrderItemId { get; set; }
        [Required(ErrorMessage = "Server: You canno create a sales order item unless you supply the product code.")]
        [StringLength(15, ErrorMessage = "Server: Product codes must be 15 characters or shorter")]
        [RegularExpression(@"^A-za-z+$", ErrorMessage = "Server: Product codes consist of letters only.")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "Server: You cannot create a sales order item unless you supply the quantity")]
        [Range(0, 100000, ErrorMessage = "Server: Quantity must be between 1 and 1,000,000")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Server: Unit price is a required field")]
        [Range(0, 100000, ErrorMessage = "Server: Unit price must be between 0 and 100,000")]
        public decimal UnitPrice { get; set; }

        public int SalesOrderId { get; set; }

        public ObjectState ObjectState { get; set; }
    }
}