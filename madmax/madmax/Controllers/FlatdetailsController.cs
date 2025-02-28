﻿using madmax.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace madmax.Controllers
{
    public class FlatdetailsController : Controller
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // Twilio credentials
        private string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
        private string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
        private string twilioPhoneNumber = Environment.GetEnvironmentVariable("TWILIO_PHONE_NUMBER");

        // GET: FlatDetails
        public ActionResult Index()
        {
            List<Flatdetails_model> flatDetailsList = new List<Flatdetails_model>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT FlatNumber, Name, PhoneNumber, PaymentStatus FROM FlatDetails";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Flatdetails_model flat = new Flatdetails_model
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
            Flatdetails_model flat = null;

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
                        flat = new Flatdetails_model
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

            System.Diagnostics.Debug.WriteLine($"Message sent to {phoneNumber}, SID: {message.Sid}");
        }
    }
}
