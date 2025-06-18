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
using PrivMX.Endpoint.Thread.Models;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Thread
{
    public interface IThreadApi
    {
        string CreateThread(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, ContainerPolicy? policies = null);
        void UpdateThread(string threadId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, long version, bool force, bool forceGenerateNewKey, ContainerPolicy? policies = null);
        void DeleteThread(string threadId);
        Models.Thread GetThread(string threadId);
        PagingList<Models.Thread> ListThreads(string contextId, PagingQuery pagingQuery);
        Message GetMessage(string messageId);
        PagingList<Message> ListMessages(string threadId, PagingQuery pagingQuery);
        string SendMessage(string threadId, byte[] publicMeta, byte[] privateMeta, byte[] data);
        void UpdateMessage(string messageId, byte[] publicMeta, byte[] privateMeta, byte[] data);
        void DeleteMessage(string messageId);
        void SubscribeForThreadEvents();
        void UnsubscribeFromThreadEvents();
        void SubscribeForMessageEvents(string threadId);
        void UnsubscribeFromMessageEvents(string threadId);
    }
}
