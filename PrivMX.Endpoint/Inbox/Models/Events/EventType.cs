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

namespace PrivMX.Endpoint.Inbox.Models.Events
{
    /// <summary>
    /// InboxApi subscription event types to listen for 
    /// </summary>
    public enum EventType : long
    {
        INBOX_CREATE = 0,
        INBOX_UPDATE = 1,
        INBOX_DELETE = 2,
        ENTRY_CREATE = 3,
        ENTRY_DELETE = 4,
        COLLECTION_CHANGE = 5,
    }
}