using Shoposphere.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shoposphere.UI.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public decimal Freight { get; set; }

       

        [Required]
        [StringLength(60)]
        public string ShipAddress { get; set; }

        public bool IsActive { get; set; }

        #region Relations

        public int UserId { get; set; }
        public User User { get; set; }

        public int ShipperId { get; set; }
        public Shipper Shipper { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
        public List<Comment> Comments { get; set; }

        #endregion


    }
}
