using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using madmax.Models;

namespace madmax.Controllers
{
    public class Contactus_Controller : Controller
    {
        private readonly string _connectionString = "Data Source=DESKTOP-5M6SBGL;Initial Catalog=harsha;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(Contactus_model model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SaveMessageToDatabase(model);
                    SendEmail(model);
                    TempData["Message"] = "Your message has been sent!";
                    return RedirectToAction("ContactUs");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "There was an error sending your message.");
                }
            }

            return View(model);
        }

        private void SaveMessageToDatabase(Contactus_model model)
        {
            var query = "INSERT INTO ContactUsMessages (Name, Email, Message) VALUES (@Name, @Email, @Message)";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", model.Name);
                command.Parameters.AddWithValue("@Email", model.Email);
                command.Parameters.AddWithValue("@Message", model.Message);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void SendEmail(Contactus_model model)
        {
            var fromAddress = new MailAddress("your-email@gmail.com");
            var toAddress = new MailAddress("recipient-email@gmail.com");
            const string subject = "Contact Us Form Submission";
            string body = $"Name: {model.Name}\nEmail: {model.Email}\nMessage: {model.Message}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    Environment.GetEnvironmentVariable("SMTP_USER"),
                    Environment.GetEnvironmentVariable("SMTP_PASS"))
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
