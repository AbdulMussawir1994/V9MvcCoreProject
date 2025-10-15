using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography.Utilities;

public class AesGcmEncryption
{
    private string _SecretKey;
    private readonly IConfiguration _configuration;

    public AesGcmEncryption(IConfiguration configuration)
    {
        _configuration = configuration;
        _SecretKey = _configuration["EncryptionSettings:EnPrivateKey"] ?? "b14ca5898a4e4133bbce2ea2315a1916";
    }

    public string Decrypt(string encryptedText, string Key = "")
    {
        string sR = string.Empty;
        if (!string.IsNullOrEmpty(encryptedText))
        {
            _SecretKey = string.IsNullOrEmpty(Key) ? _SecretKey : Key;
            byte[] iv = new byte[16];
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            AeadParameters parameters =
                      new AeadParameters(new KeyParameter(Encoding.UTF8.GetBytes(_SecretKey)), 128, iv, null);

            cipher.Init(false, parameters);
            byte[] plainBytes =
                  new byte[cipher.GetOutputSize(encryptedBytes.Length)];
            Int32 retLen = cipher.ProcessBytes
                  (encryptedBytes, 0, encryptedBytes.Length, plainBytes, 0);
            cipher.DoFinal(plainBytes, retLen);

            sR = Encoding.UTF8.GetString(plainBytes).TrimEnd
                 ("\r\n\0".ToCharArray());
        }
        return sR;
    }
    public string Encrypt(string plainText, string Key = "")
    {
        _SecretKey = string.IsNullOrEmpty(Key) ? _SecretKey : Key;
        byte[] iv = new byte[16];
        string sR = string.Empty;
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
        AeadParameters parameters =
                     new AeadParameters(new KeyParameter(Encoding.UTF8.GetBytes(_SecretKey)), 128, iv, null);
        cipher.Init(true, parameters);

        byte[] encryptedBytes =
               new byte[cipher.GetOutputSize(plainBytes.Length)];
        Int32 retLen = cipher.ProcessBytes
                       (plainBytes, 0, plainBytes.Length, encryptedBytes, 0);
        cipher.DoFinal(encryptedBytes, retLen);
        sR = Convert.ToBase64String
             (encryptedBytes, Base64FormattingOptions.None);
        return sR;
    }

    public string EncryptBase64String(string plainText, string keyBase64)
    {
        // ✅ Base64-decode to get the actual raw AES key bytes
        byte[] keyBytes = Convert.FromBase64String(keyBase64);

        if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
            throw new ArgumentException("Invalid AES key length. Must be 128, 192, or 256 bits.");

        // Initialization Vector (should be random per encryption in real usage)
        byte[] iv = new byte[16];

        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
        AeadParameters parameters = new AeadParameters(new KeyParameter(keyBytes), 128, iv, null);
        cipher.Init(true, parameters);

        byte[] encryptedBytes = new byte[cipher.GetOutputSize(plainBytes.Length)];
        int retLen = cipher.ProcessBytes(plainBytes, 0, plainBytes.Length, encryptedBytes, 0);
        cipher.DoFinal(encryptedBytes, retLen);

        return Convert.ToBase64String(encryptedBytes);
    }

    public string DecryptBase64String(string encryptedText, string keyBase64)
    {

        // ✅ Decode Base64 key into raw AES key bytes
        byte[] keyBytes = Convert.FromBase64String(keyBase64);

        byte[] iv = new byte[16]; // GCM standard IV size (can be random per encryption)
        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

        GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
        AeadParameters parameters = new AeadParameters(new KeyParameter(keyBytes), 128, iv, null);

        cipher.Init(false, parameters);

        byte[] plainBytes = new byte[cipher.GetOutputSize(encryptedBytes.Length)];
        int retLen = cipher.ProcessBytes(encryptedBytes, 0, encryptedBytes.Length, plainBytes, 0);
        cipher.DoFinal(plainBytes, retLen);

        return Encoding.UTF8.GetString(plainBytes).TrimEnd('\r', '\n', '\0');
    }

    public string PlanTextEncrypt(string plainText, string base64Key)
    {
        byte[] key = Convert.FromBase64String(base64Key);

        // Use 12-byte IV for AES-GCM (recommended by NIST)
        byte[] iv = RandomNumberGenerator.GetBytes(12);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        byte[] cipherBytes = new byte[plainBytes.Length];
        byte[] tag = new byte[16];

        using (var aes = new AesGcm(key))
        {
            aes.Encrypt(iv, plainBytes, cipherBytes, tag);
        }

        // Return hex string: IV + ciphertext + tag
        return $"{Convert.ToHexString(iv)}:{Convert.ToHexString(cipherBytes)}:{Convert.ToHexString(tag)}";
    }

    // Decrypts cipher text with AES-GCM
    public string PlanTextDecrypt(string encryptedText, string base64Key)
    {
        byte[] key = Convert.FromBase64String(base64Key);

        var parts = encryptedText.Split(':');
        if (parts.Length != 3)
            throw new ArgumentException("Invalid encrypted format (must contain iv:ciphertext:tag)");

        byte[] iv = Convert.FromHexString(parts[0]);
        byte[] cipherBytes = Convert.FromHexString(parts[1]);
        byte[] tag = Convert.FromHexString(parts[2]);

        byte[] plainBytes = new byte[cipherBytes.Length];

        using (var aes = new AesGcm(key))
        {
            aes.Decrypt(iv, cipherBytes, tag, plainBytes);
        }

        return Encoding.UTF8.GetString(plainBytes);
    }
}

