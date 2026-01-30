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

using PrivMX.Endpoint.Core.Internal;
using System;
using System.Runtime.InteropServices;

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
            Deleted_Function_0 = 11,
            Deleted_Function_1 = 12,
            Deleted_Function_2 = 13,
            Deleted_Function_3 = 14,
            SubscribeFor = 15,
            UnsubscribeFrom = 16,
            BuildSubscriptionQuery = 17,
        }

#if ANDROID
        [DllImport("libprivmxendpointthread")]
#else
        [DllImport("libprivmxendpointinterface")]
#endif
        public static extern int privmx_endpoint_newThreadApi(IntPtr connectionPtr, out IntPtr outPtr);

#if ANDROID
        [DllImport("libprivmxendpointthread")]
#else
        [DllImport("libprivmxendpointinterface")]
#endif
        public static extern int privmx_endpoint_freeThreadApi(IntPtr ptr);

#if ANDROID
        [DllImport("libprivmxendpointthread")]
#else
        [DllImport("libprivmxendpointinterface")]
#endif
        public static extern int privmx_endpoint_execThreadApi(IntPtr ptr, int method, IntPtr value, out IntPtr result);

        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execThreadApi(ptr, method, value, out result);
        }
    }
}
