﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    // [Authorize(Roles = "Admin")]
    public class CategoryController : BaseController
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Supplier> _supplierRepository;


        //private readonly IRepository<Product> _productRepository;
        public CategoryController(IRepository<Category> categoryRepository, IRepository<Product> productRepository, IRepository<Supplier> supplierRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
        }

        public ActionResult List(int id)
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

        public IActionResult Detail(int id)
        {
            var category = _categoryRepository.Get(x => x.Id == id && x.IsActive, x => x.Include(y => y.Products));

            var vm = new CategoryViewModel()
            {
                Id = id,
                CategoryName = category.CategoryName,
                IsActive = category.IsActive,
                CategoryDescription = category.CategoryDescription,
                //Products = category.Products,
            };
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);  // bu server side mı client side mı?
            }

            #region üzerinde çalışılacak kod
            //bool result;

            //// aynı isimden kategori yoksa yeni ekle, aynı isimden varsa 
            //var existingEntity = _categoryRepository.Get(x => x.CategoryName == model.CategoryName);

            //Category entity = new Category()
            //{
            //    CategoryName = model.CategoryName,
            //    CategoryDescription = model.CategoryDescription
            //};

            //if (existingEntity == null)
            //{
            //    //add new entity

            //    result = _categoryRepository.Add(entity); // Add method return bool value.
            //}
            //else
            //{
            //    var id = existingEntity.Id;
            //    return RedirectToAction("Edit",id);
            //}

            //if (result)
            //{
            //    TempData["Message"] = "Ekleme başarılı.";
            //    return RedirectToAction("List");
            //}

            //TempData["Message"] = "Ekleme yapılamadı.";
            //return View("Add", model);
            #endregion

            var currentUserId = GetCurrentUserId();

            Category entity = new Category()
            {
                CategoryName = model.CategoryName,
                CategoryDescription = model.CategoryDescription,
            };

            bool result;

            entity.CreatedById = currentUserId;
            entity.CreatedDate = DateTime.Now;
            result = _categoryRepository.Add(entity);

            if (result)
            {
                return RedirectToAction("List");
            }

            TempData["Message"] = "Uh oh! Something went wrong...";
            return View("Add", model);
        }

        public ActionResult Edit(int id)
        {
            var category = _categoryRepository.Get(x => x.Id == id && x.IsActive == true);

            if (category != null)
            {
                var vm = new CategoryViewModel()
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                    CategoryDescription = category.CategoryDescription,  
                    IsActive = category.IsActive,
                };

                return View(vm);
            }

            TempData["Message"] = "Category cannot be found!";
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var currentUserId = GetCurrentUserId();

            Category entity = new Category()
            {
                Id = model.Id,
                CategoryName = model.CategoryName,
                CategoryDescription = model.CategoryDescription,
                IsActive = model.IsActive,
                UpdatedById = currentUserId,
                UpdatedDate = DateTime.Now
            };

            bool result;

            result = _categoryRepository.Edit(entity);

            if (result)
            {
                return RedirectToAction("List");
            }

            TempData["Message"] = "Uh oh! Something went wrong...";
            return View("Edit", model);
        }

        public ActionResult Delete(int id)
        {
            var result = _categoryRepository.Delete(id);

            TempData["Message"] = result ? "" : "Silme yapılamadı";

            return RedirectToAction("List");
        }


    }
}
