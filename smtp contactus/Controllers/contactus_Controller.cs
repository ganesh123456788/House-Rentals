using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using smtp_contactus.Models;

namespace smtp_contactus.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(Class1 model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SendEmail(model);
                    TempData["Message"] = "Your message has been sent!";
                    return RedirectToAction("ContactUs");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "There was an error sending your message: " + ex.Message);
                }
            }

            return View(model);
        }

        private void SendEmail(Class1 model)
        {
            var fromAddress = new MailAddress("sriharshavaleti2@gmail.com");
            var toAddress = new MailAddress("sriharshavaleti3@gmail.com");
            const string subject = "Contact Us Form Submission";
            string body = $"Name: {model.Name}\nEmail: {model.Email}\nMessage: {model.Message}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // e.g., smtp.gmail.com for Gmail
                Port = 587, // or 465 for SSL
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("sriharshavaleti2@gmail.com", "zcvpzgonqklupgrw")
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
