using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Server;

public static class Cryptographer
{
    private static readonly byte[] Key =
    {
        0xF1, 0x22, 0xAA, 0x40, 0x7B, 0xBF, 0x32, 0x58,
        0x88, 0xE9, 0xAE, 0x69, 0x72, 0xF4, 0xBD, 0x5D
    };

    private static readonly byte[] IV =
    {
        0x41, 0xE4, 0x43, 0x42, 0x81, 0x5F, 0xDA, 0xE8,
        0x8E, 0x88, 0xA0, 0xFF, 0xEF, 0x6E, 0x5B, 0x54
    };
    
    public static byte[] GetEncryptedMessage(JObject o)
    {
        string message = o.ToString();
        byte[] data = Encoding.ASCII.GetBytes(message);
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using ICryptoTransform ct = aes.CreateEncryptor(aes.Key, aes.IV);
        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, ct, CryptoStreamMode.Write);
        cs.Write(data,0,data.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }

    public static JObject GetDecryptedMessage(byte[] message)
    {
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using ICryptoTransform ct = aes.CreateDecryptor(aes.Key, aes.IV);
        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, ct, CryptoStreamMode.Write);
        cs.Write(message,0,message.Length);
        cs.FlushFinalBlock();
        return JObject.Parse(Encoding.Default.GetString(ms.ToArray()));
    }
}