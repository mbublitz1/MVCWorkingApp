using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolutionName.Model
{

    public partial class SalesOrder
    {
        public int SalesOrderId { get; set; }

        [Required]
        [StringLength(30)]
        public string CustomerName { get; set; }

        [StringLength(10)]
        public string PONumber { get; set; }
    }
}
