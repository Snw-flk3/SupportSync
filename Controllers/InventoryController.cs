using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using HopeWorldWide.Models;

namespace HopeWorldWide.Controllers
{
    public class InventoryController : Controller
    {
        private readonly FirestoreDb _firestoreDb;

        public InventoryController()
        {
            // Initialize FirestoreDb instance
            string pathToCredentials = @"C:\Users\lab_services_student\source\repos\HopeWorldWide\HopeWorldWide\App_Data\supportsync-main-firebase-adminsdk-eqm68-8f17837218.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCredentials);
            _firestoreDb = FirestoreDb.Create("supportsync-main");
        }

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            var inventoryQuantity = await CalculateInventory();

            // Fetch intake and stock quantities for visualization
            var intakeQuerySnapshot = await _firestoreDb.Collection("intake").GetSnapshotAsync();
            var intakeQuantity = intakeQuerySnapshot.Documents.Sum(doc => doc.GetValue<int>("inQuantity"));

            var stockQuerySnapshot = await _firestoreDb.Collection("stock").GetSnapshotAsync();
            var stockQuantity = stockQuerySnapshot.Documents.Sum(doc => doc.GetValue<int>("quantity"));

            ViewBag.InventoryQuantity = inventoryQuantity;
            ViewBag.IntakeQuantity = intakeQuantity;
            ViewBag.StockQuantity = stockQuantity;

            return View();
        }

        // Method to calculate the total inventory
        private async Task<int> CalculateInventory()
        {
            try
            {
                // Calculate total intake quantity (sum of inQuantity in the 'intake' collection)
                var intakeQuerySnapshot = await _firestoreDb.Collection("intake").GetSnapshotAsync();
                var intakeQuantity = intakeQuerySnapshot.Documents.Sum(doc => doc.GetValue<int>("inQuantity"));

                // Calculate total stock quantity (sum of quantity in the 'stock' collection)
                var stockQuerySnapshot = await _firestoreDb.Collection("stock").GetSnapshotAsync();
                var stockQuantity = stockQuerySnapshot.Documents.Sum(doc => doc.GetValue<int>("quantity"));

                // Calculate the current inventory (intake - stock)
                int currentInventory = intakeQuantity - stockQuantity;

                // Update or create the inventory document with the calculated inventory
                await UpdateInventory(currentInventory);

                return currentInventory;
            }
            catch (Exception ex)
            {
                // Log the error and return 0 in case of an exception
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        // Method to update the inventory document in Firestore
        private async Task UpdateInventory(int currentInventory)
        {
            var inventoryRef = _firestoreDb.Collection("inventory").Document("currentInventory");

            var inventoryDoc = await inventoryRef.GetSnapshotAsync();

            if (inventoryDoc.Exists)
            {
                await inventoryRef.UpdateAsync("currentQuantity", currentInventory);
            }
            else
            {
                var newInventory = new Inventory
                {
                    productId = "currentInventory",
                    currentQuantity = currentInventory
                };

                await inventoryRef.SetAsync(newInventory);
            }
        }
    }
}

