using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolutionName.Model
{

    public partial class SalesOrder : IObjectWithState
    {
        public SalesOrder()
        {

            //Need to instantiate SalesOrderItems in the parent (SalesOrder) to
            //Ensure the collection of children is ready to use
            SalesOrderItems = new List<SalesOrderItem>();
        }
        public int SalesOrderId { get; set; }

        [Required]
        [StringLength(30)]
        public string CustomerName { get; set; }

        [StringLength(10)]
        public string PONumber { get; set; }
        [NotMapped]
        public ObjectState ObjectState { get; set; }

        //Virtual will allow lazy loading of data from EF, in real production applicaitons this should be turned off
        public virtual List<SalesOrderItem> SalesOrderItems { get; set; }
    }
}
