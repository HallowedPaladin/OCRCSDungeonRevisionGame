namespace InsigniaServer.DTO
{
    public class RegistrationDTO
    {
        public UserCredentialsDTO credentialsDTO { get; set; } = null!;

        public UserDTO userDTO { get; set; } = null!;
    }
}