using Shoposphere.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shoposphere.Admin.Models
{
    public class OrderDetailViewModel
    {

        public int OrderID { get; set; }
        public Order Order { get; set; }

        //[Key]
        //[Column(Order = 1)]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public short Quantity { get; set; } //smallint

        public float Discount { get; set; } //real

        public OrderViewModel OrderDetail { get; set; }

        public ProductViewModel Products { get; set; }



    }
}
