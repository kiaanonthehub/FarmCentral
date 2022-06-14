using FarmCentralWebApp.Models;
using FarmCentralWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace FarmCentralWebApp.Controllers
{
    public class ProductsController : Controller
    {
        // GET: ProductsController
        public ActionResult Index()
        {
            Global.populateProductType();
            ViewData["ProductType"] = Global.lstProductType;
            return View();
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/AddProduct
        public ActionResult AddProduct()
        {
            Global.populateProductType();
            ViewData["ProductType"] = Global.lstProductType;
            return View();
        }

        // POST: ProductsController/AddProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct([Bind("ProductName,Quantity,ProductType,ProductDate")] StoreProduct storeProduct)
        {
            try
            {
                List<Product> listProducts = new List<Product>();
                List<ProductType> listProductType = new List<ProductType>();

                Console.WriteLine(Global.currentUserId);
                // instantiate HttpResponseMessage object
                HttpResponseMessage httpResponse;

                // instantiate model objects
                Product product = new Product();
                product.ProductName = storeProduct.ProductName;
                httpResponse = Global.httpClient.PostAsJsonAsync("Products", product).Result; // PUT
                httpResponse = Global.httpClient.GetAsync("Products").Result; // GET
                listProducts = httpResponse.Content.ReadAsAsync<List<Product>>().Result; 
                product.ProductId = listProducts.Where(x=> x.ProductName == storeProduct.ProductName).Select(x=> x.ProductId).FirstOrDefault();

                // instantiate model objects
                ProductType productType = new ProductType();
                productType.ProductType1 = storeProduct.ProductType;
                httpResponse = Global.httpClient.PostAsJsonAsync("ProductTypes", productType).Result; // PUT
                httpResponse = Global.httpClient.GetAsync("ProductTypes").Result; // GET
                listProductType = httpResponse.Content.ReadAsAsync<List<ProductType>>().Result;
                productType.ProductTypeId = listProductType.Where(x => x.ProductType1 == storeProduct.ProductType).Select(x => x.ProductTypeId).FirstOrDefault();


                UsersProduct usersProduct = new UsersProduct();
                usersProduct.UserId = Global.currentUserId;
                usersProduct.ProductId = product.ProductId;
                usersProduct.ProductTypeId = productType.ProductTypeId;
                usersProduct.Quantity = storeProduct.Quantity;
                usersProduct.ProductType = storeProduct.ProductType;
                usersProduct.ProductDate = storeProduct.ProductDate;
                
                httpResponse = Global.httpClient.PostAsJsonAsync("UsersProducts", usersProduct).Result;

                if (httpResponse.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index", "Home");
                }
                else { return View(storeProduct); }
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
