using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using RegistrationApp.Models;
using smtp_contactus.Models;
namespace RegistrationApp.Controllers
{
    public class adminController : Controller
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // GET: admin/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        // POST: admin/Register
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var command = new SqlCommand("INSERT INTO Usersd (FirstName, LastName, PhoneNumber, Email, FlatNumber, JoiningDate, AadhaarNumber, Password) VALUES (@FirstName, @LastName, @PhoneNumber, @Email, @FlatNumber, @JoiningDate, @AadhaarNumber, @Password)", connection);
                        command.Parameters.AddWithValue("@FirstName", user.FirstName);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@FlatNumber", user.FlatNumber);
                        command.Parameters.AddWithValue("@JoiningDate", user.JoiningDate);
                        command.Parameters.AddWithValue("@AadhaarNumber", user.AadhaarNumber);
                        command.Parameters.AddWithValue("@Password", user.Password); // Consider hashing the password before storing it.
                        command.ExecuteNonQuery();
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error while registering user: " + ex.Message);
                }
            }
            return View(user);
        }
    }
}