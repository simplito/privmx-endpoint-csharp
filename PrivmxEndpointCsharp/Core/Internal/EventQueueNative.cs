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
    internal class EventQueueNative : INativeExecutor
    {
        public enum Method
        {
            WaitEvent = 0,
            GetEvent = 1,
            EmitBreakEvent = 2

        }

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_newEventQueue(out IntPtr outPtr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_freeEventQueue(IntPtr ptr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_execEventQueue(IntPtr ptr, int method, IntPtr value, out IntPtr result);

        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execEventQueue(ptr, method, value, out result);
        }
    }
}
