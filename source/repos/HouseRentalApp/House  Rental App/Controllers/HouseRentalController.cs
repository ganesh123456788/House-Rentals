using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using House__Rental_App.Models;

namespace House__Rental_App.Controllers
{
    public class HouseRentalController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["HouseRentalDB"].ConnectionString;

        public ActionResult Index()
        {
            var rentals = GetAllRentals();
            return View(rentals);
        }

        private IEnumerable<HouseRental> GetAllRentals()
        {
            var rentals = new List<HouseRental>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM HouseRentals";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rental = new HouseRental
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            HouseNumber = reader["HouseNumber"].ToString(),
                            HouseRent = (decimal)reader["HouseRent"],
                            Date = (DateTime)reader["Date"]
                        };
                        rentals.Add(rental);
                    }
                }
            }
            return rentals;
        }

        // GET: HouseRental/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HouseRental/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HouseRental rental)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO HouseRentals (Name, HouseNumber, HouseRent, Date) VALUES (@Name, @HouseNumber, @HouseRent, @Date)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", rental.Name);
                    command.Parameters.AddWithValue("@HouseNumber", rental.HouseNumber);
                    command.Parameters.AddWithValue("@HouseRent", rental.HouseRent);
                    command.Parameters.AddWithValue("@Date", DateTime.Now); // Current date
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            return View(rental);
        }
    }
}
