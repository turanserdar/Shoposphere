using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoposphere.Data.Entities;

namespace Shoposphere.UI.Models
{
    public class CartItemViewModel
    {

        public Product Product { get; set; }
        public short Quantity { get; set; }
        public decimal SubTotalPrice
        {
            get
            {
                return this.Product.UnitPrice * this.Quantity;
            }
        }
        public string PictureStr { get; set; }



    }
}
