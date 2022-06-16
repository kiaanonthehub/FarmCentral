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

        public IActionResult ViewHistory()
        {
            HttpResponseMessage httpResponse;
            List<UsersProduct> usersProduct;
            List<Product> product;
            List<ViewHistory> viewHistories = new List<ViewHistory>();
            ViewHistory vh;

            // GET
            httpResponse = Global.httpClient.GetAsync("UsersProducts").Result;
            usersProduct = httpResponse.Content.ReadAsAsync<List<UsersProduct>>().Result;
            //GET
            httpResponse = Global.httpClient.GetAsync("Products").Result;
            product = httpResponse.Content.ReadAsAsync<List<Product>>().Result;

            // https://stackoverflow.com/questions/6253656/how-do-i-join-two-lists-using-linq-or-lambda-expressions
            var joinTables = usersProduct.Join(product, x => x.ProductId, y => y.ProductId,
                (x, y) => new
                { y.ProductName, x.Quantity, x.ProductType, x.ProductDate, x.UserId, x.UsersProductId }).ToList();

            //foreach (var i in joinTables)
            //{
            //    vh.ProductName = i.ProductName;
            //    vh.Quantity = i.Quantity;
            //    vh.ProductType = i.ProductType;
            //    vh.ProductDate = i.ProductDate;
            //    viewHistories.Add(vh);
            //}
            viewHistories.Clear();
            joinTables.ForEach(i =>
            {
                vh = new ViewHistory(i.UsersProductId, i.UserId, i.ProductName, i.Quantity, i.ProductType, i.ProductDate);
                viewHistories.Add(vh);
            });

            // filter the list only for the current user to see his/her details
            return View(viewHistories.Where(x => x.UserId.Equals(Global.currentUserId)).ToList());
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult AddProductType()
        {
            return View();
        }

        // POST: Products/AddProductType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductType([Bind("ProductType1")] StoreProductType storeProductType)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage httpResponse;
                // api call to get the existing database list of productTypes
                List<ProductType> productTypes;
                httpResponse = Global.httpClient.GetAsync("ProductTypes").Result;
                productTypes = httpResponse.Content.ReadAsAsync<List<ProductType>>().Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    // filter to check if product type exists
                    var filterProdcutTypes = productTypes.Where(x => x.ProductType1.Equals(storeProductType)).ToList().FirstOrDefault();

                    if (filterProdcutTypes == null)
                    {
                        // add product to the database with api post
                        httpResponse = Global.httpClient.PostAsJsonAsync("ProductTypes", storeProductType).Result;
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            return RedirectToAction("AddProduct", "Products");
                        }
                        else
                        {
                            ViewBag.ProductError = "Failed to add " + storeProductType.ProductType1;
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.ProductError = storeProductType.ProductType1 + " already exists. Please add a different type.";
                        return View();
                    }
                }
            }

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
                product.ProductId = listProducts.Where(x => x.ProductName == storeProduct.ProductName).Select(x => x.ProductId).FirstOrDefault();

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
            Global.editId = id;
            Global.populateProductType();
            ViewData["ProductType"] = Global.lstProductType;

            HttpResponseMessage httpResponse = Global.httpClient.GetAsync(String.Format("UsersProducts/{0}", id)).Result;
            return View(httpResponse.Content.ReadAsAsync<ViewHistory>().Result);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewHistory viewHistory)
        {

            Global.populateProductType();
            ViewData["ProductType"] = Global.lstProductType;

            List<UsersProduct> lstUsersProducts;
            HttpResponseMessage httpResponse = Global.httpClient.GetAsync(String.Format("UsersProducts")).Result;
            lstUsersProducts = httpResponse.Content.ReadAsAsync<List<UsersProduct>>().Result;

            List<UsersProduct> filter = lstUsersProducts.Where(x => x.UsersProductId == Global.editId).ToList();

            if (filter != null)
            {
                UsersProduct usersProduct = new UsersProduct();

                foreach (var i in filter)
                {
                    usersProduct.UsersProductId = i.UsersProductId;
                    usersProduct.ProductId = i.ProductId;
                    usersProduct.ProductTypeId = i.ProductTypeId;
                    usersProduct.UserId = i.UserId;

                    // updated
                    usersProduct.Quantity = viewHistory.Quantity;
                    usersProduct.ProductType = viewHistory.ProductType;
                    usersProduct.ProductDate = viewHistory.ProductDate;
                }

                httpResponse = Global.httpClient.PutAsJsonAsync(String.Format("UsersProducts/{0}", usersProduct.UsersProductId), usersProduct).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("FarmerProductHistory", "Products");
                }
                else { return RedirectToAction("Index", "Home"); }
            }
            else { return RedirectToAction("Index", "Home"); }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            Global.deleteId = id;
            HttpResponseMessage httpResponse = Global.httpClient.DeleteAsync(String.Format("UsersProducts/{0}", id)).Result;
            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("FarmerProductHistory", "Products");
            }
            else
            {
                return RedirectToAction("Idnex", "Home");
            }
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

        // GET: Products/FarmerProductHistory
        public IActionResult FarmerProductHistory()
        {
            HttpResponseMessage httpResponse;
            List<UsersProduct> usersProduct;
            List<Product> product;
            List<ViewHistory> viewHistories = new List<ViewHistory>();
            ViewHistory vh;

            // GET
            httpResponse = Global.httpClient.GetAsync("UsersProducts").Result;
            usersProduct = httpResponse.Content.ReadAsAsync<List<UsersProduct>>().Result;
            //GET
            httpResponse = Global.httpClient.GetAsync("Products").Result;
            product = httpResponse.Content.ReadAsAsync<List<Product>>().Result;

            // https://stackoverflow.com/questions/6253656/how-do-i-join-two-lists-using-linq-or-lambda-expressions
            var joinTables = usersProduct.Join(product, x => x.ProductId, y => y.ProductId,
                (x, y) => new
                { y.ProductName, x.Quantity, x.ProductType, x.ProductDate, x.UserId, x.UsersProductId }).ToList();


            viewHistories.Clear();
            joinTables.ForEach(i =>
            {
                vh = new ViewHistory(i.UsersProductId, i.UserId, i.ProductName, i.Quantity, i.ProductType, i.ProductDate);
                viewHistories.Add(vh);
            });

            // filter the list only for the current user to see his/her details
            return View(viewHistories.Where(x => x.UserId.Equals(Global.EmployeeUserId)).ToList());
        }

        // GET: Products/SelectFarmer
        public IActionResult SelectFarmer()
        {
            Global.GetFarmers();
            ViewData["farmerName"] = Global.lstFarmerNames;
            return View();
        }

        // POST: Products/SelectFarmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectFarmer([Bind("Name")] FilterByEmployee filterByEmployee)
        {
            Global.GetFarmers();
            ViewData["FarmerName"] = Global.lstFarmerNames;
            if (ModelState.IsValid)
            {
                if (filterByEmployee != null)
                {
                    List<User> user;
                    HttpResponseMessage httpResponse = Global.httpClient.GetAsync("Users").Result;
                    user = httpResponse.Content.ReadAsAsync<List<User>>().Result;

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        try
                        {
                            Global.EmployeeUserId = user.Where(x => x.Name.Equals(filterByEmployee.Name)).Select(x => x.UserId).First();
                            Global.viewFarmerFullname = filterByEmployee.Name.ToString();
                            return RedirectToAction("FarmerProductHistory", "Products");
                        }
                        catch (System.InvalidOperationException e)
                        {
                            throw e;
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            return View();
        }


        // GET: Products/FilterByDate
        public ActionResult FilterByDate()
        {
            return View();
        }

        // POST: Products/FilterByDate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FilterByDate([Bind("StartDate, EndDate")] FilterByDate filterByDate)
        {
            if (ModelState.IsValid)
            {
                Global.filterStartDate = filterByDate.StartDate;
                Global.filterEndDate = filterByDate.EndDate;

                return RedirectToAction("ViewDateFilter", "Products");
            }
            return View();
        }

        // GET: Products/FilterByType
        public ActionResult FilterByType()
        {
            Global.populateProductType();
            ViewData["ProductType"] = Global.lstProductType;
            return View();
        }

        // POST: Products/FilterByType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FilterByType([Bind("ProductType")] FilterByType filterByType)
        {
            if (ModelState.IsValid)
            {
                Global.filterType = filterByType.ProductType;

                return RedirectToAction("ViewTypeFilter", "Products");
            }
            return View();
        }

        // GET: Products/ViewDateFilter
        public ActionResult ViewDateFilter()
        {
            HttpResponseMessage httpResponse;
            List<UsersProduct> usersProduct;
            List<Product> product;
            List<ViewHistory> viewHistories = new List<ViewHistory>();
            ViewHistory vh;

            // GET
            httpResponse = Global.httpClient.GetAsync("UsersProducts").Result;
            usersProduct = httpResponse.Content.ReadAsAsync<List<UsersProduct>>().Result;
            //GET
            httpResponse = Global.httpClient.GetAsync("Products").Result;
            product = httpResponse.Content.ReadAsAsync<List<Product>>().Result;

            // https://stackoverflow.com/questions/6253656/how-do-i-join-two-lists-using-linq-or-lambda-expressions
            var joinTables = usersProduct.Join(product, x => x.ProductId, y => y.ProductId,
                (x, y) => new
                { y.ProductName, x.Quantity, x.ProductType, x.ProductDate, x.UserId, x.UsersProductId }).ToList();


            viewHistories.Clear();
            joinTables.ForEach(i =>
            {
                vh = new ViewHistory(i.UsersProductId, i.UserId, i.ProductName, i.Quantity, i.ProductType, i.ProductDate);
                viewHistories.Add(vh);
            });

            // filter the list only for the current user to see his/her details
            return View(viewHistories.Where(x => x.UserId.Equals(Global.EmployeeUserId) && (x.ProductDate >= Global.filterStartDate && x.ProductDate <= Global.filterEndDate)).ToList());
        }

        // GET: Products/FilterByType
        public IActionResult ViewTypeFilter()
        {
            HttpResponseMessage httpResponse;
            List<UsersProduct> usersProduct;
            List<Product> product;
            List<ViewHistory> viewHistories = new List<ViewHistory>();
            ViewHistory vh;

            // GET
            httpResponse = Global.httpClient.GetAsync("UsersProducts").Result;
            usersProduct = httpResponse.Content.ReadAsAsync<List<UsersProduct>>().Result;
            //GET
            httpResponse = Global.httpClient.GetAsync("Products").Result;
            product = httpResponse.Content.ReadAsAsync<List<Product>>().Result;

            // https://stackoverflow.com/questions/6253656/how-do-i-join-two-lists-using-linq-or-lambda-expressions
            var joinTables = usersProduct.Join(product, x => x.ProductId, y => y.ProductId,
                (x, y) => new
                { y.ProductName, x.Quantity, x.ProductType, x.ProductDate, x.UserId, x.UsersProductId }).ToList();


            viewHistories.Clear();
            joinTables.ForEach(i =>
            {
                vh = new ViewHistory(i.UsersProductId, i.UserId, i.ProductName, i.Quantity, i.ProductType, i.ProductDate);
                viewHistories.Add(vh);
            });

            // filter the list only for the current user to see his/her details
            return View(viewHistories.Where(x => x.UserId.Equals(Global.EmployeeUserId) && x.ProductType.Equals(Global.filterType)).ToList());
        }
    }
}
