using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Exiled.API.Features;

namespace Fentanyl_ReactorUpdate.API.Classes
{
    public class WebSocketServer
    {
        private static readonly string JsonFilePath = $"{Paths.Exiled}/BankSystem-{Server.Port}.json";
        private static readonly string EncryptionKey = "G5f#8kYq7^dPz!2LwR9$N@CmXt&UvBi";

        public void SubEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        }

        public void UnSubEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        }

        private void OnRoundStart()
        {
            Log.Info($"Custom message for UserID");
        }

        public string GetCustomMessage(string userId)
        {
            return "Solana";
        }
        

        public Dictionary<string, string> LoadMessagesFromJson()
        {
            string encryptedContent = File.ReadAllText(JsonFilePath);
            string decryptedContent = DecryptString(encryptedContent);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(decryptedContent) ?? new Dictionary<string, string>();
        }

        private void SaveMessagesToJson(Dictionary<string, string> messages)
        {
            string jsonContent = JsonConvert.SerializeObject(messages, Formatting.Indented);
            string encryptedContent = EncryptString(jsonContent);
            File.WriteAllText(JsonFilePath, encryptedContent);
        }

        private string EncryptString(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey.PadRight(32, ' ')); // Key should be 32 bytes
                aesAlg.IV = new byte[16]; // Initialize to 0 (you can use a better IV handling system)
                
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        private string DecryptString(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey.PadRight(32, ' ')); // Key should be 32 bytes
                aesAlg.IV = new byte[16]; // Initialize to 0 (you can use a better IV handling system)
                
                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}
