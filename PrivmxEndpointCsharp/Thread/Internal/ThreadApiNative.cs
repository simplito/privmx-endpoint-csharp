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

namespace PrivMX.Endpoint.Thread.Internal
{
    internal class ThreadApiNative : INativeExecutor
    {
        public enum Method
        {
            Create = 0,
            CreateThread = 1,
            UpdateThread = 2,
            DeleteThread = 3,
            GetThread = 4,
            ListThreads = 5,
            GetMessage = 6,
            ListMessages = 7,
            SendMessage = 8,
            DeleteMessage = 9,
            UpdateMessage = 10,
            SubscribeForThreadEvents = 11,
            UnsubscribeFromThreadEvents = 12,
            SubscribeForMessageEvents = 13,
            UnsubscribeFromMessageEvents = 14
        }

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_newThreadApi(IntPtr connectionPtr, out IntPtr outPtr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_freeThreadApi(IntPtr ptr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_execThreadApi(IntPtr ptr, int method, IntPtr value, out IntPtr result);

        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execThreadApi(ptr, method, value, out result);
        }
    }
}
