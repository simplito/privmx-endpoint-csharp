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
using PrivMX.Endpoint.Store.Models;
using System.Collections.Generic;
using EventSelectorType = PrivMX.Endpoint.Store.Models.EventSelectorType;
using EventType = PrivMX.Endpoint.Store.Models.EventType;

namespace PrivMX.Endpoint.Store
{
    public interface IStoreApi
    {
        string CreateStore(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, 
            byte[] privateMeta, ContainerPolicy? policies = null);
        void UpdateStore(string storeId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, 
            byte[] privateMeta, long version, bool force, bool forceGenerateNewKey, ContainerPolicy? policies = null);
        void DeleteStore(string storeId);
        Models.Store GetStore(string storeId);
        PagingList<Models.Store> ListStores(string contextId, PagingQuery pagingQuery);
        long CreateFile(string storeId, byte[] publicMeta, byte[] privateMeta, long size, bool randomWrite = false);
        long UpdateFile(string fileId, byte[] publicMeta, byte[] privateMeta, long size);
        void UpdateFileMeta(string fileId, byte[] publicMeta, byte[] privateMeta);
        void WriteToFile(long fileHandle, byte[] dataChunk, bool truncate = false);
        void DeleteFile(string storeId);
        File GetFile(string fileId);
        PagingList<File> ListFiles(string storeId, PagingQuery pagingQuery);
        long OpenFile(string fileId);
        byte[] ReadFromFile(long fileHandle, long length);
        void SeekInFile(long fileHandle, long position);
        string CloseFile(long fileHandle);
        List<string> SubscribeFor(List<string> subscriptionQueries);
        void UnsubscribeFrom(List<string> subscriptionIds);
        string BuildSubscriptionQuery(EventType eventType, EventSelectorType selectorType, string selectorId);
        void SyncFile(long fileHandle);
    }
}
