using FarmCentralWebApp.Models;
using FarmCentralWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace FarmCentralWebApp.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: UsersController/Login
        public ActionResult Login()
        {
            ViewData["Roles"] = Global.lstRoles;
            Global.currentUserRole = null;
            return View();
        }


        // POST: UsersController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind("Email,Password,Role")] Login login)
        {

            List<User> user;
            HttpResponseMessage httpResponse = Global.httpClient.GetAsync("Users").Result;
            user = httpResponse.Content.ReadAsAsync<List<User>>().Result;

            var userEmail = user.Where(x => x.Email.ToLower().Equals(login.Email.ToLower())).FirstOrDefault();
            var userPassword = user.Where(x => x.Email.ToLower().Equals(login.Email.ToLower())).Select(x => x.Password).FirstOrDefault();
            var userRole = user.Where(x => x.Email.ToLower().Equals(login.Email.ToLower())).Select(x => x.Role).FirstOrDefault();

            if (userEmail == null)
            {
                ViewBag.UserError = "User Not Found. Please try again.";
                return RedirectToAction("Login", "Users");
            }
            else
            {
                try
                {
                    // verify if user password is the same as hashed password in the database
                    bool verify = BCrypt.Net.BCrypt.Verify(login.Password, userPassword);
                    if (verify)
                    {
                        if (userRole.Equals(login.Role))
                        {
                            Global.currentUserRole = userRole;
                            return RedirectToAction("Index", "Home"); // this will be the home nav page for the employee
                        }
                        else
                        {
                            ViewBag.UserError = login.Email+" role is invalid. Please try again.";
                            return View(login);
                        }
                    }
                    else
                    {
                        ViewBag.UserError = "This password is invalid. Please try again.";
                        return View(login);
                    }
                }
                catch (System.ArgumentNullException)
                {
                    ViewBag.UserError = "This password is invalid. Please try again.";
                }
                return View(login);
            }
        }

        // GET: UsersController/CreateAccountEmployee
        public ActionResult CreateAccount()
        {
            return View();
        }

        // POST: UsersController/CreateAccountEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount(User user)
        {
            try
            {
                user.Role = "Farmer";
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                HttpResponseMessage httpResponse = Global.httpClient.PostAsJsonAsync("Users", user).Result;

                return RedirectToAction("Create", "UsersProducts"); // redirect the employee to add a product or nav page???
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ResetFarmerPassword()
        {
            return View();
        }

        // POST: UsersController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetFarmerPassword([Bind("Email,Password")] ResetFarmerPassword reset)
        {

            List<User> user;
            HttpResponseMessage httpResponse = Global.httpClient.GetAsync("Users").Result;
            user = httpResponse.Content.ReadAsAsync<List<User>>().Result;

            // linq query to find the farmers password
            var filter = user.Where(x => x.Email.ToLower().Equals(reset.Email.ToLower())).Select(x => new { x.UserId, x.Password }).FirstOrDefault();

            // update and hash the farmers password to new password
            httpResponse = Global.httpClient.PutAsJsonAsync(String.Format("Users/{0}", filter.UserId), reset).Result; //// TEST THIS !!


            return RedirectToAction("Index", "Home"); // this will be the home nav page for the employee
        }
    }


}
