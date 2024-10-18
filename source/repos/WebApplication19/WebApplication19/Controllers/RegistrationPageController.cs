using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication19.Models;

namespace WebApplication19.Controllers
{
    public class RegistrationPageController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Generate OTP
                    string otp = GenerateOtp();
                    // Store the OTP and user information temporarily (e.g., in session)
                    Session["OTP"] = otp;
                    Session["User"] = user;
                    // Send OTP to the user's email
                    await SendOtpEmailAsync(user.Email, otp);
                    TempData["Message"] = "OTP has been sent to your email. Please enter it to complete registration.";
                    return RedirectToAction("VerifyOTP");  // Redirect to the OTP verification page
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
            }
            return View(user);
        }
        [HttpGet]
        public ActionResult VerifyOTP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyOTP(string enteredOtp)
        {
            string generatedOtp = Session["OTP"] as string;
            RegisterModel user = Session["User"] as RegisterModel;
            if (enteredOtp == generatedOtp)
            {
                try
                {
                    // OTP is valid, complete the registration by saving the user
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    SaveUserAsync(user).Wait(); // Save the user
                    TempData["Message"] = "Registration successful!";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid OTP. Please try again.");
            }
            return View();
        }
        private string GenerateOtp()
        {
            // Generate a 6-digit OTP
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        private async Task SendOtpEmailAsync(string email, string otp)
        {
            var fromAddress = new MailAddress("tejalavu96@gmail.com", "tejalavu");
            var toAddress = new MailAddress(email);
            const string fromPassword = "tmguzuomjonxoxtb";
            const string subject = "OTP for Registration";
            string body = $"Your OTP is: {otp}";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }
        private async Task SaveUserAsync(RegisterModel user)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpicesDBConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO UserRegistrationDBd
                                (FirstName, LastName, Email, Password, DateOfBirth, Gender, ApartmentName, Flatno, Pincode)
                                VALUES (@FirstName, @LastName, @Email, @Password, @DateOfBirth, @Gender, @ApartmentName, @Flatno, @Pincode)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", user.Gender);
                    command.Parameters.AddWithValue("@ApartmentName", user.ApartmentName);
                    command.Parameters.AddWithValue("@Flatno", user.Flatno);
                    command.Parameters.AddWithValue("@Pincode", user.Pincode);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            // Placeholder for actual login logic
            TempData["Message"] = "Login successful!";
            return RedirectToAction("Index", "Home");
        }
    }
}