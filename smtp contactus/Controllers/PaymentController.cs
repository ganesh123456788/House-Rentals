using System.Web.Mvc;
using smtp_contactus.Models; // Ensure this is the correct namespace for your Payment class
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System;
using System.Threading.Tasks;

namespace smtp_contactus.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentDbContext _context;

        public PaymentController()
        {
            _context = new PaymentDbContext(); // Initialize the DbContext
        }

        // Show the payment form
        [HttpGet]
        public ActionResult PaymentForm()
        {
            return View(); // Returns the PaymentForm view
        }

        // Handle the form submission
        [HttpPost]
        public async Task<ActionResult> SubmitPayment(Payment model)
        {
            if (ModelState.IsValid)
            {
                // Calculate the total amount
                decimal totalAmount = model.TotalAmount;

                // Create an order with Razorpay
                string orderId = await CreateRazorpayOrder(totalAmount);

                // If the order was successfully created
                if (!string.IsNullOrEmpty(orderId))
                {
                    ViewBag.RazorpayKey = "rzp_test_wTbquqDjUOsQkb"; // Replace with your Razorpay key
                    ViewBag.OrderId = orderId; // The created order ID

                    // Prepare payment details to save in the database
                    var paymentDetails = new smtp_contactus.Models.Payment // Use the fully qualified name here
                    {
                        TenantName = model.TenantName,
                        ApartmentNumber = model.ApartmentNumber,
                        RentAmount = model.RentAmount,
                        MaintenanceCharges = model.MaintenanceCharges,
                        GasCharges = model.GasCharges,
                        WaterCharges = model.WaterCharges,
                        ElectricityCharges = model.ElectricityCharges,
                        TotalAmount = totalAmount // Store the total amount
                    };

                    // Store the payment details in the database
                    _context.Payments.Add(paymentDetails);
                    await _context.SaveChangesAsync(); // Save changes to the database

                    return View("RazorpayCheckout", model); // Return to a Razorpay checkout view
                }
                else
                {
                    ModelState.AddModelError("", "Error creating payment order. Please try again.");
                }
            }
            return View("PaymentForm", model); // Return to the form if the model is invalid
        }

        // Show the payment success page
        public ActionResult PaymentSuccess(string paymentId)
        {
            ViewBag.PaymentId = paymentId; // Show payment ID on success page
            return View(); // Return the PaymentSuccess view
        }

        // Method to create an order in Razorpay
        private async Task<string> CreateRazorpayOrder(decimal amount)
        {
            try
            {
                // Set up Razorpay API endpoint and authentication
                var client = new HttpClient();
                var url = "https://api.razorpay.com/v1/orders";

                // Replace these with your Razorpay credentials
                var razorpayKey = "rzp_test_wTbquqDjUOsQkb"; // Update with your Razorpay key
                var razorpaySecret = "brAcIDuUO6zU8Y0Iyn77r9VS"; // Update with your Razorpay secret

                // Create the order request payload
                var orderData = new
                {
                    amount = amount * 100, // Convert to paise
                    currency = "INR",
                    receipt = "receipt#1",
                    payment_capture = 1 // Automatically capture payment
                };

                var json = JsonConvert.SerializeObject(orderData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Set Basic Authentication Header
                var byteArray = Encoding.ASCII.GetBytes($"{razorpayKey}:{razorpaySecret}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Send POST request to create the order
                var response = await client.PostAsync(url, content);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseContent);
                    return result.id; // Return the order ID
                }
                else
                {
                    // Log the error response for debugging
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", "Error creating order: " + errorResponse);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur
                ModelState.AddModelError("", "An error occurred while creating the order: " + ex.Message);
            }
            return null; // Return null if order creation fails
        }
    }
}

