using System;
namespace GameServer.DTO
{
    public class RegistrationDTO
    {
        public CredentialsDTO credentialsDTO { get; set; } = null!;

        public UserDTO userDTO { get; set; } = null!;
    }
}