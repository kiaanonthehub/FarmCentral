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
            Initialise();
            ViewData["Roles"] = Global.lstRoles;
            Global.currentUserRole = null;
            return View();
        }


        // POST: UsersController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind("Email,Password,Role")] Login login)
        {
            // initialise view bag
            ViewData["Roles"] = Global.lstRoles;

            // instantiate view model object
            List<User> user;

            // instantiate HttpResponseMessage obj and make GET api request
            HttpResponseMessage httpResponse = Global.httpClient.GetAsync("Users").Result;
            user = httpResponse.Content.ReadAsAsync<List<User>>().Result;

            // use linq to filter properties needed
            var userEmail = user.Where(x => x.Email.ToLower().Equals(login.Email.ToLower())).FirstOrDefault();
            var userPassword = user.Where(x => x.Email.ToLower().Equals(login.Email.ToLower())).Select(x => x.Password).FirstOrDefault();
            var userRole = user.Where(x => x.Email.ToLower().Equals(login.Email.ToLower())).Select(x => x.Role).FirstOrDefault();
            var userId = user.Where(x => x.Email.ToLower().Equals(login.Email.ToLower())).Select(x => x.UserId).FirstOrDefault();

            // check if the email is null
            if (userEmail == null)
            {
                // populate the viewbag
                ViewBag.UserError = "User Not Found. Please try again.";
                return View(login);
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
                            // initialise static fields
                            Global.GetUserId(userEmail.Email);
                            Global.currentUserRole = userRole;
                            Global.currentFullname = user.Where(x => x.UserId == Global.currentUserId).Select(x => x.Name).FirstOrDefault().ToString() + " " + user.Where(x => x.UserId == Global.currentUserId).Select(x => x.Surname).FirstOrDefault().ToString();
                            if (userRole.Equals("Employee"))
                            {
                                return RedirectToAction("Index", "Home"); 
                            }
                            else if (userRole.Equals("Farmer")) { return RedirectToAction("ResetPassword", "Users"); }
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
                // initiase view model instance
                user.Role = "Farmer";
                
                // hash password
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                                
                // POST api request made 
                HttpResponseMessage httpResponse = Global.httpClient.PostAsJsonAsync("Users", user).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    // redirect the employee to home page
                    return RedirectToAction("Index", "Home"); 
                }
                else { return View(user); }
            }
            catch
            {
                return View();
            }
        }

        // GET : UsersController/ResetPassword 
        public ActionResult ResetPassword()
        {
            return View();
        }

        // POST: UsersController/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword([Bind("Email,Password,ConfirmPassword")] ResetPassword reset)
        {

            List<User> user;
            HttpResponseMessage httpResponse = Global.httpClient.GetAsync("Users").Result;
            user = httpResponse.Content.ReadAsAsync<List<User>>().Result;

            String currentEmail = user.Where(x => x.UserId == Global.currentUserId).Select(x => x.Email).FirstOrDefault();

            // linq query to find the farmers password
            int id = user.Where(x => x.Email.ToLower().Equals(currentEmail.ToLower())).Select(x => x.UserId).FirstOrDefault();
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

            httpResponse = Global.httpClient.PutAsJsonAsync(String.Format("Users/{0}", id), updatedUser).Result; 

            if (httpResponse.IsSuccessStatusCode)
            {
                // this will be the home nav page for the employee
                return RedirectToAction("ViewHistory", "Products"); 
            }
            else
            {
                ViewBag.UserUpdatePassword = "Failed to update password.";
                return View();
            }
        }

        // method to initialise all static fields
        public void Initialise()
        {
            Global.currentUserRole = "";
            Global.currentUserId = 0;
            Global.currentFullname = "";
            Global.currentEmail = "";
            Global.viewFarmerFullname = "";
            Global.EmployeeUserId = 0;
        }
    }
}
