using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using HopeWorldWide.Models;

namespace HopeWorldWide.Controllers
{
    public class AssetController : Controller
    {
        private readonly FirestoreDb _firestoreDb;

        public AssetController()
        {
            string pathToCredentials = @"C:\Users\lab_services_student\source\repos\HopeWorldWide\HopeWorldWide\App_Data\supportsync-main-firebase-adminsdk-eqm68-c67d780b0b.json";

            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS")))
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCredentials);
            }

            _firestoreDb = FirestoreDb.Create("supportsync-main");
        }

        // GET: Asset Library with optional search query
        public async Task<IActionResult> Index(string searchQuery = "")
        {
            List<Asset> assets = new List<Asset>();

            try
            {
                // Fetch all assets from Firestore
                var snapshot = await _firestoreDb.Collection("assets").GetSnapshotAsync();

                foreach (var document in snapshot.Documents)
                {
                    if (document.Exists)
                    {
                        var asset = document.ConvertTo<Asset>();

                        // If search query is provided, filter by keyword in any field
                        if (string.IsNullOrEmpty(searchQuery) ||
                            asset.assetCode.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                            asset.assetMake.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                            asset.assetModel.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                            asset.location.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                            asset.site.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                            asset.department.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                            asset.assetCategory.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                        {
                            assets.Add(asset);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching assets: {ex.Message}");
                ModelState.AddModelError("", "Error loading assets.");
            }

            // Pass the searchQuery to the view
            ViewBag.SearchQuery = searchQuery;
            await PopulateDropdownsAsync();
            return View(assets);
        }

        private async Task PopulateDropdownsAsync()
        {
            try
            {
                ViewBag.assetCategories = new List<string> { "Electronics", "Furniture", "Vehicles", "Laptops" };
                ViewBag.locations = new List<string> { "Soweto", "Johannesburg", "Pretoria" };
                ViewBag.sites = new List<string> { "Senaoane", "Site B", "Site C" };
                ViewBag.departments = new List<string> { "IT", "HR", "Finance", "SupportSync" };
                ViewBag.AssignedUsers = await GetAssignedUserIdsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PopulateDropdownsAsync: {ex.Message}");
            }
        }

        private async Task<List<string>> GetAssignedUserIdsAsync()
        {
            var employeeIds = new List<string>();
            try
            {
                QuerySnapshot snapshot = await _firestoreDb.Collection("users").GetSnapshotAsync();
                foreach (var document in snapshot.Documents)
                {
                    if (document.Exists)
                    {
                        employeeIds.Add(document.GetValue<string>("EmployeeId"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
            }
            return employeeIds;
        }
    }
}
