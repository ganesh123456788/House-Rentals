using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using RegistrationApp.Models;

namespace RegistrationApp.Controllers
{
    public class RegistrationPageController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["SpicesDBConnectionString"].ConnectionString;

        public ActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Email == "existing@example.com") // Simulate email check
                {
                    ModelState.AddModelError("", "Email already exists.");
                }
                else
                {
                    // Registration logic
                    return RedirectToAction("Login", "Auth");
                }
            }
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new Registrationform());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Registrationform registrationform)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO DryFruits (FirstName, LastName, Phoneno, Email, Password, ConfirmPassword) VALUES (@FirstName, @LastName, @Phoneno, @Email, @Password, @ConfirmPassword)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", registrationform.FirstName);
                        command.Parameters.AddWithValue("@LastName", registrationform.LastName);
                        command.Parameters.AddWithValue("@Phoneno", registrationform.Phoneno);
                        command.Parameters.AddWithValue("@Email", registrationform.Email);
                        command.Parameters.AddWithValue("@Password", registrationform.Password);
                        command.Parameters.AddWithValue("@ConfirmPassword", registrationform.ConfirmPassword);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index","Home");
            }
            return View(registrationform);
        }
    }
}
