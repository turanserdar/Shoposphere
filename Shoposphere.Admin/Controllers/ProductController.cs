using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shoposphere.Admin.Models;
using Shoposphere.Data.Entities;
using Shoposphere.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shoposphere.Admin.Controllers
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
            var products = _productRepository.GetAll(include: x => x.Include(y => y.Category).Include(y => y.Supplier))
                .Where(x => x.IsActive); // isactive getall'ın içine alınacak

            if (id != null)
            {
                products = _productRepository.GetAll(x => x.CategoryId == id);
            }

            var vm = products.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                ReorderLevel = x.ReorderLevel,
                Discontinued = x.Discontinued,
                //CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                //SupplierId = x.SupplierId,
                SupplierName = x.Supplier.SupplierName,
                PictureStr = Convert.ToBase64String(x.Picture)

            }).ToList();

            return View(vm);
        }

        //public IActionResult Detail(int id)
        //{
        //    var product = _productRepository.Get(x => x.Id == id)
        //    return View();
        //}

        public IActionResult Add()
        {
            ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString(),
            }).ToList();

          

            ViewBag.Suppliers = _supplierRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
            {
                Text = x.SupplierName,
                Value = x.Id.ToString()
                
            }).ToList();

            

            //ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive)
            //        .Select(x => new SelectListItem()
            //        {
            //            Text = x.CategoryName,
            //            Value = x.Id.ToString()
            //        })
            //        .ToList();

            //ViewBag.Categories = _supplierRepository.GetAll(x => x.IsActive)
            //        .Select(x => new SelectListItem()
            //        {
            //            Text = x.SupplierName,
            //            Value = x.Id.ToString()
            //        })
            //        .ToList();

            return View("Add");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString(),
                }).ToList();

                ViewBag.Suppliers = _supplierRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
                {
                    Text = x.SupplierName,
                    Value = x.Id.ToString()

                }).ToList();

                return View(model);
            }

            ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString(),
            }).ToList();

            ViewBag.Suppliers = _supplierRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
            {
                Text = x.SupplierName,
                Value = x.Id.ToString()

            }).ToList();

            Product entity = new Product()
            {
                ProductName = model.ProductName,
                UnitPrice = model.UnitPrice,
                UnitsInStock = model.UnitsInStock,
                Discontinued = model.Discontinued,
                ReorderLevel = model.ReorderLevel,
                CreatedDate = DateTime.Now,
                CategoryId = model.CategoryId,
                SupplierId = model.SupplierId,
                IsActive = true,
            };


            #region Picture için düzenleme

            if (model.Picture.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    model.Picture.CopyTo(ms);
                    var fileByteArray = ms.ToArray();
                    entity.Picture = fileByteArray;
                }
            }
            else
            {
                TempData["Message"] = "This Field is required.";
            }

            #endregion

            bool result;
            int currentUserId = GetCurrentUserId();

            entity.CreatedById = currentUserId;
            entity.CreatedDate = DateTime.Now;
            result = _productRepository.Add(entity);

            if (result)
            {
                return RedirectToAction("List");
            }

            return View("Add", model);
        }

        public IActionResult Edit(int id)
        {
            var product = _productRepository.Get(x => x.Id == id && x.IsActive, include: x => x.Include(y => y.Category).Include(y => y.Supplier));

            if (product != null)
            {
                var vm = new ProductViewModel()
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    SupplierName = product.Supplier.SupplierName,
                    CategoryName = product.Category.CategoryName,
                    Isactive = product.IsActive,
                    UnitsInStock = product.UnitsInStock,
                    UnitPrice = product.UnitPrice,
                    ReorderLevel = product.ReorderLevel,
                    Discontinued = product.Discontinued,
                };
                ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString(),
                }).ToList();

                ViewBag.Suppliers = _supplierRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
                {
                    Text = x.SupplierName,
                    Value = x.Id.ToString()

                }).ToList();
                return View(vm);
            }

            TempData["Message"] = "Product cannot be found!";
            return View("List");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString(),
                }).ToList();

                ViewBag.Suppliers = _supplierRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
                {
                    Text = x.SupplierName,
                    Value = x.Id.ToString()

                }).ToList();
                return View(model);
            }

            ViewBag.Categories = _categoryRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString(),
            }).ToList();

            ViewBag.Suppliers = _supplierRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
            {
                Text = x.SupplierName,
                Value = x.Id.ToString()

            }).ToList();

            int currentUserId = GetCurrentUserId();
            var entity = new Product()
            {
                Id = model.Id,
                Discontinued = model.Discontinued,
                ReorderLevel = model.ReorderLevel,
                CategoryId = model.CategoryId,
                SupplierId = model.SupplierId,
                ProductName = model.ProductName,
                UnitsInStock = model.UnitsInStock,
                UnitPrice = model.UnitPrice,
                UpdatedById = currentUserId,
                UpdatedDate = DateTime.Now,
                IsActive = model.Isactive,

            };


            #region Picture için düzenleme

            if (model.Picture.Length > 0) // lenght = 0 ise dosyanın içi boştur
            {
                using (var ms = new MemoryStream())
                {
                    model.Picture.CopyTo(ms);
                    var fileByteArray = ms.ToArray();

                    entity.Picture = fileByteArray;
                }
            }
            else
            {
                ViewBag.Message = "Boş dosya yükleyemezsiniz";
            }

            #endregion


            bool result;


            entity.Id = model.Id;
            entity.UpdatedById = currentUserId;
            result = _productRepository.Edit(entity);

            if (result)
            {
                return RedirectToAction("List");
            }

            TempData["Message"] = "Uh oh! Something went wrong...";
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var result = _productRepository.Delete(id);

            TempData["Message"] = result ? "" : "Silme yapılamadı.";
            return RedirectToAction("List");
        }
    }
}
