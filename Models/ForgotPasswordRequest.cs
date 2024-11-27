namespace HopeWorldWide.Models
{
    public class ForgotPasswordRequest
    {
        public string Id { get; set; } // Firestore autoId
        public string Email { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RequestTime { get; set; }
    }
}
