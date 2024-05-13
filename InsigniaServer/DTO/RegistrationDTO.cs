using System;
namespace GameServer.DTO
{
    public class RegistrationDTO
    {
        public UserCredentialsDTO credentialsDTO { get; set; } = null!;

        public UserDTO userDTO { get; set; } = null!;
    }
}