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

namespace PrivMX.Endpoint.Store.Internal
{
    internal class StoreApiNative : INativeExecutor
    {
        public enum Method
        {
            Create = 0,
            CreateStore = 1,
            UpdateStore = 2,
            DeleteStore = 3,
            GetStore = 4,
            ListStores = 5,
            CreateFile = 6,
            UpdateFile = 7,
            UpdateFileMeta = 8,
            WriteToFile = 9,
            DeleteFile = 10,
            GetFile = 11,
            ListFiles = 12,
            OpenFile = 13,
            ReadFromFile = 14,
            SeekInFile = 15,
            CloseFile = 16,
            SubscribeForStoreEvents = 17,
            UnsubscribeFromStoreEvents = 18,
            SubscribeForFileEvents = 19,
            UnsubscribeFromFileEvents = 20
        }

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_newStoreApi(IntPtr connectionPtr, out IntPtr outPtr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_deleteStoreApi(IntPtr ptr);

        [DllImport("libprivmxendpointinterface")]
        public static extern int privmx_endpoint_execStoreApi(IntPtr ptr, int method, IntPtr value, out IntPtr result);

        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execStoreApi(ptr, method, value, out result);
        }
    }
}
