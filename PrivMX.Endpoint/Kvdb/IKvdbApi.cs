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

using System.Collections.Generic;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Kvdb.Models;
using EventType = PrivMX.Endpoint.Kvdb.Models.EventType;

namespace PrivMX.Endpoint.Kvdb
{
    public interface IKvdbApi
    {
        string CreateKvdb(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers,
            byte[] publicMeta, byte[] privateMeta, ContainerPolicy? policy = null);
        void UpdateKvdb(string kvdbId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, 
            byte[] privateMeta, long version, bool force, bool forceGenerateNewKey, ContainerPolicy? policy = null);
        void DeleteKvdb(string kvdbId);
        Models.Kvdb GetKvdb(string kvdbId);
        PagingList<Models.Kvdb> ListKvdbs(string contextId, PagingQuery pagingQuery);
        KvdbEntry GetEntry(string kvdbId, string key);
        bool hasEntry(string kvdbId, string key);
        PagingList<string> ListEntriesKeys(string kvdbId, PagingQuery pagingQuery);
        PagingList<KvdbEntry> ListEntries(string kvdbId, PagingQuery pagingQuery);
        void SetEntry(string kvdbId, string key, byte[] publibMeta, byte[] privateMeta, byte[] data, long version = 0);
        void DeleteEntry(string kvdbId, string key);
        Dictionary<string, bool> DeleteEntries(string kvdbId, List<string> keys);
        List<string> SubscribeFor(List<string> subscriptionQueries);
        void UnsubscribeFrom(List<string> subscriptionIds);
        string BuildSubscriptionQuery(string channelName, Models.EventSelectorType selectorType, string selectorId);
        string BuildSubscriptionQueryForSelectedEntry(EventType eventType, string kvdbIds, string kvdbEntryKey);
    }
}