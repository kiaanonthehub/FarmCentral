using FarmCentralWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                Product product = new Product(); 


                HttpResponseMessage httpResponse = Global.httpClient.PostAsJsonAsync("Products", user).Result;
                HttpResponseMessage httpResponse = Global.httpClient.PostAsJsonAsync("ProductTypes", user).Result;
                HttpResponseMessage httpResponse = Global.httpClient.PostAsJsonAsync("UserProducts", user).Result;

                return RedirectToAction(nameof(Index));
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
