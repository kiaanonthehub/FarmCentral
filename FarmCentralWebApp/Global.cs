using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FarmCentralWebApp
{
    public static class Global
    {
        public static string currentUserRole;

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
    }
}
