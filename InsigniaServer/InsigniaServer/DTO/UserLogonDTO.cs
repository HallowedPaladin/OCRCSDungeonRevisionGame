namespace GameServer.DTO
{
    public class UserLogonDTO
    {
        public int UserId { get; set; }

        public DateTime LastLogonDate { get; set; }

        public int LogonAttempts { get; set; }

        public sbyte IsLocked { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

