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

using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Core.Internal;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Store.Internal;
using PrivMX.Endpoint.Store.Models;
using System;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Store
{
    public class StoreApi : IStoreApi
    {
        public readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new StoreApiNative());

        /// <summary>
        /// Creates an instance of the <see cref="StoreApi"/>.
        /// </summary>
        /// <param name="connection">Instance of <see cref="Connection"/></param>
        /// <returns>Created instance of the <see cref="StoreApi"/>.</returns>
        static public StoreApi Create(Connection connection)
        {
            StoreApi storeApi = new StoreApi(connection);
            storeApi.executor.ExecuteVoid(storeApi.ptr, (int)StoreApiNative.Method.Create, new List<object>{});
            return storeApi;
        }

        private StoreApi(Connection connection)
        {
            StoreApiNative.privmx_endpoint_newStoreApi(connection.ptr, out ptr);
        }

        ~StoreApi()
        {
            StoreApiNative.privmx_endpoint_freeStoreApi(ptr);
        }

        /// <summary>
        /// Creates a new Store in given Context.
        /// </summary>
        /// <param name="contextId">ID of the Context to create the Store in.</param>
        /// <param name="users">Array of UserWithPubKey structs which indicates who will have access to the created Store.</param>
        /// <param name="managers">Array of UserWithPubKey structs which indicates who will have access (and management rights) to the created Store.</param>
        /// <param name="publicMeta">Public metadata that will remain unencrypted on the Bridge.</param>
        /// <param name="privateMeta">Private metadata that will be encrypted before being sent to the Bridge.</param>
        /// <param name="policies">(optional) Store policy.</param>
        /// <returns>Created Store ID.</returns>
        public string CreateStore(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, ContainerPolicy policies = null)
        {
            return executor.Execute<string>(ptr, (int)StoreApiNative.Method.CreateStore, new List<object>{contextId, users, managers, publicMeta, privateMeta, policies});
        }

        /// <summary>
        /// Updates an existing Store.
        /// </summary>
        /// <param name="storeId">ID of the Store to update.</param>
        /// <param name="users">Array of UserWithPubKey structs which indicates who will have access to the created Store.</param>
        /// <param name="managers">Array of UserWithPubKey structs which indicates who will have access (and management rights) to the created Store.</param>
        /// <param name="publicMeta">Public metadata that will remain unencrypted on the Bridge.</param>
        /// <param name="privateMeta">Private metadata that will be encrypted before being sent to the Bridge.</param>
        /// <param name="version">Current version of the updated Store.</param>
        /// <param name="force">Force update (without checking version).</param>
        /// <param name="forceGenerateNewKey">Force to renenerate a key for the Store.</param>
        /// <param name="policies">(optional) Store policy.</param>
        public void UpdateStore(string storeId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, long version, bool force, bool forceGenerateNewKey, ContainerPolicy policies = null)
        {
            executor.ExecuteVoid(ptr, (int)StoreApiNative.Method.UpdateStore, new List<object>{storeId, users, managers, publicMeta, privateMeta, version, force, forceGenerateNewKey, policies});
        }

        /// <summary>
        /// Deletes a Store by given Store ID.
        /// </summary>
        /// <param name="storeId">ID of the Store to delete.</param>
        public void DeleteStore(string storeId)
        {
            executor.ExecuteVoid(ptr, (int)StoreApiNative.Method.DeleteStore, new List<object>{storeId});
        }

        /// <summary>
        /// Gets a single Store by given Store ID.
        /// </summary>
        /// <param name="storeId">ID of the Store to get.</param>
        /// <returns>Information about about the Store.</returns>
        public Models.Store GetStore(string storeId)
        {
            return executor.Execute<Models.Store>(ptr, (int)StoreApiNative.Method.GetStore, new List<object>{storeId});
        }

        /// <summary>
        /// Gets a list of Stores in given Context.
        /// </summary>
        /// <param name="contextId">ID of the Context to get the Stores from.</param>
        /// <param name="pagingQuery">List query parameters.</param>
        /// <returns>List of Stores.</returns>
        public PagingList<Models.Store> ListStores(string contextId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<Models.Store>>(ptr, (int)StoreApiNative.Method.ListStores, new List<object>{contextId, pagingQuery});
        }

        /// <summary>
        /// Creates a new file in a Store.
        /// </summary>
        /// <param name="storeId">ID of the Store to create the file in.</param>
        /// <param name="publicMeta">Public file meta_data.</param>
        /// <param name="privateMeta">Private file meta_data.</param>
        /// <param name="size">Size of the file.</param>
        /// <returns>Handle to write data.</returns>
        public long CreateFile(string storeId, byte[] publicMeta, byte[] privateMeta, long size)
        {
            return executor.Execute<long>(ptr, (int)StoreApiNative.Method.CreateFile, new List<object>{storeId, publicMeta, privateMeta, size});
        }

        /// <summary>
        /// Updates an existing file in a Store.
        /// </summary>
        /// <param name="fileId">ID of the file to update.</param>
        /// <param name="publicMeta">Public file meta_data.</param>
        /// <param name="privateMeta">Private file meta_data.</param>
        /// <param name="size">Size of the file.</param>
        /// <returns>Handle to write file data.</returns>
        public long UpdateFile(string fileId, byte[] publicMeta, byte[] privateMeta, long size)
        {
            return executor.Execute<long>(ptr, (int)StoreApiNative.Method.UpdateFile, new List<object>{fileId, publicMeta, privateMeta, size});
        }

        /// <summary>
        /// Updates meta data of an existing file in a Store.
        /// </summary>
        /// <param name="fileId">ID of the file to update.</param>
        /// <param name="publicMeta">Public file meta_data.</param>
        /// <param name="privateMeta">Private file meta_data.</param>
        public void UpdateFileMeta(string fileId, byte[] publicMeta, byte[] privateMeta)
        {
            executor.ExecuteVoid(ptr, (int)StoreApiNative.Method.UpdateFileMeta, new List<object>{fileId, publicMeta, privateMeta});
        }

        /// <summary>
        /// Writes a file data.
        /// </summary>
        /// <param name="fileHandle">Handle to write file data.</param>
        /// <param name="dataChunk">File data chunk.</param>
        public void WriteToFile(long fileHandle, byte[] dataChunk)
        {
            executor.ExecuteVoid(ptr, (int)StoreApiNative.Method.WriteToFile, new List<object>{fileHandle, dataChunk});
        }

        /// <summary>
        /// Deletes a file by given ID.
        /// </summary>
        /// <param name="storeId">ID of the file to delete.</param>
        public void DeleteFile(string storeId)
        {
            executor.ExecuteVoid(ptr, (int)StoreApiNative.Method.DeleteFile, new List<object>{storeId});
        }

        /// <summary>
        /// Gets a single file by the given file ID.
        /// </summary>
        /// <param name="fileId">ID of the file to get.</param>
        /// <returns>Information about the file.</returns>
        public File GetFile(string fileId)
        {
            return executor.Execute<File>(ptr, (int)StoreApiNative.Method.GetFile, new List<object>{fileId});
        }

        /// <summary>
        /// Gets a list of files in given Store.
        /// </summary>
        /// <param name="storeId">ID of the Store to get files from.</param>
        /// <param name="pagingQuery">List query parameters.</param>
        /// <returns>List of files.</returns>
        public PagingList<File> ListFiles(string storeId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<File>>(ptr, (int)StoreApiNative.Method.ListFiles, new List<object>{storeId, pagingQuery});
        }

        /// <summary>
        /// Opens a file to read.
        /// </summary>
        /// <param name="fileId">ID of the file to read.</param>
        /// <returns>Handle to read file data.</returns>
        public long OpenFile(string fileId)
        {
            return executor.Execute<long>(ptr, (int)StoreApiNative.Method.OpenFile, new List<object>{fileId});
        }

        /// <summary>
        /// Reads file data.
        /// </summary>
        /// <param name="fileHandle">Handle to write file data.</param>
        /// <param name="length">Size of data to read.</param>
        /// <returns>File data chunk.</returns>
        public byte[] ReadFromFile(long fileHandle, long length)
        {
            return executor.Execute<byte[]>(ptr, (int)StoreApiNative.Method.ReadFromFile, new List<object>{fileHandle, length});
        }

        /// <summary>
        /// Moves read cursor.
        /// </summary>
        /// <param name="fileHandle">Handle to write file data.</param>
        /// <param name="position">New cursor position.</param>
        public void SeekInFile(long fileHandle, long position)
        {
            executor.ExecuteVoid(ptr, (int)StoreApiNative.Method.SeekInFile, new List<object>{fileHandle, position});
        }

        /// <summary>
        /// Closes the file handle.
        /// </summary>
        /// <param name="fileHandle">Handle to read/write file data.</param>
        /// <returns>ID of closed file.</returns>
        public string CloseFile(long fileHandle)
        {
            return executor.Execute<string>(ptr, (int)StoreApiNative.Method.CloseFile, new List<object>{fileHandle});
        }

        /// <summary>
        /// Subscribes for the Store module main events.
        /// </summary>
        public void SubscribeForStoreEvents()
        {
            executor.ExecuteVoid(ptr, (int)StoreApiNative.Method.SubscribeForStoreEvents, new List<object>{});
        }

        /// <summary>
        /// Unsubscribes from the Store module main events.
        /// </summary>
        public void UnsubscribeFromStoreEvents()
        {
            executor.ExecuteVoid(ptr, (int)StoreApiNative.Method.UnsubscribeFromStoreEvents, new List<object>{});
        }

        /// <summary>
        /// Subscribes for the events in given Store.
        /// </summary>
        /// <param name="storeId">ID of the Store to subscribe for.</param>
        public void SubscribeForFileEvents(string storeId)
        {
            executor.ExecuteVoid(ptr, (int)StoreApiNative.Method.SubscribeForFileEvents, new List<object>{storeId});
        }

        /// <summary>
        /// Unsubscribes from the events in given Store.
        /// </summary>
        /// <param name="storeId">ID of the Store to unsubscribe from.</param>
        public void UnsubscribeFromFileEvents(string storeId)
        {
            executor.ExecuteVoid(ptr, (int)StoreApiNative.Method.UnsubscribeFromFileEvents, new List<object>{storeId});
        }
    }
}
