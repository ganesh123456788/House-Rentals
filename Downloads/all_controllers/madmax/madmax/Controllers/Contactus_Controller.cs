using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using madmax.Models;

namespace madmax.Controllers
{
    public class Contactus_Controller : Controller
    {
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

        private void SendEmail(Contactus_model model)
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