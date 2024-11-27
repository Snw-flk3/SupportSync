using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using HopeWorldWide.Models;
using Microsoft.Extensions.Logging;

namespace HopeWorldWide.Controllers
{
    public class AddEmployeeController : Controller
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly ILogger<AddEmployeeController> _logger;

        public AddEmployeeController(FirestoreDb firestoreDb, ILogger<AddEmployeeController> logger)
        {
            _firestoreDb = firestoreDb ?? throw new ArgumentNullException(nameof(firestoreDb));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: AddEmployee
        public IActionResult Index()
        {
            return View();
        }

        // POST: AddEmployee
        [HttpPost]
        public async Task<IActionResult> Index(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Hash the password and generate a salt
                var (hashedPassword, salt) = HashPassword(model.Password);

                // Create a new User object
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmployeeId = model.EmployeeId,
                    Email = model.Email,
                    Password = hashedPassword,
                    Salt = salt, // Ensure the generated salt is assigned
                    Role = model.Role
                };

                _logger.LogDebug($"User object before saving: {System.Text.Json.JsonSerializer.Serialize(user)}");

                // Add user to Firestore
                var userRef = _firestoreDb.Collection("users").Document();
                await userRef.SetAsync(user);

                // Set success message and redirect
                TempData["Message"] = "Employee added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the error and display a user-friendly message
                _logger.LogError(ex, "Failed to add employee.");
                ModelState.AddModelError("", "Failed to add employee. Please try again.");
                return View(model);
            }
        }

        // Generates a hashed password and a salt
        private (string HashedPassword, string Salt) HashPassword(string password)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 100000);
            byte[] hash = rfc2898DeriveBytes.GetBytes(32);
            string hashedPassword = Convert.ToBase64String(hash);

            return (hashedPassword, salt);
        }
    }
}
