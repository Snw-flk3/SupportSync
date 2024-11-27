using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using HopeWorldWide.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HopeWorldWide.Controllers
{
    public class ReportsRequestsController : Controller
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly ILogger<ReportsRequestsController> _logger;

        public ReportsRequestsController(FirestoreDb firestoreDb, ILogger<ReportsRequestsController> logger)
        {
            _firestoreDb = firestoreDb;
            _logger = logger;
        }

        // Action to display the combined reports and requests dashboard
        public async Task<IActionResult> Dashboard(string assetCodeFilter = null, string categoryFilter = null)
        {
            try
            {
                // Fetch reports and requests data with optional filters
                var report = await GetReportsAsync(assetCodeFilter);
                var requests = await GetRequestsAsync(categoryFilter);

                // Render the Index view inside the ReportsRequestsDashboard folder with both reports and requests
                var model = new ReportsRequestsViewModel
                {
                    Reports = report,
                    Requests = requests
                };

                return View("~/Views/ReportsRequestsDashboard/Index.cshtml", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reports and requests from Firestore");
                TempData["Error"] = "Unable to load reports and requests. Please try again later.";

                // Return the view with empty lists to avoid runtime errors
                var emptyModel = new ReportsRequestsViewModel
                {
                    Reports = new List<Report>(),
                    Requests = new List<Request>()
                };
                return View("~/Views/ReportsRequestsDashboard/Index.cshtml", emptyModel);
            }
        }

        // Fetch reports data from Firestore with filtering by assetCode
        private async Task<List<Report>> GetReportsAsync(string assetCodeFilter = null)
        {
            var reportsRef = _firestoreDb.Collection("report");

            // Create a query and apply filter if assetCodeFilter is provided
            Query query = reportsRef;  // Initialize the query with the collection reference
            if (!string.IsNullOrEmpty(assetCodeFilter))
            {
                query = query.WhereEqualTo("assetCode", assetCodeFilter);
            }

            var snapshot = await query.GetSnapshotAsync();  // Execute the query

            var reports = new List<Report>();
            foreach (var document in snapshot.Documents)
            {
                var report = new Report
                {
                    assetCode = document.TryGetValue("assetCode", out string assetCode) ? assetCode : null,
                    issueDescription = document.TryGetValue("issueDescription", out string issueDescription) ? issueDescription : null,
                    timestamp = document.TryGetValue("timestamp", out Timestamp timestamp) ? timestamp.ToDateTime() : default(DateTime)
                };
                reports.Add(report);
            }

            return reports;
        }

        // Fetch requests data from Firestore with filtering by category
        private async Task<List<Request>> GetRequestsAsync(string categoryFilter = null)
        {
            var requestsRef = _firestoreDb.Collection("requests");

            // Create a query and apply filter if categoryFilter is provided
            Query query = requestsRef;  // Initialize the query with the collection reference
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                query = query.WhereEqualTo("category", categoryFilter);
            }

            var snapshot = await query.GetSnapshotAsync();  // Execute the query

            var requests = new List<Request>();
            foreach (var document in snapshot.Documents)
            {
                var request = new Request
                {
                    category = document.TryGetValue("category", out string category) ? category : null,
                    employeeId = document.TryGetValue("employeeId", out string employeeId) ? employeeId : null,
                    reason = document.TryGetValue("reason", out string reason) ? reason : null,
                    timestamp = document.TryGetValue("timestamp", out Timestamp timestamp) ? timestamp.ToDateTime() : default(DateTime),
                    type = document.TryGetValue("type", out string type) ? type : null
                };
                requests.Add(request);
            }

            return requests;
        }

        // Action to resolve (delete) a report by assetCode
        [HttpPost]
        public async Task<IActionResult> ResolveReport(string assetCode)
        {
            try
            {
                // Query Firestore to find the report by assetCode
                var reportRef = _firestoreDb.Collection("report").WhereEqualTo("assetCode", assetCode);
                var snapshot = await reportRef.GetSnapshotAsync();

                if (snapshot.Documents.Count > 0)
                {
                    var document = snapshot.Documents.First();
                    // Delete the document
                    await document.Reference.DeleteAsync();
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Report not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resolving report");
                return Json(new { success = false, message = "An error occurred while resolving the report." });
            }
        }

        // Action to resolve (delete) a request by employeeId
        [HttpPost]
        public async Task<IActionResult> ResolveRequest(string employeeId)
        {
            try
            {
                // Query Firestore to find the request by employeeId
                var requestRef = _firestoreDb.Collection("requests").WhereEqualTo("employeeId", employeeId);
                var snapshot = await requestRef.GetSnapshotAsync();

                if (snapshot.Documents.Count > 0)
                {
                    var document = snapshot.Documents.First();
                    // Delete the document
                    await document.Reference.DeleteAsync();
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Request not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resolving request");
                return Json(new { success = false, message = "An error occurred while resolving the request." });
            }
        }
    }
}
