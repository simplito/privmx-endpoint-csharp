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

using PrivMX.Endpoint.Core.Internal;
using PrivMX.Endpoint.Core.Models;
using System;
using System.Collections.Generic;
using PrivMX.Endpoint.Core.Models.Events;

namespace PrivMX.Endpoint.Core
{
    public class Connection : IConnection
    {
        public readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new ConnectionNative());

        /// <summary>
        /// Connects to the PrivMX Bridge server.
        /// </summary>
        /// <param name="userPrivKey">User's private key.</param>
        /// <param name="solutionId">ID of the Solution.</param>
        /// <param name="bridgeUrl">PrivMX Bridge URL.</param>
        /// <returns>Created and connected instance of the <see cref="Connection"/>.</returns>
        static public Connection Connect(byte[] userPrivKey, string solutionId, string bridgeUrl)
        {
            Connection connection = new Connection();
            connection.executor.ExecuteVoid(connection.ptr, (int)ConnectionNative.Method.Connect, new List<object?> { userPrivKey, solutionId, bridgeUrl });
            return connection;
        }

        /// <summary>
        /// Connects to the PrivMX Bridge server as a guest user.
        /// </summary>
        /// <param name="solutionId">ID of the Solution.</param>
        /// <param name="bridgeUrl">PrivMX Bridge URL.</param>
        /// <returns>Created and connected instance of the <see cref="Connection"/>.</returns>
        static public Connection ConnectPublic(string solutionId, string bridgeUrl)
        {
            Connection connection = new Connection();
            connection.executor.ExecuteVoid(connection.ptr, (int)ConnectionNative.Method.ConnectPublic, new List<object?> { solutionId, bridgeUrl });
            return connection;
        }

        private Connection()
        {
            ConnectionNative.privmx_endpoint_newConnection(out ptr);
        }

        ~Connection()
        {
            ConnectionNative.privmx_endpoint_freeConnection(ptr);
        }

        /// <summary>
        /// Gets the ID of the current connection.
        /// </summary>
        /// <returns>ID of the connection.</returns>
        public long GetConnectionId()
        {
            return executor.ExecuteValue<long>(ptr, (int)ConnectionNative.Method.GetConnectionId, new List<object?> { });
        }

        /// <summary>
        /// Gets a list of Contexts available for the user.
        /// </summary>
        /// <param name="pagingQuery">List query parameters.</param>
        /// <returns>List of contexts.</returns>
        public PagingList<Context> ListContexts(PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<Context>>(ptr, (int)ConnectionNative.Method.ListContexts, new List<object?> { pagingQuery });
        }

        /// <summary>
        /// Gets a list of Users in a given Context
        /// </summary>
        /// <param name="contextId">ID of a context</param>
        /// <param name="pagingQuery">Paging query</param>
        /// <returns>List of userInfo</returns>
        public PagingList<UserInfo> ListContextUsers(string contextId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<UserInfo>>(ptr, (int)ConnectionNative.Method.ListContextUsers, 
                new List<object?> { contextId, pagingQuery });
        }

        /// <summary>
        /// Subscribe for the Connection events on the given subscription query.
        /// </summary>
        /// <param name="subscriptionQueries">list of queries</param>
        /// <returns>list of subscriptionIds in matching order to subscriptionQueries</returns>
        public List<string> SubscribeFor(List<string> subscriptionQueries)
        {
            return executor.Execute<List<string>>(ptr, (int)ConnectionNative.Method.SubscribeFor,
                new List<object?> { subscriptionQueries });
        }

        /// <summary>
        /// Unsubscribe from events for the given subscriptionId.
        /// </summary>
        /// <param name="subscriptionIds">list of subscriptionId</param>
        public void UnsubscribeFrom(List<string> subscriptionIds)
        {
            executor.ExecuteVoid(ptr, (int)ConnectionNative.Method.UnsubscribeFrom, new List<object?> { subscriptionIds });
        }

        /// <summary>
        /// Generate subscription Query for the Connection events.
        /// </summary>
        /// <param name="eventType">type of event which you listen for</param>
        /// <param name="selectorType">scope on which you listen for events </param>
        /// <param name="selectorId">ID of the selector</param>
        /// <returns>A subscription query as string</returns>
        public string BuildSubscriptionQuery(EventType eventType, EventSelectorType selectorType, string selectorId)
        {
            return executor.Execute<string>(ptr,  (int)ConnectionNative.Method.BuildSubscriptionQuery, 
                new List<object?> { eventType, selectorType, selectorId });
        }

        /// <summary>
        /// Disconnects from the PrivMX Bridge.
        /// </summary>
        public void Disconnect()
        {
            executor.ExecuteVoid(ptr, (int)ConnectionNative.Method.Disconnect, new List<object?> { });
        }

        public void SetUserVerifier(UserVerifierInterface verifier)
        {
            throw new NotImplementedException();
        }
    }
}
