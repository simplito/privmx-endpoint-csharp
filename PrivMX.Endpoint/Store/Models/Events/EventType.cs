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

namespace PrivMX.Endpoint.Store.Models.Events
{
    /// <summary>
    /// KvdbApi subscription event types to listen for 
    /// </summary>
    public enum EventType : long
    {
        STORE_CREATE = 0,
        STORE_UPDATE = 1,
        STORE_DELETE = 2,
        STORE_STATS = 3,
        FILE_CREATE = 4,
        FILE_UPDATE = 5,
        FILE_DELETE = 6,
        COLLECTION_CHANGE = 7,
    }
}