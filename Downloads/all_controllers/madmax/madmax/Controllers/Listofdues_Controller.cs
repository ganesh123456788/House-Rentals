using madmax.Models;
using System;
using System.Collections.Generic;
using System.Configuration; // For accessing configuration settings
using System.Data.SqlClient; // For SQL Server data access
using System.Web.Mvc;

namespace madmax.Controllers
{
    public class Listofdues_Controller : Controller
    {
        // Connection string retrieved from the configuration file (e.g., web.config)
        private string connectionString = ConfigurationManager.ConnectionStrings["HouseRentalDB"].ConnectionString;

        // GET: List of Dues
        public ActionResult ListofduesIndex()
        {
            var houses = GetHouselists(); // Fetch data from the database
            return View(houses);
        }

        // Method to retrieve house lists from the database
        private List<Listofdues_model> GetHouselists()
        {
            var houses = new List<Listofdues_model>();

            // Using ADO.NET to interact with the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM TenantsPayments"; // SQL query to fetch data
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open(); // Open the connection

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Creating a HouseModel instance for each row
                        var house = new Listofdues_model
                        {
                            Id = (int)reader["Id"], // Assuming there's an Id column
                            FlatNumber = reader["Name"].ToString(),
                            FlatType = reader["HouseNumber"].ToString(),
                            Price = (decimal)reader["HouseRent"]
                            // Add other properties as needed
                        };

                        houses.Add(house); // Add the house to the list
                    }
                }
            }

            return houses; // Return the list of houses
        }
    }
}