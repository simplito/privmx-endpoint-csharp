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
    internal class BackendRequesterNative : INativeExecutor
    {
        public enum Method
        {
            BackendRequest = 0
        }

#if ANDROID
        [DllImport("libprivmxendpointcore")]
#else
        [DllImport("libprivmxendpointinterface")]
#endif
        public static extern int privmx_endpoint_newBackendRequester(out IntPtr outPtr);

#if ANDROID
        [DllImport("libprivmxendpointcore")]
#else
        [DllImport("libprivmxendpointinterface")]
#endif
        public static extern int privmx_endpoint_freeBackendRequester(IntPtr ptr);

#if ANDROID
        [DllImport("libprivmxendpointcore")]
#else
        [DllImport("libprivmxendpointinterface")]
#endif
        public static extern int privmx_endpoint_execBackendRequester(IntPtr ptr, int method, IntPtr value, out IntPtr result);

        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execBackendRequester(ptr, method, value, out result);
        }
    }
}
