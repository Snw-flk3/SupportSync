using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using HopeWorldWide.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace HopeWorldWide.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly ILogger<DeliveryController> _logger;

        public DeliveryController(FirestoreDb firestoreDb, ILogger<DeliveryController> logger)
        {
            _firestoreDb = firestoreDb;
            _logger = logger;
        }

        // Action to display the delivery history dashboard with pagination
        public async Task<IActionResult> Dashboard(int page = 1, int pageSize = 10)
        {
            try
            {
                // Fetch delivery history data with pagination
                var (deliveryHistory, totalRecords) = await GetDeliveryHistoryAsync(page, pageSize);

                // Set pagination metadata
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                // Render the Index view in the Dashboard folder
                return View("~/Views/Dashboard/Index.cshtml", deliveryHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching delivery history from Firestore");
                TempData["Error"] = "Unable to load delivery history. Please try again later.";

                // Return the view with an empty list to avoid runtime errors
                return View("~/Views/Dashboard/Index.cshtml", new List<DeliveryHistory>());
            }
        }

        // Fetch paginated delivery history data from Firestore
        private async Task<(List<DeliveryHistory>, int)> GetDeliveryHistoryAsync(int page, int pageSize)
        {
            var stockRef = _firestoreDb.Collection("stock");

            // Fetch all documents to calculate the total record count
            var allSnapshot = await stockRef.GetSnapshotAsync();
            int totalRecords = allSnapshot.Count;

            // Fetch paginated data without ordering
            var query = stockRef.Offset((page - 1) * pageSize).Limit(pageSize);
            var snapshot = await query.GetSnapshotAsync();

            // Handle empty or missing documents
            if (snapshot.Count == 0)
            {
                _logger.LogWarning("No documents found for the specified page.");
                return (new List<DeliveryHistory>(), totalRecords);
            }

            var deliveryHistory = new List<DeliveryHistory>();
            foreach (var document in snapshot.Documents)
            {
                try
                {
                    var delivery = new DeliveryHistory
                    {
                        DeliveryId = document.GetValue<string>("deliveryId"),
                        DriverId = document.GetValue<string>("driverId"),
                        Image = document.ContainsField("image") ? document.GetValue<string>("image") : "",
                        Location = document.GetValue<string>("location"),
                        Quantity = document.GetValue<int>("quantity"),
                        Status = document.GetValue<string>("status")
                    };
                    deliveryHistory.Add(delivery);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing document ID {document.Id}: {ex.Message}");
                }
            }

            return (deliveryHistory, totalRecords);
        }
    }
}
