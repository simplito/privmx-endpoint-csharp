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
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Core.Internal;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Kvdb.Internal;
using PrivMX.Endpoint.Kvdb.Models;
using EventSelectorType = PrivMX.Endpoint.Kvdb.Models.Events.EventSelectorType;
using EventType = PrivMX.Endpoint.Kvdb.Models.Events.EventType;

namespace PrivMX.Endpoint.Kvdb
{
    /// <summary>
    /// 'KvdbApi' is a class representing Endpoint's API for Kvdbs and their messages.
    /// </summary>
    public class KvdbApi : IKvdbApi
    {
        public readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new KvdbApiNative());

        /// <summary>
        /// Creates an instance of 'KvdbApi'.
        /// </summary>
        /// <param name="connection">instance of 'Connection'</param>
        /// <returns>KvdbApi object</returns>
        public static KvdbApi Create(Connection connection)
        {
            KvdbApi kvdbApi = new KvdbApi(connection);
            kvdbApi.executor.ExecuteVoid(kvdbApi.ptr, (int)KvdbApiNative.Method.Create, new List<object?>{});
            return kvdbApi;
        }

        private KvdbApi(Connection connection)
        {
            KvdbApiNative.privmx_endpoint_newKvdbApi(connection.ptr, out ptr);
        }

        ~KvdbApi()
        {
            KvdbApiNative.privmx_endpoint_freeKvdbApi(ptr);
        }

        /// <summary>
        /// Creates a new KVDB in given Context.
        /// </summary>
        /// <param name="contextId">ID of the Context to create the KVDB in</param>
        /// <param name="users">array of UserWithPubKey structs which indicates who will have access to the created KVDB</param>
        /// <param name="managers"> array of UserWithPubKey struct which indicates who will have access
        /// (and management rights) to the created KVDB</param>
        /// <param name="publicMeta">public (unencrypted) metadata</param>
        /// <param name="privateMeta">private (encrypted) metadat</param>
        /// <param name="policy">s KVDB's policie</param>
        /// <returns>ID of the created KVDB</returns>
        public string CreateKvdb(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers,
            byte[] publicMeta, byte[] privateMeta,
            ContainerPolicy? policy = null)
        {
            return executor.Execute<string>(ptr, (int)KvdbApiNative.Method.CreateKvdb, new List<object?> 
            { 
                contextId, users, managers, publicMeta, privateMeta, policy 
            });
        }

        /// <summary>
        /// Updates an existing KVDB
        /// </summary>
        /// <param name="kvdbId">ID of the KVDB to update</param>
        /// <param name="users">array of UserWithPubKey structs which indicates who will have access to the updated KVDB</param>
        /// <param name="managers"> array of UserWithPubKey struct which indicates who will have access
        /// (and management rights) to the updated KVDB</param>
        /// <param name="publicMeta">public (unencrypted) metadata</param>
        /// <param name="privateMeta">private (encrypted) metadat</param>
        /// <param name="version">current version of the updated KVD</param>
        /// <param name="force">force update (without checking version</param>
        /// <param name="forceGenerateNewKey">force to regenerate a key for the KVD</param>
        /// <param name="policy">KVDB's policie</param>
        public void UpdateKvdb(string kvdbId, List<UserWithPubKey> users, List<UserWithPubKey> managers, 
            byte[] publicMeta, byte[] privateMeta, long version,
            bool force, bool forceGenerateNewKey, ContainerPolicy? policy = null)
        {
            executor.ExecuteVoid(ptr, (int)KvdbApiNative.Method.UpdateKvdb, new List<object?>
            {
                kvdbId, users, managers, publicMeta, privateMeta, version, force, forceGenerateNewKey, policy
            });
        }

        /// <summary>
        /// Deletes a KVDB by given KVDB ID
        /// </summary>
        /// <param name="kvdbId">ID of the KVDB to delete</param>
        public void DeleteKvdb(string kvdbId)
        {
            executor.ExecuteVoid(ptr,  (int)KvdbApiNative.Method.DeleteKvdb, new List<object?>{kvdbId});
        }

        /// <summary>
        /// Gets a KVDB by given KVDB ID
        /// </summary>
        /// <param name="kvdbId">ID of KVDB to get</param>
        /// <returns>struct containing info about the KVD</returns>
        public Models.Kvdb GetKvdb(string kvdbId)
        {
            return executor.Execute<Models.Kvdb>(ptr, (int)KvdbApiNative.Method.GetKvdb, new List<object?>{kvdbId});
        }

        /// <summary>
        /// Gets a list of Kvdbs in given Context
        /// </summary>
        /// <param name="contextId">ID of the Context to get the Kvdbs from</param>
        /// <param name="pagingQuery">pagingQuery with list query parameter</param>
        /// <returns>struct containing a list of Kvdb</returns>
        public PagingList<Models.Kvdb> ListKvdbs(string contextId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<Models.Kvdb>>(ptr, (int)KvdbApiNative.Method.ListKvdbs, 
                new List<object?>{contextId, pagingQuery});
        }

        /// <summary>
        /// Gets a KVDB entry by given KVDB entry key and KVDB I
        /// </summary>
        /// <param name="kvdbId">KVDB ID of the KVDB entry to ge</param>
        /// <param name="key">key of the KVDB entry to get</param>
        /// <returns>struct containing the KVDB entry</returns>
        public KvdbEntry GetEntry(string kvdbId, string key)
        {
            return executor.Execute<KvdbEntry>(ptr, (int)KvdbApiNative.Method.GetEntry, new List<object?>{kvdbId, key});
        }

