using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ER70_483._3_Debuging_and_Security._3._2_Encryption
{

    #region Sample 1
    /// <summary>
    /// Initial sample, showing how to perform a symmetric encription/decription by using AES algorithm
    /// </summary>
    [SampleName(Name = "Sample 1 - Basic symmetric encryption")]
    public class SymmetricEncriptionSample1 : BasicSample
    {
        static byte[] Encrypt(SymmetricAlgorithm aesAlg, string plainText)
        {
            // First step is to set the encriptor's configuration by passing the key and the initialization vector (IV)
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt =
                new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return msEncrypt.ToArray();
                }
            }
        }
        static string Decrypt(SymmetricAlgorithm aesAlg, byte[] cipherText)
        {
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt =
                new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        protected override void UseSample()
        {
            string original = "My secret data!";
            using (SymmetricAlgorithm symmetricAlgorithm =
            new AesManaged())
            {
                byte[] encrypted = Encrypt(symmetricAlgorithm, original);
                string roundtrip = Decrypt(symmetricAlgorithm, encrypted);
                // Displays: My secret data!
                Logger.Info($"Original: {original}");
                Logger.Info($"Round Trip: {roundtrip}");
            }
        }
    }
    #endregion

    #region Sample 2
    /// <summary>
    /// Basic asymmetric encription. Key generation
    /// </summary>
    [SampleName(Name = "Sample 2 - Basic symmetric encryption. RSA private and public key generation")]
    public class SymmetricEncriptionSample2 : BasicSample
    {
        protected override void UseSample()
        {
            // This example shows how you can create a new instance of the RSACryptoServiceProvider and
            // export the public key to XML.By passing true to the ToXmlString method, you also export the
            // private part of your key
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicKeyXML = rsa.ToXmlString(false);
            string privateKeyXML = rsa.ToXmlString(true);
            Logger.Info(publicKeyXML);
            Logger.Info(privateKeyXML);
        }
    }
    #endregion

    #region Sample 3
    /// <summary>
    /// Basic asymmetric encription
    /// </summary>
    [SampleName(Name = "Sample 3 - Using a public and a private key to encrypt and decrypt data")]
    public class SymmetricEncriptionSample3 : BasicSample
    {
        protected override void UseSample()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicKeyXML = rsa.ToXmlString(false);
            string privateKeyXML = rsa.ToXmlString(true);

            // First step is to convert our secret message into an array of bytes, so .Net the encryption mechanism can work with the data
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes("My Secret Data!");

            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(publicKeyXML);
                encryptedData = RSA.Encrypt(dataToEncrypt, false);
            }
            byte[] decryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(privateKeyXML);
                decryptedData = RSA.Decrypt(encryptedData, false);
            }
            string decryptedString = ByteConverter.GetString(decryptedData);
            Logger.Info(decryptedString); // Displays: My Secret Data!
        }
    }
    #endregion

    #region Sample 4
    /// <summary>
    /// Basic asymmetric encription. Using a container to store the public and a private key
    /// The .NET Framework offers a secure location for storing asymmetric keys in a key container. A key container can be specific to a user or to the whole machine
    /// </summary>
    [SampleName(Name = "Sample 4 - Using a container to store the public and a private key")]
    public class SymmetricEncriptionSample4 : BasicSample
    {
        protected override void UseSample()
        {
            // First step is to convert our secret message into an array of bytes, so .Net the encryption mechanism can work with the data
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes("My Secret Data!");

            string containerName = "SecretContainer";
            CspParameters csp = new CspParameters() { KeyContainerName = containerName };
            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(csp))
            {
                encryptedData = RSA.Encrypt(dataToEncrypt, false);
            }

            byte[] decryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(csp))
            {
                decryptedData = RSA.Decrypt(encryptedData, false);
            }
            string decryptedString = ByteConverter.GetString(decryptedData);
            Logger.Info(decryptedString); // Displays: My Secret Data!
        }
    }
    #endregion
}
