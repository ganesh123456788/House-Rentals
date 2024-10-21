using pavani1.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace pavani1.Controllers
{
    public class FlatDetailsController : Controller
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // Twilio credentials
        private string accountSid = "your_account_sid";   // Replace with your Twilio Account SID
        private string authToken = "your_auth_token";     // Replace with your Twilio Auth Token
        private string twilioPhoneNumber = "your_twilio_phone_number";  // Replace with your Twilio phone number

        // GET: FlatDetails
        public ActionResult Index()
        {
            List<FlatDetails> flatDetailsList = new List<FlatDetails>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT FlatNumber, Name, PhoneNumber, PaymentStatus FROM FlatDetails";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FlatDetails flat = new FlatDetails
                        {
                            FlatNumber = (int)reader["FlatNumber"],
                            Name = reader["Name"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            PaymentStatus = reader["PaymentStatus"].ToString()
                        };

                        flatDetailsList.Add(flat);
                    }
                }
            }

            return View(flatDetailsList);
        }

        // Action to send SMS for due payments
        public ActionResult SendSms(int flatNumber)
        {
            FlatDetails flat = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT FlatNumber, Name, PhoneNumber, PaymentStatus FROM FlatDetails WHERE FlatNumber = @FlatNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FlatNumber", flatNumber);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        flat = new FlatDetails
                        {
                            FlatNumber = (int)reader["FlatNumber"],
                            Name = reader["Name"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            PaymentStatus = reader["PaymentStatus"].ToString()
                        };
                    }
                }
            }

            if (flat != null && flat.PaymentStatus.ToLower() == "due")
            {
                SendDuePaymentSms(flat.Name, flat.PhoneNumber);
                ViewBag.Message = "SMS sent successfully!";
            }
            else
            {
                ViewBag.Message = "SMS not sent. Payment status is not 'Due'.";
            }

            return RedirectToAction("Index");
        }

        // Method to send SMS
        private void SendDuePaymentSms(string name, string phoneNumber)
        {
            TwilioClient.Init(accountSid, authToken);

            string messageBody = $"Dear {name}, your payment is due. Please make the payment at your earliest convenience.";

            var message = MessageResource.Create(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(twilioPhoneNumber),
                body: messageBody);

            // Optional: log the message SID for debugging
            System.Diagnostics.Debug.WriteLine($"Message sent to {phoneNumber}, SID: {message.Sid}");
        }
    }
}
