using FarmCentralWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FarmCentralWebApp
{
    public static class Global
    {
        // static fields
        public static string currentUserRole;
        public static int currentUserId;
        public static int EmployeeUserId;
        public static List<SelectListItem> lstProductType = new List<SelectListItem>();
        public static List<SelectListItem> lstFarmerNames = new List<SelectListItem>();

        // httpClient object
        public static HttpClient httpClient = new HttpClient();

        // static class constructor
        static Global()
        {
            httpClient.BaseAddress = new Uri("http://localhost:8273/api/");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // list of application roles 
        public static List<SelectListItem> lstRoles = new List<SelectListItem>()
        {
            new SelectListItem {Text = "Employee", Value = "Employee" },
            new SelectListItem {Text = "Farmer" , Value = "Farmer"},
        };


        public static void populateProductType()
        {
            List<ProductType> product;
            HttpResponseMessage httpResponse = Global.httpClient.GetAsync("ProductTypes").Result;
            product = httpResponse.Content.ReadAsAsync<List<ProductType>>().Result;

            lstProductType.Clear();
            product.ForEach(x => lstProductType.Add(new SelectListItem { Text = x.ProductType1, Value = x.ProductType1 }));
        }

        public static void GetUserId(String email)
        {
            if (email != null)
            {
                List<User> user;
                HttpResponseMessage httpResponse = Global.httpClient.GetAsync("Users").Result;
                user = httpResponse.Content.ReadAsAsync<List<User>>().Result;

                currentUserId = user.Where(x => x.Email.ToLower().Trim().Equals(email.ToLower().Trim())).Select(x => x.UserId).FirstOrDefault();
            }
        }

        public static void GetFarmers()
        {
            List<User> user;
            HttpResponseMessage httpResponse = Global.httpClient.GetAsync("Users").Result;
            user = httpResponse.Content.ReadAsAsync<List<User>>().Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                var farmers = user.Where(x => x.Role.Equals("Farmer")).ToList();

                lstFarmerNames.Clear();
                farmers.ForEach(x => lstFarmerNames.Add(new SelectListItem { Text = x.Name, Value = x.Name }));
            }
        }

    }


}
