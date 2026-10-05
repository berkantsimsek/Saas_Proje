namespace SaaSProje.Models
{
    public class User : IMustHaveTenant
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int FirmId { get; set; }

        // Navigation Property
        public Firm Firm { get; set; }
    }
}