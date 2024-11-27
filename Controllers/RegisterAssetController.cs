using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using HopeWorldWide.Models;

namespace HopeWorldWide.Controllers
{
    public class RegisterAssetController : Controller
    {
        private FirestoreDb _firestoreDb;

        public RegisterAssetController()
        {
            // Initialize FirestoreDb instance
            string pathToCredentials = @"C:\Users\lab_services_student\source\repos\HopeWorldWide\HopeWorldWide\App_Data\supportsync-main-firebase-adminsdk-eqm68-8f17837218.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCredentials);
            _firestoreDb = FirestoreDb.Create("supportsync-main");
        }

        // GET: RegisterAsset
        public async Task<IActionResult> Index()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        // POST: RegisterAsset
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Asset model)
        {
            // Validate user input
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(model);
            }

            // Create a new asset document
            var asset = new Asset
            {
                assetCode = model.assetCode, 
                assetMake = model.assetMake,
                assetModel = model.assetModel,
                EmployeeId = model.EmployeeId,
                location = model.location,
                site = model.site,
                department = model.department,
                assetCategory = model.assetCategory,
                currentValue = model.currentValue,
                insuredValue = model.insuredValue,
                allRiskCover = model.allRiskCover,
                policyCover = model.policyCover
            };

            try
            {
                // Add asset to Firestore 'assets' collection
                DocumentReference docRef = _firestoreDb.Collection("assets").Document();
                await docRef.SetAsync(asset);

                // Redirect to a confirmation page or back to the Register Asset page
                TempData["Message"] = "Asset registered successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log error or show error message to user
                ModelState.AddModelError("", "Failed to register asset: " + ex.Message);
                await PopulateDropdownsAsync();
                return View(model);
            }
        }

        private async Task PopulateDropdownsAsync()
        {
            ViewBag.AssetCategories = new List<string> { "Electronics", "Furniture", "Vehicles", "Laptops" };
            ViewBag.Locations = new List<string> { "Soweto", "Johannesburg", "Pretoria" };
            ViewBag.Sites = new List<string> { "Senaoane", "Site B", "Site C" };
            ViewBag.Departments = new List<string> { "IT", "HR", "Finance", "SupportSync" };

        }

    }
}
