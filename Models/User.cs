using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

namespace HopeWorldWide.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [FirestoreProperty]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [FirestoreProperty]
        [Required(ErrorMessage = "Employee ID is required")]
        public string EmployeeId { get; set; }

        [FirestoreProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [FirestoreProperty]
        public string Password { get; set; }

        [FirestoreProperty]
        public string Salt { get; set; } // No validation since this is auto-generated

        [FirestoreProperty]
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        // Firestore Document ID, not mapped to Firestore
        public string Id { get; set; } // No validation as it's managed internally
    }
}
