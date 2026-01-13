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

namespace PrivMX.Endpoint.Core.Internal
{
    internal class UtilsNative : INativeExecutor
    {
        public enum Method
        {
            EncodeHex = 1,
            DecodeHex = 2,
            IsHex = 3,
            EncodeBase32 = 4,
            DecodeBase32 = 5,
            IsBase32 = 6,
            EncodeBase64 = 7,
            DecodeBase64 = 8,
            IsBase64 = 9,
            Trim = 10,
            Split = 11,
            Ltrim = 12,
            Rtrim = 13,
        }
        
        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_newUtils(out IntPtr outPtr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_freeUtils(IntPtr ptr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_execUtils(IntPtr ptr, int method, IntPtr value, out IntPtr result);
        
        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execUtils(ptr, method, value, out result);
        }
    }
}