using Shoposphere.Data.Entities;
using Shoposphere.Services.Interfaces;
using Shoposphere.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoposphere.UI.Controllers
{
    //[Authorize]
    public class CartController : BaseController
    {

        private readonly IRepository<Product> _productRepository;
        public CartController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        const string SessionShopCart = "";
        public IActionResult Add(int id)
        {
            var product = _productRepository.Get(x => x.IsActive && x.Id == id);
            var sessionCart = HttpContext.Session.GetString("SessionShopCart");

            if (product != null)
            {
                if (sessionCart == null)
                {
                    var cartItemList = new List<CartItemViewModel>();
                    var cartItem = new CartItemViewModel()
                    {
                        Product = product,
                        Quantity = 1,
                        PictureStr = Convert.ToBase64String(product.Picture),
                    };

                    cartItemList.Add(cartItem);

                    HttpContext.Session.SetString("SessionShopCart", JsonConvert.SerializeObject(cartItemList));
                }
                else
                {
                    var cartItemList = JsonConvert.DeserializeObject<List<CartItemViewModel>>(HttpContext.Session.GetString("SessionShopCart"));


                    if (cartItemList.Any(x => x.Product.Id == id))
                    {
                        var currentProduct = cartItemList.FirstOrDefault(x => x.Product.Id == id);
                        currentProduct.Quantity += 1;
                    }
                    else
                    {
                        var cartItem = new CartItemViewModel()
                        {
                            Product = product,
                            Quantity = 1
                        };

                        cartItemList.Add(cartItem);
                    }
                    HttpContext.Session.SetString("SessionShopCart", JsonConvert.SerializeObject(cartItemList));
                }
            }

            return RedirectToAction("List");

        }
        public IActionResult List()
        {
            var cartItemList = new List<CartItemViewModel>();
            var sessionCart = HttpContext.Session.GetString("SessionShopCart");

            if (sessionCart != null)
            {
                cartItemList = JsonConvert.DeserializeObject<List<CartItemViewModel>>(sessionCart);
            }

            TempData["Login"] = "Please login to see your cart";
            return View(cartItemList);
        }

        public IActionResult Delete(int id)
        {
            var cartItemList = new List<CartItemViewModel>();

            var sessionCart = HttpContext.Session.GetString("SessionShopCart");

            if (sessionCart != null)
            {
                cartItemList = JsonConvert.DeserializeObject<List<CartItemViewModel>>(sessionCart);
            }

            var currentProduct = cartItemList.FirstOrDefault(x => x.Product.Id == id);
            if (currentProduct != null)
            {
                currentProduct.Quantity -= 1;
                if (currentProduct.Quantity == 0)
                {
                    cartItemList.Remove(currentProduct);
                }
                HttpContext.Session.SetString("SessionShopCart", JsonConvert.SerializeObject(cartItemList));
            }
            return RedirectToAction("List");
        }

    }
}
