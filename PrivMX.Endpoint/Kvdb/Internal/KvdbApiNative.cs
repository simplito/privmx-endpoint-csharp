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

namespace PrivMX.Endpoint.Kvdb.Internal
{
    internal class KvdbApiNative : INativeExecutor
    {
        public enum Method
        {
            Create = 0,
            CreateKvdb = 1,
            UpdateKvdb = 2,
            DeleteKvdb = 3,
            GetKvdb = 4,
            ListKvdbs = 5,
            GetEntry = 6,
            ListEntriesKeys = 7,
            ListEntries = 8,
            SetEntry = 9,
            DeleteEntry = 10,
            DeleteEntries = 11,
            Deleted_Function_0 = 12,
            Deleted_Function_1 = 13,
            Deleted_Function_2 = 14,
            Deleted_Function_3 = 15,
            HasEntry = 16,
            SubscribeFor = 17,
            UnsubscribeFrom = 18,
            BuildSubscriptionQuery = 19,
            BuildSubscriptionQueryForSelectedEntry = 20,
        }
        
        [DllImport("libprivmxendpointkvdb")]
        public static extern int privmx_endpoint_newKvdbApi(IntPtr connectionPtr, out IntPtr outPtr);
        
        [DllImport("libprivmxendpointkvdb")]
        public static extern int privmx_endpoint_freeKvdbApi(IntPtr ptr);
        
        [DllImport("libprivmxendpointkvdb")]
        public static extern int privmx_endpoint_execKvdbApi(IntPtr ptr, int method, IntPtr value, out IntPtr result);
        
        public int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result)
        {
            return privmx_endpoint_execKvdbApi(ptr, method, value, out result);
        }
    }
}