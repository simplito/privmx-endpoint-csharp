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

using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Inbox.Models;
using System.Collections.Generic;
using PrivMX.Endpoint.Inbox.Models.Events;
using EventType = PrivMX.Endpoint.Inbox.Models.Events.EventType;

namespace PrivMX.Endpoint.Inbox
{
    public interface IInboxApi
    {
        string CreateInbox(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, 
            byte[] privateMeta, FilesConfig? filesConfig, ContainerPolicyWithoutItem? policies = null);
        void UpdateInbox(string inboxId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, 
            byte[] privateMeta, FilesConfig? filesConfig, long version, bool force, bool forceGenerateNewKey, 
            ContainerPolicyWithoutItem? policies = null);
        Models.Inbox GetInbox(string inboxId);
        PagingList<Models.Inbox> ListInboxes(string contextId, PagingQuery pagingQuery);
        InboxPublicView GetInboxPublicView(string inboxId);
        void DeleteInbox(string inboxId);
        long PrepareEntry(string inboxId, byte[] data, List<long> inboxFileHandles, byte[]? userPrivKey);
        void SendEntry(long inboxHandle);
        InboxEntry ReadEntry(string inboxEntryId);
        PagingList<InboxEntry> ListEntries(string inboxId, PagingQuery pagingQuery);
        void DeleteEntry(string inboxEntryId);
        long CreateFileHandle(byte[] publicMeta, byte[] privateMeta, long fileSize);
        void WriteToFile(long inboxHandle, long inboxFileHandle, byte[] dataChunk);
        long OpenFile(string fileId);
        byte[] ReadFromFile(long fileHandle, long length);
        void SeekInFile(long fileHandle, long position);
        string CloseFile(long fileHandle);
        List<string> SubscribeFor(List<string> subscriptionQueries);
        void UnsubscribeFrom(List<string> subscriptionIds);
        string BuildSubscriptionQuery(EventType eventType, EventSelectorType selectorType, string selectorId);
    }
}
