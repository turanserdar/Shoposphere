using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shoposphere.Admin.Models;
using Shoposphere.Data.Entities;
using Shoposphere.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoposphere.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IRepository<Order> _orderRepository;
       
        private readonly IRepository<Product> _productRepository;

        public OrderController(IRepository<Order> orderRepository,  IRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            
            _productRepository = productRepository;
        }

        public IActionResult List()
        {

         var orders= _orderRepository.GetAll().Select(x =>
              new OrderViewModel()
              {
                  Id=x.Id,
                  UserId=x.UserId,
                  ShipperId=x.ShipperId,
                  Comments=x.Comments,
                  Freight=x.Freight,
                  RequiredDate=x.RequiredDate,
                  ShipAddress=x.ShipAddress,
                  ShippedDate=x.ShippedDate,
                  Shipper=x.Shipper,
                  User=x.User,
                  IsActive=x.IsActive
                  
                  
              });


            return View(orders);
        }

        public IActionResult Detail(int id)
        {
            //var orderDetail = _orderRepository.Get(x => x.Id == id && x.IsActive,
            //   x => x.Include(y => y.OrderDetails)
            //   .ThenInclude(y => y.Product));

            var orderDetail = _orderRepository.Get(x => x.Id == id /*&& x.IsActive*/, x => x.Include(y => y.User).Include(y => y.OrderDetails).ThenInclude(y => y.Product).ThenInclude(y=>y.Supplier));

            var orderDetailVM = new OrderViewModel()
            {
                Id = orderDetail.Id,
                Comments = orderDetail.Comments,
                Freight = orderDetail.Freight,
                IsActive = orderDetail.IsActive,
                RequiredDate = orderDetail.RequiredDate,
                ShipAddress = orderDetail.ShipAddress,
                ShippedDate = orderDetail.ShippedDate,
                Shipper = orderDetail.Shipper,
                ShipperId = orderDetail.ShipperId,
                User = orderDetail.User,
                UserId = orderDetail.UserId,



            };

            var vm = new OrderDetailViewModel()
            {
                OrderDetail = orderDetailVM,
                Products = new ProductViewModel()
            };

            return View(vm);
        }



        public ActionResult Edit(int id)
        {


        

            var orders = _orderRepository.Get(x => x.Id == id);
            //ViewBag.orders = _orderRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
            //{
            //    Text = x.IsActive.ToString(),
            //    Value = x.Id.ToString(),
            //}).ToList();


            if (orders != null)
            {
                var vm = new OrderViewModel()
                {
                    Id=orders.Id,
                    Freight=orders.Freight,
                    IsActive=orders.IsActive ,
                    RequiredDate=orders.RequiredDate,
                    ShipAddress=orders.ShipAddress,
                    Shipper=orders.Shipper,
                    ShippedDate=orders.ShippedDate,
                    ShipperId=orders.ShipperId,
                    UserId=orders.UserId


                };

                return View(vm);
            }

            TempData["Message"] = "Order cannot be found!";
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //ViewBag.orders = _orderRepository.GetAll(x => x.IsActive).Select(x => new SelectListItem()
                //{
                //    Text = x.IsActive.ToString(),
                //    Value = x.Id.ToString(),
                //}).ToList();


                return View("Edit", model);
            }
            


            var currentUserId = GetCurrentUserId();

            Order entity = new Order()
            {
                Id = model.Id,

                UserId = model.UserId,
                ShipperId = model.ShipperId,
                Comments = model.Comments,
                Freight = model.Freight,
                RequiredDate = model.RequiredDate,
                ShipAddress = model.ShipAddress,
                ShippedDate = model.ShippedDate,
                Shipper = model.Shipper,
                User = model.User,
                IsActive=model.IsActive,
                
                


            };

            bool result;

            result = _orderRepository.Edit(entity);

            if (result)
            {
                return RedirectToAction("List");
            }

            TempData["Message"] = "Uh oh! Something went wrong...";
            return View("Edit", model);
        }


    }
}
