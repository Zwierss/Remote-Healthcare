using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace DoctorLogic;

public static class Cryptographer
{
    /* The key used to encrypt and decrypt the message. */
    private static readonly byte[] Key =
    {
        0xF1, 0x22, 0xAA, 0x40, 0x7B, 0xBF, 0x32, 0x58,
        0x88, 0xE9, 0xAE, 0x69, 0x72, 0xF4, 0xBD, 0x5D
    };

   /* The initialization vector for the AES encryption. */
     private static readonly byte[] Iv =
    {
        0x41, 0xE4, 0x43, 0x42, 0x81, 0x5F, 0xDA, 0xE8,
        0x8E, 0x88, 0xA0, 0xFF, 0xEF, 0x6E, 0x5B, 0x54
    }; 
    
    /// <summary>
    /// It takes a JObject, converts it to a string, converts the string to a byte array, creates an AES object, sets the
    /// key and IV, creates an encryptor, creates a memory stream, creates a crypto stream, writes the data to the crypto
    /// stream, flushes the final block, and returns the memory stream as a byte array
    /// </summary>
    /// <param name="JObject">The packet that you want to encrypt.</param>
    /// <returns>
    /// The encrypted message
    /// </returns>
    public static byte[] GetEncryptedMessage(JObject packet)
    {
        string message = packet.ToString();
        byte[] data = Encoding.ASCII.GetBytes(message);
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = Iv;

        using ICryptoTransform ct = aes.CreateEncryptor(aes.Key, aes.IV);
        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, ct, CryptoStreamMode.Write);
        cs.Write(data,0,data.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }

    /// <summary>
    /// > Decrypts the message using the key and IV, then parses the message into a JObject
    /// </summary>
    /// <param name="message">The encrypted message</param>
    /// <returns>
    /// A JObject
    /// </returns>
    public static JObject GetDecryptedMessage(byte[] message)
    {
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = Iv;

        using ICryptoTransform ct = aes.CreateDecryptor(aes.Key, aes.IV);
        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, ct, CryptoStreamMode.Write);
        cs.Write(message, 0, message.Length);
        cs.FlushFinalBlock();
        return JObject.Parse(Encoding.Default.GetString(ms.ToArray()));
    }
}