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
                            ViewBag.UserError = login.Email + " role is invalid. Please try again.";
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

        public ActionResult ResetPassword()
        {
            return View();
        }

        // POST: UsersController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword([Bind("Email,Password,ConfirmPassword")] ResetPassword reset)
        {

            List<User> user;
            HttpResponseMessage httpResponse = Global.httpClient.GetAsync("Users").Result;
            user = httpResponse.Content.ReadAsAsync<List<User>>().Result;

            // linq query to find the farmers password
            int id = user.Where(x => x.Email.ToLower().Equals(reset.Email.ToLower())).Select(x => x.UserId).FirstOrDefault();
            //string password = user.Where(x => x.Email.ToLower().Equals(reset.Email.ToLower())).Select(x => x.Password).FirstOrDefault();

            // update and hash the farmers password to new password
            user = user.Where(x => x.UserId == id).ToList();

            User updatedUser = new User();
            foreach (var x in user)
            {
                updatedUser.UserId = x.UserId;
                updatedUser.Name = x.Name;
                updatedUser.Surname = x.Surname;
                updatedUser.Email = x.Email;
                updatedUser.Role = x.Role;
                updatedUser.Password = BCrypt.Net.BCrypt.HashPassword(reset.Password);
            }

            httpResponse = Global.httpClient.PutAsJsonAsync(String.Format("Users/{0}", id), updatedUser).Result; //// TEST THIS !!

            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home"); // this will be the home nav page for the employee
            }
            else
            {
                ViewBag.UserUpdatePassword = "Failed to update password.";
                return View();
            }
        }
    }


}
