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


namespace PrivMX.Endpoint.Thread.Models
{
    /// <summary>
    /// ThreadApi subscription event types to listen for 
    /// </summary>
    public enum EventType : long
    {
        THREAD_CREATE = 0,
        THREAD_UPDATE = 1,
        THREAD_DELETE = 2,
        THREAD_STATS = 3,
        MESSAGE_CREATE = 4,
        MESSAGE_UPDATE = 5,
        MESSAGE_DELETE = 6,
        COLLECTION_CHANGE = 7,
    }
}