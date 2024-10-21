using System;
using System.Web.Mvc;
using madmax.Models;

namespace madmax.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Here, add your authentication logic.
                // For example, checking against a database for a valid user.
                if (IsValidUser(model.Email, model.Password))
                {
                    // If successful, redirect to another page, e.g., dashboard
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // If credentials are incorrect, show an error
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View(model);
        }

        private bool IsValidUser(string email, string password)
        {
            // Mock authentication logic; replace with actual database validation.
            return email == "test@example.com" && password == "password123";
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            // Perform logout logic, like clearing session or authentication cookies
            // Redirect to the login page
            return RedirectToAction("Login");
        }
    }
}
