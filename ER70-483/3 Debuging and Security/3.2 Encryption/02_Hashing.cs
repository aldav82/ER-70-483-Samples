using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ER70_483._3_Debuging_and_Security._3._2_Encryption
{
    #region Sample 1
    /// <summary>
    /// Initial sample, using SHA256Managed to calculate hash code
    /// </summary>
    [SampleName(Name = "Sample 1 - Basic symmetric encryption")]
    public class HashingSample1 : BasicSample
    {
        protected override void UseSample()
        {
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            SHA256 sha256 = SHA256.Create();
            string data = "A paragraph of text";
            byte[] hashA = sha256.ComputeHash(byteConverter.GetBytes(data));
            data = "A paragraph of changed text";
            byte[] hashB = sha256.ComputeHash(byteConverter.GetBytes(data));
            data = "A paragraph of text";
            byte[] hashC = sha256.ComputeHash(byteConverter.GetBytes(data));
            Logger.Info(hashA.SequenceEqual(hashB)); // Displays: false
            Logger.Info(hashA.SequenceEqual(hashC)); // Displays: true
        }
    }
    #endregion
}
