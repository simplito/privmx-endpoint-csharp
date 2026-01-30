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
    internal class ConnectionNative : INativeExecutor
    {
        public enum Method
        {
            Connect = 0,
            ConnectPublic = 1,
            GetConnectionId = 2,
            ListContexts = 3,
            Disconnect = 4,
            Deleted_Function_0 = 5,
            SetUserVerifier = 6,
            SubscribeFor = 7,
            UnsubscribeFrom = 8,
            BuildSubscriptionQuery = 9,
            ListContextUsers = 10
        }

#if ANDROID
        [DllImport("libprivmxendpointcore")]
#else
        [DllImport("libprivmxendpointinterface")]
#endif
        public static extern int privmx_endpoint_newConnection(out IntPtr outPtr);

#if ANDROID
        [DllImport("libprivmxendpointcore")]
#else
        [DllImport("libprivmxendpointinterface")]
#endif
        public static extern int privmx_endpoint_freeConnection(IntPtr ptr);

#if ANDROID
        [DllImport("libprivmxendpointcore")]
#else
        [DllImport("libprivmxendpointinterface")]
#endif
        public static extern int privmx_endpoint_execConnection(IntPtr ptr, int method, IntPtr value, out IntPtr result);

        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execConnection(ptr, method, value, out result);
        }
        
#if ANDROID
        [DllImport("libprivmxendpointcore")]
#else
        [DllImport("libprivmxendpointinterface")]
#endif
        public static extern int privmx_endpoint_setCertsPath(IntPtr certsPath);
    }
}
