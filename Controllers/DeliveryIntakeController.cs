using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using HopeWorldWide.Models;

namespace HopeWorldWide.Controllers
{
    public class DeliveryIntakeController : Controller
    {
        private FirestoreDb _firestoreDb;

        public DeliveryIntakeController()
        {
            // Initialize FirestoreDb instance
            string pathToCredentials = @"C:\Users\lab_services_student\source\repos\HopeWorldWide\HopeWorldWide\App_Data\supportsync-main-firebase-adminsdk-eqm68-8f17837218.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCredentials);
            _firestoreDb = FirestoreDb.Create("supportsync-main");
        }

        // GET: DeliveryIntake
        public IActionResult Index()
        {
            return View();
        }

        // POST: DeliveryIntake
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Intake model)
        {
            // Validate the form input
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Create a new document reference in the 'intake' collection
                var intakeDocRef = _firestoreDb.Collection("intake").Document();

                // Set the data in the Firestore document
                await intakeDocRef.SetAsync(model);

                // Redirect to the same page with a success message
                TempData["Message"] = "Delivery intake registered successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle any errors
                ModelState.AddModelError("", "Failed to register delivery intake: " + ex.Message);
                return View(model);
            }
        }
    }
}