        /// <summary>
        /// Check whether the KVDB entry exists
        /// </summary>
        /// <param name="kvdbId">KVDB ID of the KVDB entry to check</param>
        /// <param name="key">key of the KVDB entry to check</param>
        /// <returns>'true' if the KVDB has an entry with given key, 'false' otherwise</returns>
        public bool hasEntry(string kvdbId, string key)
        {
            return executor.ExecuteValue<bool>(ptr, (int)KvdbApiNative.Method.HasEntry, new List<object?>{kvdbId, key});
        }

        /// <summary>
        /// Gets a list of KVDB entries keys from a KVDB
        /// </summary>
        /// <param name="kvdbId">ID of the KVDB to list KVDB entries from</param>
        /// <param name="pagingQuery">pagingQuery with list query parameter</param>
        /// <returns>struct containing a list of KVDB entries</returns>
        public PagingList<string> ListEntriesKeys(string kvdbId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<string>>(ptr,  (int)KvdbApiNative.Method.ListEntriesKeys, 
                new List<object?>{kvdbId, pagingQuery});
        }

        /// <summary>
        /// Gets a list of KVDB entries from a KVDB.
        /// </summary>
        /// <param name="kvdbId">ID of the KVDB to list KVDB entries from</param>
        /// <param name="pagingQuery">pagingQuery with list query parameters</param>
        /// <returns>struct containing a list of KVDB entries</returns>
        public PagingList<KvdbEntry> ListEntries(string kvdbId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<KvdbEntry>>(ptr,   (int)KvdbApiNative.Method.ListEntries,
                new List<object?>{kvdbId, pagingQuery});
        }

        /// <summary>
        /// Sets a KVDB entry in the given KVDB.
        /// </summary>
        /// <param name="kvdbId">ID of the KVDB to set the entry to</param>
        /// <param name="key">KVDB entry key</param>
        /// <param name="publibMeta">public KVDB entry metadata</param>
        /// <param name="privateMeta">private KVDB entry metadata</param>
        /// <param name="data">content of the KVDB entry</param>
        /// <param name="version">version number of the entry</param>
        public void SetEntry(string kvdbId, string key, byte[] publibMeta, byte[] privateMeta, byte[] data, 
            long version = 0)
        {
            executor.ExecuteVoid(ptr, (int)KvdbApiNative.Method.SetEntry, 
                new List<object?> {kvdbId, key, publibMeta, privateMeta, data, version});
        }

        /// <summary>
        /// Deletes a KVDB entry by given KVDB entry ID
        /// </summary>
        /// <param name="kvdbId">KVDB ID of the KVDB entry</param>
        /// <param name="key">key of the KVDB entry</param>
        public void DeleteEntry(string kvdbId, string key)
        {
            executor.ExecuteVoid(ptr, (int)KvdbApiNative.Method.DeleteEntry, new List<object?>{kvdbId, key});
        }

        /// <summary>
        /// Deletes KVDB entries by given KVDB IDs and the list of entry keys.
        /// </summary>
        /// <param name="kvdbId">ID of the KVDB database to delete from</param>
        /// <param name="keys">vector of the keys of the KVDB entries to delete</param>
        /// <returns>map with the statuses of deletion for every key</returns>
        public Dictionary<string, bool> DeleteEntries(string kvdbId, List<string> keys)
        {
            return executor.Execute<Dictionary<string, bool>>(ptr, (int)KvdbApiNative.Method.DeleteEntries,
                new List<object?> { kvdbId, keys });
        }

        /// <summary>
        /// Subscribe for the KVDB events on the given subscription query.
        /// </summary>
        /// <param name="subscriptionQueries">list of queries</param>
        /// <returns>list of subscriptionIds in maching order to subscriptionQueries</returns>
        public List<string> SubscribeFor(List<string> subscriptionQueries)
        {
            return executor.Execute<List<string>>(ptr,  (int)KvdbApiNative.Method.SubscribeFor, 
                new List<object?>{subscriptionQueries});
        }

        /// <summary>
        /// Unsubscribe from events for the given subscriptionId.
        /// </summary>
        /// <param name="subscriptionIds">list of subscriptionId</param>
        public void UnsubscribeFrom(List<string> subscriptionIds)
        {
            executor.ExecuteVoid(ptr, (int)KvdbApiNative.Method.UnsubscribeFrom, new List<object?>{subscriptionIds});
        }

        /// <summary>
        /// Generate subscription Query for the KVDB events for single KvdbEntry.
        /// </summary>
        /// <param name="channelName">type of event which you listen for</param>
        /// <param name="selectorType">Id of Kvdb</param>
        /// <param name="selectorId">ey of Kvdb Entry</param>
        /// <returns>A subscription query as string</returns>
        public string BuildSubscriptionQuery(string channelName, EventSelectorType selectorType, string selectorId)
        {
            return executor.Execute<string>(ptr, (int)KvdbApiNative.Method.BuildSubscriptionQuery,
                new List<object?> { channelName, selectorType, selectorId });
        }

        /// <summary>
        /// Generate subscription Query for the KVDB events for sin
        /// </summary>
        /// <param name="eventType">aram eventType type of event which you listen for</param>
        /// <param name="kvdbId">Id of Kvdb</param>
        /// <param name="kvdbEntryKey">Key of Kvdb Entry</param>
        /// <returns>A subscription query as string</returns>
        public string BuildSubscriptionQueryForSelectedEntry(EventType eventType, string kvdbId, string kvdbEntryKey)
        {
            return executor.Execute<string>(ptr, (int)KvdbApiNative.Method.BuildSubscriptionQueryForSelectedEntry,
                new List<object?> { eventType, kvdbId, kvdbEntryKey });
        }
    }
}