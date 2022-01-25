﻿using Microsoft.AspNetCore.Http;
using Shoposphere.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shoposphere.UI.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(500)]
        public string CategoryDescription { get; set; }
        public IFormFile Picture { get; set; }
        public string PictureStr { get; set; }

        public bool IsActive { get; set; }

        //public List<Product> Products { get; set; }
    }
}
