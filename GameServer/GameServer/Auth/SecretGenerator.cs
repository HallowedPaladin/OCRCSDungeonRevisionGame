using System;
using System.Security.Cryptography;
using System.Text;

namespace GameServer.Auth;

public class SecretGenerator
{
    public static string GenerateRandomSecret(int length)
    {
        // Define the characters allowed in the secret
        const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Create a byte array to hold the random bytes
        byte[] randomBytes = new byte[length];

        // Generate random bytes using RandomNumberGenerator
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        // Convert the random bytes to characters
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in randomBytes)
        {
            // Ensure that the byte value is within the range of allowed characters
            int index = b % allowedChars.Length;
            stringBuilder.Append(allowedChars[index]);
        }

        return stringBuilder.ToString();
    }
}