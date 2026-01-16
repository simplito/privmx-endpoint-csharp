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

using PrivMX.Endpoint.Inbox.Models;
using PrivMX.Endpoint.Event.Models;
using System;
using System.Collections.Generic;
using PrivMX.Endpoint.Core.Models.Events;
using PrivMX.Endpoint.Inbox.Models.Events;
using PrivMX.Endpoint.Kvdb.Models.Events;
using PrivMX.Endpoint.Store.Models.Events;
using PrivMX.Endpoint.Thread.Models.Events;

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
            {"core$CollectionChangedEvent", typeof(CollectionChangedEvent)},
            {"core$ContextUserAddedEvent", typeof(ContextUserAddedEvent)},
            {"core$ContextUserRemovedEvent", typeof(ContextUserRemovedEvent)},
            {"core$ContextUsersStatusChangedEvent", typeof(ContextUsersStatusChangedEvent)},

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
            {"inbox$InboxEntryDeletedEvent", typeof(InboxEntryDeletedEvent)},

            {"event$ContextCustomEvent", typeof(ContextCustomEvent)},
            
            {"kvdb$KvdbCreatedEvent", typeof(KvdbCreatedEvent)},
            {"kvdb$KvdbDeletedEvent", typeof(KvdbDeletedEvent)},
            {"kvdb$KvdbEntryDeletedEvent", typeof(KvdbEntryDeletedEvent)},
            {"kvdb$KvdbEntryUpdatedEvent", typeof(KvdbEntryUpdatedEvent)},
            {"kvdb$KvdbNewEntryEvent", typeof(KvdbNewEntryEvent)},
            {"kvdb$KvdbStatsChangedEvent", typeof(KvdbStatsChangedEvent)},
            {"kvdb$KvdbUpdatedEvent", typeof(KvdbUpdatedEvent)}
        };
    }
}
