using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shoposphere.UI.Models;
using Shoposphere.Data.Entities;
using Shoposphere.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoposphere.UI.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        public ProductController(IRepository<Product> productRepository, IRepository<Category> categoryRepository, IRepository<Supplier> supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
        }

        public IActionResult List(int? id)
        {

            var indexViewModel = new IndexViewModel();

            var categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new CategoryViewModel()
            {
                CategoryName = x.CategoryName,
               CategoryDescription=x.CategoryDescription,
                Id = x.Id,
                PictureStr = Convert.ToBase64String(x.Picture)

            }).ToList();


            var products = _productRepository.GetAll(include: x => x.Include(y => y.Category).Include(y => y.Supplier)).Where(x => x.CategoryId == id).Select(x =>
                new ProductViewModel()
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice,
                    UnitsInStock = x.UnitsInStock,
                    ReorderLevel = x.ReorderLevel,
                    Discontinued = x.Discontinued,
                    CategoryId = x.Category.Id,
                    SupplierId = x.SupplierId,

                    PictureStr = Convert.ToBase64String(x.Picture)
                }).ToList();


            indexViewModel.Categories = categories;
            indexViewModel.Products = products;
            return View(indexViewModel);


        }
        public IActionResult ProductDetail(int id)
        {

            var indexViewModel = new IndexViewModel();

            var categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new CategoryViewModel()
            {
                CategoryName = x.CategoryName,
                CategoryDescription = x.CategoryDescription,
                Id = x.Id,
                PictureStr = Convert.ToBase64String(x.Picture)

            }).ToList();


            var products = _productRepository.GetAll(include: x => x.Include(y => y.Category).Include(y => y.Supplier)).Where(x => x.Id == id).Select(x =>
                 new ProductViewModel()
                 {
                     Id = x.Id,
                     ProductName = x.ProductName,
                     UnitPrice = x.UnitPrice,
                     UnitsInStock = x.UnitsInStock,
                     ReorderLevel = x.ReorderLevel,
                     Discontinued = x.Discontinued,
                     CategoryId = x.Category.Id,
                     SupplierId = x.SupplierId,
                     SupplierName = x.Supplier.SupplierName,

                     PictureStr = Convert.ToBase64String(x.Picture)
                 }).ToList();

            indexViewModel.Categories = categories;
            indexViewModel.Products = products;
            return View(indexViewModel);





        }
    }
}
