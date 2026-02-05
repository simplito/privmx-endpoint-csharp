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

namespace PrivMX.Endpoint.Stream.Internal
{
    internal class StreamApiNative : INativeExecutor
    {
        public enum Method
        {
            Create = 0,
            GetTurnCredentials = 1,
            CreateStreamRoom = 2,
            UpdateStreamRoom = 3,
            ListStreamRooms = 4,
            GetStreamRoom = 5,
            DeleteStreamRoom = 6,

            SubscribeFor = 7,
            UnsubscribeFrom = 8,
            BuildSubscriptionQuery = 9,

            ListStreams = 10,
            JoinStreamRoom = 11,
            LeaveStreamRoom = 12,

            CreateStream = 13,
            PublishStream = 14,
            UnpublishStream = 15,
            SubscribeToRemoteStreams = 16,
            ModifyRemoteStreamsSubscriptions = 17,
            UnsubscribeFromRemoteStreams = 18,
            Trickle = 19,
            AcceptOfferOnReconfigure = 20,
            KeyManagement = 21,
            UpdateStream = 22,

            JoinStreamRoomEx = 25,
        }
        
        [DllImport("libprivmxendpointstream")]
        public static extern int privmx_endpoint_newStreamApiLow(IntPtr connectionPtr, IntPtr eventApiPtr, out IntPtr outPtr);
        
        [DllImport("libprivmxendpointstream")]
        public static extern int privmx_endpoint_freeStreamApiLow(IntPtr ptr);
        
        [DllImport("libprivmxendpointstream")]
        public static extern int privmx_endpoint_execStreamApiLow(IntPtr ptr, int method, IntPtr value, out IntPtr result);

        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execStreamApiLow(ptr, method, value, out result);
        }
    }
}