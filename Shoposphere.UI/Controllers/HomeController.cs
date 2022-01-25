using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoposphere.Data.Entities;
using Shoposphere.Services.Interfaces;
using Shoposphere.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoposphere.UI.Controllers
{
    public class HomeController : Controller
    
        {
            private readonly IRepository<Category> _categoryRepository;
            private readonly IRepository<Product> _productRepository;
            private readonly IRepository<Supplier> _supplierRepository;
            public HomeController(IRepository<Category> categoryRepository, IRepository<Product> productRepository, IRepository<Supplier> supplierRepository)
            {
                _categoryRepository = categoryRepository;
                _productRepository = productRepository;
                _supplierRepository = supplierRepository;
            }

            public IActionResult Index()
            {

                var indexViewModel = new IndexViewModel();

                var categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new CategoryViewModel()
                {
                    CategoryName = x.CategoryName,
                    CategoryDescription = x.CategoryDescription,
                    Id = x.Id,
                    PictureStr = Convert.ToBase64String(x.Picture)

                }).ToList();


                var products = _productRepository.GetAll(include: x => x.Include(y => y.Category).Include(y => y.Supplier)).Select(x =>
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

            //public IActionResult Privacy()
            //{
            //    return View();
            //}

            ////[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            ////public IActionResult Error()
            ////{
            ////    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            ////}
            //public IActionResult AboutUs() // unutma
            //{
            //    return View();
            //}
            //public IActionResult ContactUs()
            //{
            //    return View();
            //}
        }
    }

