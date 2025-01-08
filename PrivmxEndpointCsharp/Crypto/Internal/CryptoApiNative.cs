//
// PrivMX Endpoint C#
// Copyright © 2024 Simplito sp. z o.o.
//
// This file is part of the PrivMX Platform (https://privmx.dev).
// This software is Licensed under the MIT License.
//
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Runtime.InteropServices;
using PrivMX.Endpoint.Core.Internal;

namespace PrivMX.Endpoint.Crypto.Internal
{
    internal class CryptoApiNative : INativeExecutor
    {
        public enum Method
        {
            Create = 0,
            SignData = 1,
            GeneratePrivateKey = 2,
            DerivePrivateKey = 3,
            DerivePublicKey = 4,
            GenerateKeySymmetric = 5,
            EncryptDataSymmetric = 6,
            DecryptDataSymmetric = 7,
            ConvertPEMKeytoWIFKey = 8,
            VerifySignature = 9,
        }

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_newCryptoApi(out IntPtr outPtr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_freeCryptoApi(IntPtr ptr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_execCryptoApi(IntPtr ptr, int method, IntPtr value, out IntPtr result);

        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execCryptoApi(ptr, method, value, out result);
        }
    }
}
