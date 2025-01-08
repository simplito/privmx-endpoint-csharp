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
            GetInstanceId = 2,
            ListContexts = 3,
            Disconnect = 4

        }

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_newConnection(out IntPtr outPtr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_freeConnection(IntPtr ptr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_execConnection(IntPtr ptr, int method, IntPtr value, out IntPtr result);

        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execConnection(ptr, method, value, out result);
        }
    }
}
