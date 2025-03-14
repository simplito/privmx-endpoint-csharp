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

namespace PrivMX.Endpoint.Inbox.Internal
{
    internal class InboxApiNative : INativeExecutor
    {
        public enum Method
        {
            Create = 0,
            CreateInbox = 1,
            UpdateInbox = 2,
            GetInbox = 3,
            ListInboxes = 4,
            GetInboxPublicView = 5,
            DeleteInbox = 6,
            PrepareEntry = 7,
            SendEntry = 8,
            ReadEntry = 9,
            ListEntries = 10,
            DeleteEntry = 11,
            CreateFileHandle = 12,
            WriteToFile = 13,
            OpenFile = 14,
            ReadFromFile = 15,
            SeekInFile = 16,
            CloseFile = 17,
            SubscribeForInboxEvents = 18,
            UnsubscribeFromInboxEvents = 19,
            SubscribeForEntryEvents = 20,
            UnsubscribeFromEntryEvents = 21
        }

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_newInboxApi(IntPtr connectionPtr, IntPtr threadApiPtr, IntPtr storeApiPtr, out IntPtr outPtr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_freeInboxApi(IntPtr ptr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_execInboxApi(IntPtr ptr, int method, IntPtr value, out IntPtr result);

        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execInboxApi(ptr, method, value, out result);
        }
    }
}
