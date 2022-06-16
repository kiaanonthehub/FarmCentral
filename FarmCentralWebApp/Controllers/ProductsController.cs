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
            // populate the dropdowns with the product types
            Global.populateProductType();
            ViewData["ProductType"] = Global.lstProductType;
            return View();
        }

        public IActionResult ViewHistory()
        {
            // instantiate HttpResponseMessage object to connect to the api
            HttpResponseMessage httpResponse;

            // instantiate generic collections
            List<UsersProduct> usersProduct;
            List<Product> product;
            List<ViewHistory> viewHistories = new List<ViewHistory>();

            // instantiate ViewHistory object
            ViewHistory vh;

            // GET request to the api to the UsersProducts controller
            httpResponse = Global.httpClient.GetAsync("UsersProducts").Result;
            usersProduct = httpResponse.Content.ReadAsAsync<List<UsersProduct>>().Result;

            //GET request to the api products controller
            httpResponse = Global.httpClient.GetAsync("Products").Result;
            product = httpResponse.Content.ReadAsAsync<List<Product>>().Result;

            // join the lists of both tables using LINQ
            var joinTables = usersProduct.Join(product, x => x.ProductId, y => y.ProductId,
                (x, y) => new
                { y.ProductName, x.Quantity, x.ProductType, x.ProductDate, x.UserId, x.UsersProductId }).ToList();

            // clear list
            viewHistories.Clear();

            // populate the list with linq foreach
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
                // instantiate HttpResponseMessage object
                HttpResponseMessage httpResponse;

                // api call to get the existing database list of productTypes and store to the list
                List<ProductType> productTypes;
                httpResponse = Global.httpClient.GetAsync("ProductTypes").Result;
                productTypes = httpResponse.Content.ReadAsAsync<List<ProductType>>().Result;


                // check if the api call is successful
                if (httpResponse.IsSuccessStatusCode)
                {
                    // filter to check if product type exists
                    var filterProdcutTypes = productTypes.Where(x => x.ProductType1.Equals(storeProductType)).ToList().FirstOrDefault();

                    // check if the list is empty
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
            // method call to populate the dropdown of product types
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
                // instantite generic list collections
                List<Product> listProducts = new List<Product>();
                List<ProductType> listProductType = new List<ProductType>();

                // instantiate HttpResponseMessage object
                HttpResponseMessage httpResponse;

                // instantiate model objects
                Product product = new Product();
                product.ProductName = storeProduct.ProductName;
                httpResponse = Global.httpClient.PostAsJsonAsync("Products", product).Result; // PUT
                httpResponse = Global.httpClient.GetAsync("Products").Result; // GET
                listProducts = httpResponse.Content.ReadAsAsync<List<Product>>().Result;
                product.ProductId = listProducts.Where(x => x.ProductName == storeProduct.ProductName).Select(x => x.ProductId).FirstOrDefault();

                // instantiate model objects and make api PUT and GET calls
                ProductType productType = new ProductType();
                productType.ProductType1 = storeProduct.ProductType;

                // PUT call request
                httpResponse = Global.httpClient.PostAsJsonAsync("ProductTypes", productType).Result;

                // GET call request
                httpResponse = Global.httpClient.GetAsync("ProductTypes").Result;
                listProductType = httpResponse.Content.ReadAsAsync<List<ProductType>>().Result;
                productType.ProductTypeId = listProductType.Where(x => x.ProductType1 == storeProduct.ProductType).Select(x => x.ProductTypeId).FirstOrDefault();

                // instantiate UsersProduct objects and initialise the fields
                UsersProduct usersProduct = new UsersProduct();
                usersProduct.UserId = Global.currentUserId;
                usersProduct.ProductId = product.ProductId;
                usersProduct.ProductTypeId = productType.ProductTypeId;
                usersProduct.Quantity = storeProduct.Quantity;
                usersProduct.ProductType = storeProduct.ProductType;
                usersProduct.ProductDate = storeProduct.ProductDate;

                // POST call request to write 
                httpResponse = Global.httpClient.PostAsJsonAsync("UsersProducts", usersProduct).Result;

                // check if the api request was successful
                if (httpResponse.IsSuccessStatusCode)
                { return RedirectToAction("ViewHistory", "Products"); }
                else { return View(storeProduct); }
            }
            catch
            { return View(); }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            // set static fields values
            Global.editId = id;
            Global.populateProductType();
            ViewData["ProductType"] = Global.lstProductType;

            // GET api call request
            HttpResponseMessage httpResponse = Global.httpClient.GetAsync(String.Format("UsersProducts/{0}", id)).Result;
            return View(httpResponse.Content.ReadAsAsync<ViewHistory>().Result);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewHistory viewHistory)
        {

            // method call to populate the dropdown product types to be edited
            Global.populateProductType();
            ViewData["ProductType"] = Global.lstProductType;

            // instantiate generic collection 
            List<UsersProduct> lstUsersProducts;
            HttpResponseMessage httpResponse = Global.httpClient.GetAsync(String.Format("UsersProducts")).Result;
            lstUsersProducts = httpResponse.Content.ReadAsAsync<List<UsersProduct>>().Result;

            // filter the list to get edited items
            List<UsersProduct> filter = lstUsersProducts.Where(x => x.UsersProductId == Global.editId).ToList();

            // check if the list is empty
            if (filter != null)
            {
                // instantiate model
                UsersProduct usersProduct = new UsersProduct();

                // iterate through the list
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

                // PUT api request to write to the database
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
            // get the id of the item to be deleted
            Global.deleteId = id;

            // DELETE api request call
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
            // instantiate HttpResponseMessage object
            HttpResponseMessage httpResponse;

            // instantiate generic collection of type view models
            List<UsersProduct> usersProduct;
            List<Product> product;
            List<ViewHistory> viewHistories = new List<ViewHistory>();
            ViewHistory vh;

            // GET api request
            httpResponse = Global.httpClient.GetAsync("UsersProducts").Result;
            usersProduct = httpResponse.Content.ReadAsAsync<List<UsersProduct>>().Result;
            //GET api request
            httpResponse = Global.httpClient.GetAsync("Products").Result;
            product = httpResponse.Content.ReadAsAsync<List<Product>>().Result;

            // join lists from the request made to the database
            var joinTables = usersProduct.Join(product, x => x.ProductId, y => y.ProductId,
                (x, y) => new
                { y.ProductName, x.Quantity, x.ProductType, x.ProductDate, x.UserId, x.UsersProductId }).ToList();

            // clear list
            viewHistories.Clear();
            
            // filter the joined tables list
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
            // method to the the farmers names from the database
            Global.GetFarmers();
            ViewData["farmerName"] = Global.lstFarmerNames;
            return View();
        }

        // POST: Products/SelectFarmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectFarmer([Bind("Name")] FilterByEmployee filterByEmployee)
        {
            // method call to the static class to get the list of farmers names from the database
            Global.GetFarmers();
            ViewData["FarmerName"] = Global.lstFarmerNames;
            if (ModelState.IsValid)
            {
                // check id the viewmodel obj is not empty
                if (filterByEmployee != null)
                {
                    // instantiate generic list 
                    List<User> user;

                    // GET api request
                    HttpResponseMessage httpResponse = Global.httpClient.GetAsync("Users").Result;
                    user = httpResponse.Content.ReadAsAsync<List<User>>().Result;

                    // check if the api request is successful
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        try
                        {
                            // filter the list to get the farmer user id and full name
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
                // get the users start and end date from the static class populated by the GET request
                Global.filterStartDate = filterByDate.StartDate;
                Global.filterEndDate = filterByDate.EndDate;

                return RedirectToAction("ViewDateFilter", "Products");
            }
            return View();
        }

        // GET: Products/FilterByType
        public ActionResult FilterByType()
        {
            // method call to populate the drop down with the product types 
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
                // initialise the product type selected by the user from the view model
                Global.filterType = filterByType.ProductType;

                return RedirectToAction("ViewTypeFilter", "Products");
            }
            return View();
        }

        // GET: Products/ViewDateFilter
        public ActionResult ViewDateFilter()
        {

            // instantiate HttpResponseMessage object
            HttpResponseMessage httpResponse;

            // instantiate generic collections of type view models
            List<UsersProduct> usersProduct;
            List<Product> product;
            List<ViewHistory> viewHistories = new List<ViewHistory>();
            ViewHistory vh;

            // GET api request
            httpResponse = Global.httpClient.GetAsync("UsersProducts").Result;
            usersProduct = httpResponse.Content.ReadAsAsync<List<UsersProduct>>().Result;
            //GET api request
            httpResponse = Global.httpClient.GetAsync("Products").Result;
            product = httpResponse.Content.ReadAsAsync<List<Product>>().Result;

            // join the lists from api request made from the database
            var joinTables = usersProduct.Join(product, x => x.ProductId, y => y.ProductId,
                (x, y) => new
                { y.ProductName, x.Quantity, x.ProductType, x.ProductDate, x.UserId, x.UsersProductId }).ToList();

            // clear the list
            viewHistories.Clear();

            // filter the list with linq
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
            // instantiate HttpResponseMessage object
            HttpResponseMessage httpResponse;

            // instantiate generic collections of type view models
            List<UsersProduct> usersProduct;
            List<Product> product;
            List<ViewHistory> viewHistories = new List<ViewHistory>();
            ViewHistory vh;

            // GET api request
            httpResponse = Global.httpClient.GetAsync("UsersProducts").Result;
            usersProduct = httpResponse.Content.ReadAsAsync<List<UsersProduct>>().Result;
            //GET api request
            httpResponse = Global.httpClient.GetAsync("Products").Result;
            product = httpResponse.Content.ReadAsAsync<List<Product>>().Result;

            // join the lists from api request made from the database
            var joinTables = usersProduct.Join(product, x => x.ProductId, y => y.ProductId,
                (x, y) => new
                { y.ProductName, x.Quantity, x.ProductType, x.ProductDate, x.UserId, x.UsersProductId }).ToList();

            // clear the list
            viewHistories.Clear();

            // filter the list with linq
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

/*
 * Code Attribution
 * Author : NerdFury
 * Subject : how do I join two lists using linq or lambda expressions
 * Link :  // https://stackoverflow.com/questions/6253656/how-do-i-join-two-lists-using-linq-or-lambda-expressions [answered Jun 6, 2011 at 14:47]
 * Date Accessed : 14-06-2022
 */