using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using HopeWorldWide.Models;
using Microsoft.Extensions.Logging;

namespace HopeWorldWide.Controllers
{
    public class LoginController : Controller
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly ILogger<LoginController> _logger;

        public LoginController(FirestoreDb firestoreDb, ILogger<LoginController> logger)
        {
            _firestoreDb = firestoreDb ?? throw new ArgumentNullException(nameof(firestoreDb));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Please enter both email and password.");
                return View();
            }

            try
            {
                // Retrieve the user based on the email
                var userRef = _firestoreDb.Collection("users").WhereEqualTo("Email", email);
                var userSnapshot = await userRef.GetSnapshotAsync();

                if (userSnapshot.Count == 0)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View();
                }

                // Get the user document
                var userDoc = userSnapshot.Documents[0];
                var user = userDoc.ConvertTo<User>();

                _logger.LogDebug($"Retrieved user: {user.Email}");

                string storedSalt = user.Salt;
                string storedHashedPassword = user.Password;

                // Verify the entered password
                if (!VerifyPassword(password, storedHashedPassword, storedSalt))
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View();
                }

                // Check the role
                if (user.Role != "Admin")
                {
                    ModelState.AddModelError("", "You do not have permission to log in.");
                    return View();
                }

                // Redirect to AssetHub
                return RedirectToAction("Index", "Asset");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View();
            }
        }

        // Verify the password by hashing the entered password with the stored salt
        private bool VerifyPassword(string enteredPassword, string storedHashedPassword, string salt)
        {
            if (string.IsNullOrWhiteSpace(salt))
            {
                _logger.LogError("Password verification failed: Missing or empty salt value.");
                throw new ArgumentException("The salt value is missing or invalid.");
            }

            try
            {
                byte[] saltBytes = Convert.FromBase64String(salt); // Validate Base64 encoding
                using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 100000);
                byte[] hash = rfc2898DeriveBytes.GetBytes(32);
                string enteredHashedPassword = Convert.ToBase64String(hash);
                return enteredHashedPassword == storedHashedPassword;
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Salt value is not a valid Base64 string.");
                throw new ArgumentException("The salt value is not a valid Base64 string.", ex);
            }
        }
    }
}
