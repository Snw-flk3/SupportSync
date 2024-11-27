using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using HopeWorldWide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HopeWorldWide.Controllers
{
    public class ProfileController : Controller
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(FirestoreDb firestoreDb, ILogger<ProfileController> logger)
        {
            _firestoreDb = firestoreDb ?? throw new ArgumentNullException(nameof(firestoreDb));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index(string EmployeeId)
        {
            if (string.IsNullOrWhiteSpace(EmployeeId))
            {
                return View(new User());
            }

            try
            {
                var userRef = _firestoreDb.Collection("users").WhereEqualTo("EmployeeId", EmployeeId);
                var snapshot = await userRef.GetSnapshotAsync();

                if (snapshot.Documents.Count > 0)
                {
                    var userDoc = snapshot.Documents.First();
                    var user = userDoc.ConvertTo<User>();
                    user.Id = userDoc.Id;  // Set the Firestore document ID

                    return View(user);
                }

                ModelState.AddModelError("", "No user found with the provided Employee ID.");
                return View(new User { EmployeeId = EmployeeId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by EmployeeId");
                ModelState.AddModelError("", "An error occurred while fetching the profile.");
                return View(new User { EmployeeId = EmployeeId });
            }
        }


       
        [HttpPost]
public async Task<IActionResult> Index(User model)
{
    ModelState.Remove("Id");
    ModelState.Remove("Salt");

    if (!ModelState.IsValid)
    {
        _logger.LogWarning("ModelState is invalid.");
        return View(model);
    }

    if (string.IsNullOrEmpty(model.Id))
    {
        _logger.LogWarning("User ID (Document ID) is missing. Cannot update profile.");
        ModelState.AddModelError("", "An error occurred: User ID is missing.");
        return View(model);
    }

    try
    {
        var userRef = _firestoreDb.Collection("users").Document(model.Id);

        var updates = new Dictionary<string, object>
        {
            { "EmployeeId", model.EmployeeId },
            { "FirstName", model.FirstName },
            { "LastName", model.LastName },
            { "Email", model.Email },
            { "Role", model.Role }
        };

        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            var (hashedPassword, salt) = HashPassword(model.Password);
            updates.Add("Password", hashedPassword);
            updates.Add("Salt", salt);
        }

        await userRef.SetAsync(updates, SetOptions.MergeAll);

        TempData["Success"] = "Profile updated successfully!";
        return RedirectToAction("Index", new { EmployeeId = model.EmployeeId });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to update profile.");
        ModelState.AddModelError("", "An error occurred while updating the profile.");
        return View(model);
    }
}



        // Generates a hashed password and salt
        private (string Password, string Salt) HashPassword(string password)
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
