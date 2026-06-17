using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public static class StaticMethods
    {
        public const string Key = "36gYDeQlI2vV";
        public static Tuple<string, string> HashPassword(string plainText)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(plainText, salt);
            return Tuple.Create(salt, hashPassword);
        }
        public static bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        public static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }
        public static string EncryptId(string Id)
        {
            var valueBytes = Encoding.UTF8.GetBytes(Id);
            return Convert.ToBase64String(valueBytes);
        }
        static readonly char[] padding = { '=' };


        public static string Encrypt(string Id)
        {
            string key = "9876543210ABFEDC";
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(Id);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);


            var cipher = TripleDES.Create();

            cipher.Key = keyBytes;
            cipher.Mode = CipherMode.ECB;
            cipher.Padding = PaddingMode.PKCS7;


            ICryptoTransform transform = cipher.CreateEncryptor();
            byte[] ciphertextBytes = transform.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

            string ciphertext = Convert.ToBase64String(ciphertextBytes);

            return ciphertext;
        }
        public static string Decrypt(string Id)
        {
            string key = "9876543210ABFEDC";
            byte[] ciphertextBytes = Convert.FromBase64String(Id);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            var cipher = TripleDES.Create();

            cipher.Key = keyBytes;
            cipher.Mode = CipherMode.ECB;
            cipher.Padding = PaddingMode.PKCS7;

            ICryptoTransform transform = cipher.CreateDecryptor();
            byte[] plaintextBytes = transform.TransformFinalBlock(ciphertextBytes, 0, ciphertextBytes.Length);

            string plaintext = Encoding.UTF8.GetString(plaintextBytes);

            return plaintext;
        }
        public static string DecryptId(string Id)
        {
            var valueBytes = System.Convert.FromBase64String(Id);
            return Encoding.UTF8.GetString(valueBytes);
        }

        public static bool CheckEligibility(DateTime previousDate)
        {
            if (DateTime.Now < previousDate.AddMonths(3))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string BloodDonationStatus(DateTime dateTime)
        {
            var status = CheckEligibility(dateTime);
            string result = string.Empty;
            if (status == true)
            {
                result = "0";
            }
            else
            {
                result = "1";
            }
            return result;
        }
    }
}
