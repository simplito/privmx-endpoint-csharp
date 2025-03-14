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
using System.Collections.Generic;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Store.Models;
using PrivMX.Endpoint.Thread.Models;
using PrivMX.Endpoint.Inbox.Models;

namespace PrivMX.Endpoint.Core.Internal
{
    internal class TypeRegistry
    {
        public static readonly Dictionary<string, Type> Types = new Dictionary<string, Type>()
        {
            {"core$LibBreakEvent", typeof(LibBreakEvent)},
            {"core$LibConnectedEvent", typeof(LibConnectedEvent)},
            {"core$LibDisconnectedEvent", typeof(LibDisconnectedEvent)},
            {"core$LibPlatformDisconnectedEvent", typeof(LibPlatformDisconnectedEvent)},

            {"thread$ThreadCreatedEvent", typeof(ThreadCreatedEvent)},
            {"thread$ThreadUpdatedEvent", typeof(ThreadUpdatedEvent)},
            {"thread$ThreadDeletedEvent", typeof(ThreadDeletedEvent)},
            {"thread$ThreadStatsChangedEvent", typeof(ThreadStatsChangedEvent)},
            {"thread$ThreadNewMessageEvent", typeof(ThreadNewMessageEvent)},
            {"thread$ThreadMessageUpdatedEvent", typeof(ThreadMessageUpdatedEvent)},
            {"thread$ThreadMessageDeletedEvent", typeof(ThreadMessageDeletedEvent)},

            {"store$StoreCreatedEvent", typeof(StoreCreatedEvent)},
            {"store$StoreDeletedEvent", typeof(StoreDeletedEvent)},
            {"store$StoreFileCreatedEvent", typeof(StoreFileCreatedEvent)},
            {"store$StoreFileUpdatedEvent", typeof(StoreFileUpdatedEvent)},
            {"store$StoreStatsChangedEvent", typeof(StoreStatsChangedEvent)},
            {"store$StoreUpdatedEvent", typeof(StoreUpdatedEvent)},
            {"store$StoreFileDeletedEvent", typeof(StoreFileDeletedEvent)},

            {"inbox$InboxCreatedEvent", typeof(InboxCreatedEvent)},
            {"inbox$InboxUpdatedEvent", typeof(InboxUpdatedEvent)},
            {"inbox$InboxDeletedEvent", typeof(InboxDeletedEvent)},
            {"inbox$InboxEntryCreatedEvent", typeof(InboxEntryCreatedEvent)},
            {"inbox$InboxEntryDeletedEvent", typeof(InboxEntryDeletedEvent)}
        };
    }
}
