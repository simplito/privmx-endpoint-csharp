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
using PrivMX.Endpoint.Thread.Internal;
using PrivMX.Endpoint.Thread.Models;
using System;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Thread
{
    public class ThreadApi : IThreadApi
    {
        public readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new ThreadApiNative());

        /// <summary>
        /// Creates an instance of the <see cref="ThreadApi"/>.
        /// </summary>
        /// <param name="connection">Instance of <see cref="Connection"/></param>
        /// <returns>Created instance of the <see cref="ThreadApi"/>.</returns>
        public static ThreadApi Create(Connection connection)
        {
            ThreadApi threadApi = new ThreadApi(connection);
            threadApi.executor.ExecuteVoid(threadApi.ptr, (int)ThreadApiNative.Method.Create, new List<object>{});
            return threadApi;
        }

        private ThreadApi(Connection connection)
        {
            ThreadApiNative.privmx_endpoint_newThreadApi(connection.ptr, out ptr);
        }

        ~ThreadApi()
        {
            ThreadApiNative.privmx_endpoint_freeThreadApi(ptr);
        }

        /// <summary>
        /// Creates new Thread in given Context.
        /// </summary>
        /// <param name="contextId">ID of the Context to create the Thread in.</param>
        /// <param name="users">Array of <see cref="UserWithPubKey"/> which indicates who will have access to the created Thread.</param>
        /// <param name="managers">Array of <see cref="UserWithPubKey"/> which indicates who will have access (and management rights) to the created Thread.</param>
        /// <param name="publicMeta">Public metadata that will remain unencrypted on the Bridge.</param>
        /// <param name="privateMeta">Private metadata that will be encrypted before being sent to the Bridge.</param>
        /// <param name="policies">(optional) Thread policy.</param>
        /// <returns>ID of the created Thread.</returns>
        public string CreateThread(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, ContainerPolicy policies = null)
        {
            return executor.Execute<string>(ptr, (int)ThreadApiNative.Method.CreateThread, new List<object>{contextId, users, managers, publicMeta, privateMeta, policies});
        }

        /// <summary>
        /// Updates an existing Thread.
        /// </summary>
        /// <param name="threadId">ID of the Thread to update.</param>
        /// <param name="users">Array of <see cref="UserWithPubKey"/> structs which indicates who will have access to the created Thread.</param>
        /// <param name="managers">Array of <see cref="UserWithPubKey"/> structs which indicates who will have access (and management rights) to the created Thread.</param>
        /// <param name="publicMeta">Public metadata that will remain unencrypted on the Bridge.</param>
        /// <param name="privateMeta">Private metadata that will be encrypted before being sent to the Bridge.</param>
        /// <param name="version">Current version of the updated Thread.</param>
        /// <param name="force">Force update (without checking version).</param>
        /// <param name="forceGenerateNewKey">Force to regenerate a key for the Thread.</param>
        /// <param name="policies">(optional) Thread policy.</param>
        public void UpdateThread(string threadId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, long version, bool force, bool forceGenerateNewKey, ContainerPolicy policies = null)
        {
            executor.ExecuteVoid(ptr, (int)ThreadApiNative.Method.UpdateThread, new List<object>{threadId, users, managers, publicMeta, privateMeta, version, force, forceGenerateNewKey, policies});
        }

        /// <summary>
        /// Deletes a Thread by given Thread ID.
        /// </summary>
        /// <param name="threadId">ID of the Thread to delete.</param>
        public void DeleteThread(string threadId)
        {
            executor.ExecuteVoid(ptr, (int)ThreadApiNative.Method.DeleteThread, new List<object>{threadId});
        }

        /// <summary>
        /// Gets a Thread by given Thread ID.
        /// </summary>
        /// <param name="threadId">ID of Thread to get.</param>
        /// <returns>Information about about the Thread.</returns>
        public Models.Thread GetThread(string threadId)
        {
            return executor.Execute<Models.Thread>(ptr, (int)ThreadApiNative.Method.GetThread, new List<object> { threadId });
        }

        /// <summary>
        /// Gets a list of Threads in given Context.
        /// </summary>
        /// <param name="contextId">ID of the Context to get the Threads from.</param>
        /// <param name="pagingQuery">List query parameters.</param>
        /// <returns>List of Threads.</returns>
        public PagingList<Models.Thread> ListThreads(string contextId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<Models.Thread>>(ptr, (int)ThreadApiNative.Method.ListThreads, new List<object>{contextId, pagingQuery});
        }

        /// <summary>
        /// Gets a message by given message ID.
        /// </summary>
        /// <param name="messageId">ID of the message to get.</param>
        /// <returns>Message.</returns>
        public Message GetMessage(string messageId)
        {
            return executor.Execute<Message>(ptr, (int)ThreadApiNative.Method.GetMessage, new List<object>{messageId});
        }

        /// <summary>
        /// Gets a list of messages from a Thread.
        /// </summary>
        /// <param name="threadId">ID of the Thread to list messages from.</param>
        /// <param name="pagingQuery">List query parameters.</param>
        /// <returns>List of messages.</returns>
        public PagingList<Message> ListMessages(string threadId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<Message>>(ptr, (int)ThreadApiNative.Method.ListMessages, new List<object>{threadId, pagingQuery});
        }

        /// <summary>
        /// Sends a message in a Thread.
        /// </summary>
        /// <param name="threadId">ID of the Thread to send message to.</param>
        /// <param name="publicMeta">Public message metadata.</param>
        /// <param name="privateMeta">Private message metadata.</param>
        /// <param name="data">Content of the message.</param>
        /// <returns>ID of the new message.</returns>
        public string SendMessage(string threadId, byte[] publicMeta, byte[] privateMeta, byte[] data)
        {
            return executor.Execute<string>(ptr, (int)ThreadApiNative.Method.SendMessage, new List<object>{threadId, publicMeta, privateMeta, data});
        }

        /// <summary>
        /// Updates a message in a Thread.
        /// </summary>
        /// <param name="messageId">ID of the message to update.</param>
        /// <param name="publicMeta">Public message metadata.</param>
        /// <param name="privateMeta">Private message metadata.</param>
        /// <param name="data">Content of the message.</param>
        public void UpdateMessage(string messageId, byte[] publicMeta, byte[] privateMeta, byte[] data)
        {
            executor.ExecuteVoid(ptr, (int)ThreadApiNative.Method.UpdateMessage, new List<object>{messageId, publicMeta, privateMeta, data});
        }

        /// <summary>
        /// Deletes a message by given message ID.
        /// </summary>
        /// <param name="messageId">ID of the message to delete.</param>
        public void DeleteMessage(string messageId)
        {
            executor.ExecuteVoid(ptr, (int)ThreadApiNative.Method.DeleteMessage, new List<object>{messageId});
        }

        /// <summary>
        /// Subscribes for the Thread module main events.
        /// </summary>
        public void SubscribeForThreadEvents()
        {
            executor.ExecuteVoid(ptr, (int)ThreadApiNative.Method.SubscribeForThreadEvents, new List<object>{});
        }

        /// <summary>
        /// Unsubscribes from the Thread module main events.
        /// </summary>
        public void UnsubscribeFromThreadEvents()
        {
            executor.ExecuteVoid(ptr, (int)ThreadApiNative.Method.UnsubscribeFromThreadEvents, new List<object>{});
        }

        /// <summary>
        /// Subscribes for the events in given Thread.
        /// </summary>
        /// <param name="threadId">ID of the Thread to subscribe for.</param>
        public void SubscribeForMessageEvents(string threadId)
        {
            executor.ExecuteVoid(ptr, (int)ThreadApiNative.Method.SubscribeForMessageEvents, new List<object>{threadId});
        }

        /// <summary>
        /// Unsubscribes from events in given Thread.
        /// </summary>
        /// <param name="threadId">ID of the Thread to unsubscribe from.</param>
        public void UnsubscribeFromMessageEvents(string threadId)
        {
            executor.ExecuteVoid(ptr, (int)ThreadApiNative.Method.UnsubscribeFromMessageEvents, new List<object>{threadId});
        }
    }
}
