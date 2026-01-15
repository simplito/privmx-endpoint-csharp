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

namespace PrivMX.Endpoint.Kvdb.Models
{
    /// <summary>
    /// KvdbApi subscription event types to listen for 
    /// </summary>
    public enum EventType : long
    {
        KVDB_CREATE = 0,
        KVDB_UPDATE = 1,
        KVDB_DELETE = 2,
        KVDB_STATS = 3,
        ENTRY_CREATE = 4,
        ENTRY_UPDATE = 5,
        ENTRY_DELETE = 6,
        COLLECTION_CHANGE = 7,
    }
}