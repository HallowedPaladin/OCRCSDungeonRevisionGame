using System.Security.Cryptography;

namespace GameServer.Auth;

public class SecretGenerator
{
    public static string GenerateSecret(int length)
    {
        var randomByteArray = new byte[length];
        string secret = "";

        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            // Fill the byte array with a cryptographically strong random sequence of bytes.
            randomNumberGenerator.GetBytes(randomByteArray);
            secret = Convert.ToBase64String(randomByteArray);
        }

        // Return a Base64 encoded random sequence
        return secret;
    }
}