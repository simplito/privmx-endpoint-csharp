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
using PrivMX.Endpoint.Inbox.Internal;
using PrivMX.Endpoint.Inbox.Models;
using PrivMX.Endpoint.Store;
using PrivMX.Endpoint.Thread;
using System;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Inbox
{
    public class InboxApi : IInboxApi
    {
        private readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new InboxApiNative());

        /// <summary>
        /// Creates an instance of the <see cref="InboxApi"/>.
        /// </summary>
        /// <param name="connection">Instance of <see cref="Connection"/></param>
        /// <param name="threadApi">Instance of <see cref="ThreadApi"/></param>
        /// <param name="storeApi">Instance of <see cref="StoreApi"/></param>
        /// <returns>Created instance of the <see cref="InboxApi"/>.</returns>
        static public InboxApi Create(Connection connection, ThreadApi threadApi, StoreApi storeApi)
        {
            InboxApi inboxApi = new InboxApi(connection, threadApi, storeApi);
            inboxApi.executor.ExecuteVoid(inboxApi.ptr, (int)InboxApiNative.Method.Create, new List<object>{});
            return inboxApi;
        }

        private InboxApi(Connection connection, ThreadApi threadApi, StoreApi storeApi)
        {
            InboxApiNative.privmx_endpoint_newInboxApi(connection.ptr, threadApi.ptr, storeApi.ptr, out ptr);
        }

        ~InboxApi()
        {
            InboxApiNative.privmx_endpoint_freeInboxApi(ptr);
        }

        /// <summary>
        /// Creates a new Inbox.
        /// </summary>
        /// <param name="contextId">ID of the Context of the new Inbox.</param>
        /// <param name="users">Vector of UserWithPubKey structs which indicates who will have access to the created Inbox.</param>
        /// <param name="managers">Vector of UserWithPubKey structs which indicates who will have access (and management rights) to the created Inbox.</param>
        /// <param name="publicMeta">Public (unencrypted) metadata.</param>
        /// <param name="privateMeta">Private (encrypted) metadata.</param>
        /// <param name="filesConfig">(optional) Configuration of files.</param>
        /// <param name="policies">(optional) Inbox policy.</param>
        /// <returns>ID of the created Inbox.</returns>
        public string CreateInbox(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, FilesConfig filesConfig, ContainerPolicyWithoutItem policies = null)
        {
            return executor.Execute<string>(ptr, (int)InboxApiNative.Method.CreateInbox, new List<object>{contextId, users, managers, publicMeta, privateMeta, filesConfig, policies});
        }

        /// <summary>
        /// Updates an existing Inbox.
        /// </summary>
        /// <param name="inboxId">ID of the Inbox to update.</param>
        /// <param name="users">Vector of UserWithPubKey structs which indicates who will have access to the created Inbox.</param>
        /// <param name="managers">Vector of UserWithPubKey structs which indicates who will have access (and management rights) to the created Inbox.</param>
        /// <param name="publicMeta">Public (unencrypted) metadata.</param>
        /// <param name="privateMeta">Private (encrypted) metadata.</param>
        /// <param name="filesConfig">(optional) Configuration of files.</param>
        /// <param name="version">Current version of the updated Inbox.</param>
        /// <param name="force">Force update without checking version.</param>
        /// <param name="forceGenerateNewKey">Force to regenerate a key for the Inbox.</param>
        /// <param name="policies">(optional) Inbox policy.</param>
        public void UpdateInbox(string inboxId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, FilesConfig filesConfig, long version, bool force, bool forceGenerateNewKey, ContainerPolicyWithoutItem policies = null)
        {
            executor.ExecuteVoid(ptr, (int)InboxApiNative.Method.UpdateInbox, new List<object>{inboxId, users, managers, publicMeta, privateMeta, filesConfig, version, force, forceGenerateNewKey, policies});
        }

        /// <summary>
        /// Gets a Inbox by given Inbox ID.
        /// </summary>
        /// <param name="inboxId">ID of the Inbox to get.</param>
        /// <returns>Information about about the Inbox.</returns>
        public Models.Inbox GetInbox(string inboxId)
        {
            return executor.Execute<Models.Inbox>(ptr, (int)InboxApiNative.Method.GetInbox, new List<object>{inboxId});
        }

        /// <summary>
        /// Gets s list of Inboxes in given Context.
        /// </summary>
        /// <param name="contextId">ID of the Context to get Inboxes from.</param>
        /// <param name="pagingQuery">List query parameters.</param>
        /// <returns>List of Inboxes.</returns>
        public PagingList<Models.Inbox> ListInboxes(string contextId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<Models.Inbox>>(ptr, (int)InboxApiNative.Method.ListInboxes, new List<object>{contextId, pagingQuery});
        }

        /// <summary>
        /// Gets public data of an Inbox.
        /// You do not have to be logged in to call this function.
        /// </summary>
        /// <param name="inboxId">ID of the Inbox to get.</param>
        /// <returns>Public accessible information about the Inbox.</returns>
        public InboxPublicView GetInboxPublicView(string inboxId)
        {
            return executor.Execute<InboxPublicView>(ptr, (int)InboxApiNative.Method.GetInboxPublicView, new List<object>{inboxId});
        }

        /// <summary>
        /// Deletes an Inbox by given Inbox ID.
        /// </summary>
        /// <param name="inboxId">ID of the Inbox to delete.</param>
        public void DeleteInbox(string inboxId)
        {
            executor.ExecuteVoid(ptr, (int)InboxApiNative.Method.DeleteInbox, new List<object>{inboxId});
        }

        /// <summary>
        /// Prepares a request to send data to an Inbox.
        /// 
        /// You do not have to be logged in to call this function.
        /// </summary>
        /// <param name="inboxId">ID of the Inbox to which the request applies.</param>
        /// <param name="data">Entry data to send.</param>
        /// <param name="inboxFileHandles">(optional) List of file handles that will be sent with the request.</param>
        /// <param name="userPrivKey">(optional) Sender's private key which can be used later to encrypt data for that sender.</param>
        /// <returns>Inbox handle.</returns>
        public long PrepareEntry(string inboxId, byte[] data, List<long> inboxFileHandles, string userPrivKey)
        {
            return executor.Execute<long>(ptr, (int)InboxApiNative.Method.PrepareEntry, new List<object>{inboxId, data, inboxFileHandles, userPrivKey});
        }

        /// <summary>
        /// Sends data to an Inbox.
        /// 
        /// You do not have to be logged in to call this function.
        /// </summary>
        /// <param name="inboxHandle">ID of the Inbox to which the request applies.</param>
        public void SendEntry(long inboxHandle)
        {
            executor.ExecuteVoid(ptr, (int)InboxApiNative.Method.SendEntry, new List<object>{inboxHandle});
        }

        /// <summary>
        /// Gets an entry from an Inbox.
        /// </summary>
        /// <param name="inboxEntryId">ID of an entry to read from the Inbox.</param>
        /// <returns>Data of the entry stored in the Inbox.</returns>
        public InboxEntry ReadEntry(string inboxEntryId)
        {
            return executor.Execute<InboxEntry>(ptr, (int)InboxApiNative.Method.ReadEntry, new List<object>{inboxEntryId});
        }

        /// <summary>
        /// Gets list of entries in given Inbox.
        /// </summary>
        /// <param name="inboxId">ID of the Inbox.</param>
        /// <param name="pagingQuery">List query parameters.</param>
        /// <returns>List of entries.</returns>
        public PagingList<InboxEntry> ListEntries(string inboxId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<InboxEntry>>(ptr, (int)InboxApiNative.Method.ListEntries, new List<object>{inboxId, pagingQuery});
        }

        /// <summary>
        /// Delete an entry from an Inbox.
        /// </summary>
        /// <param name="inboxEntryId">ID of an entry to delete.</param>
        public void DeleteEntry(string inboxEntryId)
        {
            executor.ExecuteVoid(ptr, (int)InboxApiNative.Method.DeleteEntry, new List<object>{inboxEntryId});
        }

        /// <summary>
        /// Creates a file handle to send a file to an Inbox.
        /// </summary>
        /// <param name="publicMeta">Public file metadata.</param>
        /// <param name="privateMeta">Private file metadata.</param>
        /// <param name="fileSize">Size of the file to send.</param>
        /// <returns>File handle.</returns>
        public long CreateFileHandle(byte[] publicMeta, byte[] privateMeta, long fileSize)
        {
            return executor.Execute<long>(ptr, (int)InboxApiNative.Method.CreateFileHandle, new List<object>{publicMeta, privateMeta, fileSize});
        }

        /// <summary>
        /// Sends a file's data chunk to an Inbox.
        /// 
        /// To send the entire file - divide it into pieces of the desired size and call the function for each fragment.
        /// 
        /// You do not have to be logged in to call this function.
        /// </summary>
        /// <param name="inboxHandle">ID of the Inbox to which the request applies.</param>
        /// <param name="inboxFileHandle">Handle to the file which the uploaded chunk belongs.</param>
        /// <param name="dataChunk">File chunk to send.</param>
        public void WriteToFile(long inboxHandle, long inboxFileHandle, byte[] dataChunk)
        {
            executor.ExecuteVoid(ptr, (int)InboxApiNative.Method.WriteToFile, new List<object>{inboxHandle, inboxFileHandle, dataChunk});
        }

        /// <summary>
        /// Opens a file to read.
        /// </summary>
        /// <param name="fileId">ID of the file to read.</param>
        /// <returns>Handle to read file data.</returns>
        public long OpenFile(string fileId)
        {
            return executor.Execute<long>(ptr, (int)InboxApiNative.Method.OpenFile, new List<object>{fileId});
        }

        /// <summary>
        /// Reads file data.
        /// </summary>
        /// <param name="fileHandle">Handle to the open file.</param>
        /// <param name="length">Size of data chunk to read.</param>
        /// <returns>File data chunk which size is equal to length, or smaller size when is end of file.</returns>
        public byte[] ReadFromFile(long fileHandle, long length)
        {
            return executor.Execute<byte[]>(ptr, (int)InboxApiNative.Method.ReadFromFile, new List<object>{fileHandle, length});
        }

        /// <summary>
        ///  Moves file's read cursor.
        /// </summary>
        /// <param name="fileHandle">Handle to the file.</param>
        /// <param name="position">New position of the cursor.</param>
        public void SeekInFile(long fileHandle, long position)
        {
            executor.ExecuteVoid(ptr, (int)InboxApiNative.Method.SeekInFile, new List<object>{fileHandle, position});
        }

        /// <summary>
        /// Closes a file by given handle.
        /// </summary>
        /// <param name="fileHandle">Handle to the file.</param>
        /// <returns>ID of closed file.</returns>
        public string CloseFile(long fileHandle)
        {
            return executor.Execute<string>(ptr, (int)InboxApiNative.Method.CloseFile, new List<object>{fileHandle});
        }

        /// <summary>
        /// Subscribes for the Inbox module main events.
        /// </summary>
        public void SubscribeForInboxEvents()
        {
            executor.ExecuteVoid(ptr, (int)InboxApiNative.Method.SubscribeForInboxEvents, new List<object>{});
        }

        /// <summary>
        /// Unsubscribes from the Inbox module main events.
        /// </summary>
        public void UnsubscribeFromInboxEvents()
        {
            executor.ExecuteVoid(ptr, (int)InboxApiNative.Method.UnsubscribeFromInboxEvents, new List<object>{});
        }

        /// <summary>
        /// Subscribes for events in given Inbox.
        /// </summary>
        /// <param name="inboxId">ID of the Inbox.</param>
        public void SubscribeForEntryEvents(string inboxId)
        {
            executor.ExecuteVoid(ptr, (int)InboxApiNative.Method.SubscribeForEntryEvents, new List<object>{inboxId});
        }

        /// <summary>
        /// Unsubscribes from events in given Inbox.
        /// </summary>
        /// <param name="inboxId">ID of the Inbox.</param>
        public void UnsubscribeFromEntryEvents(string inboxId)
        {
            executor.ExecuteVoid(ptr, (int)InboxApiNative.Method.UnsubscribeFromEntryEvents, new List<object>{inboxId});
        }
    }
}
