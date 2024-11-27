using Google.Cloud.Firestore;
using HopeWorldWide.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopeWorldWide.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly ILogger<ForgotPasswordController> _logger;

        public ForgotPasswordController(FirestoreDb firestoreDb, ILogger<ForgotPasswordController> logger)
        {
            _firestoreDb = firestoreDb;
            _logger = logger;
        }

        // Action to display all forgot password requests
        public async Task<IActionResult> Index()
        {
            try
            {
                // Fetch forgot password requests from Firestore
                var requests = await GetForgotPasswordRequestsAsync();

                return View(requests); // Pass the data to the Razor view
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching forgot password requests");
                TempData["Error"] = "Unable to load forgot password requests. Please try again later.";
                return View(new List<Models.ForgotPasswordRequest>());
            }
        }

        // Fetch requests from Firestore
        private async Task<List<Models.ForgotPasswordRequest>> GetForgotPasswordRequestsAsync()
        {
            var forgotPasswordRef = _firestoreDb.Collection("forgotPassword");
            var snapshot = await forgotPasswordRef.GetSnapshotAsync();

            var requests = new List<Models.ForgotPasswordRequest>();
            foreach (var document in snapshot.Documents)
            {
                var request = new Models.ForgotPasswordRequest
                {
                    Id = document.Id,
                    Email = document.TryGetValue("Email", out string email) ? email : null,
                    EmployeeId = document.TryGetValue("EmployeeId", out string employeeId) ? employeeId : null,
                    FirstName = document.TryGetValue("FirstName", out string firstName) ? firstName : null,
                    LastName = document.TryGetValue("LastName", out string lastName) ? lastName : null,
                    RequestTime = document.TryGetValue("RequestTime", out Timestamp timestamp) ? timestamp.ToDateTime() : default(DateTime)
                };
                requests.Add(request);
            }

            return requests.OrderBy(r => r.RequestTime).ToList(); // Sort by request time
        }

        // Action to resolve a forgot password request
        [HttpPost]
        public async Task<IActionResult> Resolve(string id)
        {
            try
            {
                var requestRef = _firestoreDb.Collection("forgotPassword").Document(id);
                await requestRef.DeleteAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error resolving forgot password request with ID {id}");
                return Json(new { success = false, message = "Failed to resolve the request. Please try again." });
            }
        }
    }
}
