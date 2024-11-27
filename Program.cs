using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Google.Cloud.Firestore;
using System;
using System.IO;

namespace HopeWorldWide
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register FirestoreDb as a singleton for dependency injection
            builder.Services.AddSingleton(provider =>
            {
                // Define the path to your Firebase credentials file
                string pathToCredentials = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "supportsync-main-firebase-adminsdk-eqm68-c67d780b0b.json");

                // Check if the credentials file exists
                if (!File.Exists(pathToCredentials))
                {
                    Console.WriteLine("Error: Firebase credentials file not found at " + pathToCredentials);
                    throw new FileNotFoundException("Firebase credentials file not found.", pathToCredentials);
                }

                // Set the Google application credentials environment variable
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCredentials);

                // Initialize Firestore with the specified project ID
                string projectId = "supportsync-main"; // Replace with your actual project ID
                return FirestoreDb.Create(projectId);
            });

            // Add session services
            builder.Services.AddDistributedMemoryCache(); // Required for session state
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout duration
                options.Cookie.HttpOnly = true; // Prevent client-side access to session cookie
                options.Cookie.IsEssential = true; // Make session cookie essential for the application
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Use session middleware before routing and endpoints
            app.UseSession(); // <-- This needs to be before UseRouting and UseEndpoints

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
