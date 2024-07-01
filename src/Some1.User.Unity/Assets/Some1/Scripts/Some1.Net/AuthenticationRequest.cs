using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MemoryPack;

namespace Some1.Net
{
    [MemoryPackable]
    public partial class AuthenticationRequest
    {
        public byte[] Payload1 { get; set; } = null!;
        public byte[] Payload2 { get; set; } = null!;
        public byte[] Payload3 { get; set; } = null!;

        public static AuthenticationRequest Create(string idToken, string publicKeyXml)
        {
            byte[] desKey, desIV, encryptedIdToken;
            using (var mStream = new MemoryStream())
            {
                using (var des = DES.Create())
                using (ICryptoTransform encryptor = des.CreateEncryptor(des.Key, des.IV))
                using (var cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
                using (var streamWriter = new StreamWriter(cStream, Encoding.UTF8))
                {
                    streamWriter.Write(idToken);
                    desKey = des.Key;
                    desIV = des.IV;
                }

                encryptedIdToken = mStream.ToArray();
            }

            using var rsa = RSA.Create();
            rsa.FromXmlString(publicKeyXml);
            return new()
            {
                Payload1 = rsa.Encrypt(desKey, RSAEncryptionPadding.Pkcs1),
                Payload2 = rsa.Encrypt(desIV, RSAEncryptionPadding.Pkcs1),
                Payload3 = encryptedIdToken
            };
        }
        
        public string GetIdToken(string keyXml)
        {
            if (Payload1 is null || Payload2 is null || Payload3 is null)
            {
                throw new InvalidOperationException();
            }

            byte[] desKey, desIV;
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(keyXml);
                desKey = rsa.Decrypt(Payload1, RSAEncryptionPadding.Pkcs1);
                desIV = rsa.Decrypt(Payload2, RSAEncryptionPadding.Pkcs1);
            }

            using var mStream = new MemoryStream(Payload3);
            using var des = DES.Create();
            using ICryptoTransform decryptor = des.CreateDecryptor(desKey, desIV);
            using var cStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read);
            using StreamReader reader = new StreamReader(cStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}
