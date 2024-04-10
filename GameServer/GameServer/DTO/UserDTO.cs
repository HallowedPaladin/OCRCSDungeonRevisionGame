namespace GameServer.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? FamilyName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Email { get; set; }

        public int? PhoneCountryCode { get; set; }

        public int? PhoneNumber { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime Timestamp { get; set; }

        public int UserTypeId { get; set; }
    }
}