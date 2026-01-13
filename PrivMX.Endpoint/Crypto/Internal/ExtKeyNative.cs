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
    internal class ExtKeyNative : INativeExecutor
    {
        public enum Method
        {
            FromSeed = 0,
            FromBase58 = 1,
            GenerateRandom = 2,
            Derive = 3,
            DeriveHardened = 4,
            GetPrivatePartAsBase58 = 5,
            GetPublicPartAsBase58 = 6,
            GetPrivateKey = 7,
            GetPublicKey = 8,
            GetPrivateEncKey = 9,
            GetPublicKeyAsBase58Address = 10,
            GetChainCode = 11,
            VerifyCompactSignatureWithHash = 12,
            IsPrivate = 13
        }
        
        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_newExtKey(out IntPtr outPtr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_freeExtKey(IntPtr ptr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_execExtKey(IntPtr ptr, int method, IntPtr value, out IntPtr result);
        
        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execExtKey(ptr, method, value, out result);
        }
    }
}