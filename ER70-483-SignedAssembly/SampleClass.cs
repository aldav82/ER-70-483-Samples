using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER70_483_SignedAssembly
{
    /// <summary>
    /// In order to sign an assembly, you will need to set the following values:
    ///     * Assembly Name
    ///     * Assembly Version Number
    ///     * Assembly Culture Information
    ///     
    /// By strongly naming an assembly we can guarantee its uniqueness, since it is necessary to use your private key.
    /// This also provides a strong integrity check, since only your private key can sign the assembly, and your public key is used to verify its validity.
    /// The public key is distributed with the assembly
    /// 
    /// To obtain a new key pair file we can use the developer command prompt and run the following command:
    ///     sn -k myKey.snk
    /// The key generation can be done inside Visual Studio by accessing the tab Signing, in the library project properties
    /// </summary>
    public class SampleClass
    {
        public int Operation()
        {
            return 9;
        }
    }
}
