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

namespace PrivMX.Endpoint.Event.Internal
{
    internal class EventApiNative : INativeExecutor
    {
        public enum Method
        {
            Create = 0,
            EmitEvent = 1,
            Deleted_Function_0 = 2,
            Deleted_Function_1 = 3,
            SubscribeFor = 4,
            UnsubscribeFrom = 5,
            BuildSubscriptionQuery = 6,
        }
        
        [DllImport("libprivmxendpointevent")]
        public static extern int privmx_endpoint_newEventApi(IntPtr connectionPtr, out IntPtr outPtr);
        
        [DllImport("libprivmxendpointevent")]
        public static extern int privmx_endpoint_freeEventApi(IntPtr ptr);
        
        [DllImport("libprivmxendpointevent")]
        public static extern int privmx_endpoint_execEventApi(IntPtr ptr, int method, IntPtr value, out IntPtr result);

        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execEventApi(ptr, method, value, out result);
        }
    }
}
