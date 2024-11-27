using System.ComponentModel.DataAnnotations;

namespace HopeWorldWide.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 100 characters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 100 characters.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Employee ID must be between 6 and 50 characters.")]
        public string EmployeeId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "Email must be a valid address.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select a role.")]
        public string Role { get; set; }
    }
}

