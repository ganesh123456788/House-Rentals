using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using madmax.Models;

namespace madmax.Controllers
{
    public class AdminRegistrationController : Controller
    {
        // Connection string from Web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: AdminRegistration
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminRegistration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminRegistration/Create
        [HttpPost]
        public ActionResult Create(AdminRegistrationModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Save data to the database using SqlConnection and SqlCommand
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();

                        string query = "INSERT INTO AdminRegistration (FirstName, LastName, PhoneNumber, Email, Flatno, AdharNumber, JoiningDate, ApartmentName, Details) " +
                                       "VALUES (@FirstName, @LastName, @PhoneNumber, @Email, @Flatno, @AdharNumber, @JoiningDate, @ApartmentName, @Details)";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                            cmd.Parameters.AddWithValue("@LastName", model.LastName);
                            cmd.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                            cmd.Parameters.AddWithValue("@Email", model.Email);
                            cmd.Parameters.AddWithValue("@Flatno", model.Flatno);
                            cmd.Parameters.AddWithValue("@AdharNumber", model.AdharNumber);
                            cmd.Parameters.AddWithValue("@JoiningDate", model.JoiningDate);
                            cmd.Parameters.AddWithValue("@ApartmentName", model.ApartmentName);
                            cmd.Parameters.AddWithValue("@Details", model.Details);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    TempData["SuccessMessage"] = "Admin registered successfully!";
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ModelState.AddModelError("", "Error while registering admin: " + ex.Message);
                return View(model);
            }
        }
    }
}
