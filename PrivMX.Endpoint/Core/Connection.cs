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
using System.Runtime.InteropServices;
using PrivMX.Endpoint.Core.Models.Events;

namespace PrivMX.Endpoint.Core
{
    /// <summary>
    /// 'Connection' represents and manages the current connection between the Endpoint and the Bridge server.
    /// </summary>
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
        /// <param name="verificationOptions">PrivMX Bridge server instance verification options using a PKI server</param>
        /// <returns>Created and connected instance of the <see cref="Connection"/>.</returns>
        static public Connection Connect(string userPrivKey, string solutionId, string bridgeUrl, PKIVerificationOptions? verificationOptions = null)
        {
            verificationOptions ??= new PKIVerificationOptions();
            
            Connection connection = new Connection();
            connection.executor.ExecuteVoid(connection.ptr, (int)ConnectionNative.Method.Connect, 
                new List<object?> { userPrivKey, solutionId, bridgeUrl, verificationOptions });
            return connection;
        }

        /// <summary>
        /// Connects to the PrivMX Bridge server as a guest user.
        /// </summary>
        /// <param name="solutionId">ID of the Solution.</param>
        /// <param name="bridgeUrl">PrivMX Bridge URL.</param>
        /// <param name="verificationOptions">PrivMX Bridge server instance verification options using a PKI server</param>
        /// <returns>Created and connected instance of the <see cref="Connection"/>.</returns>
        static public Connection ConnectPublic(string solutionId, string bridgeUrl, PKIVerificationOptions? verificationOptions = null)
        {
            verificationOptions ??= new PKIVerificationOptions();
            
            Connection connection = new Connection();
            connection.executor.ExecuteVoid(connection.ptr, (int)ConnectionNative.Method.ConnectPublic, 
                new List<object?> { solutionId, bridgeUrl, verificationOptions });
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

        public static void SetCertsPath(string path)
        {
            IntPtr ptr = Marshal.StringToHGlobalAnsi(path);
            ConnectionNative.privmx_endpoint_setCertsPath(ptr);
            Marshal.FreeHGlobal(ptr);
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
        /// Gets a list of users with their status and the last status change.
        /// </summary>
        /// <param name="contextId">ID of the Context</param>
        /// <param name="pagingQuery">pagingQuery struct with list query parameters</param>
        /// <returns>List of users with their status and the last status change</returns>
        public PagingList<UserInfo> ListContextUsers(string contextId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<UserInfo>>(ptr, (int)ConnectionNative.Method.ListContextUsers, new List<object?> { contextId, pagingQuery });
        }

        /// <summary>
        /// Subscribe for the Context events on the given subscription query.
        /// </summary>
        /// <param name="subscriptionQueries">subscriptionQueries List of queries</param>
        /// <returns>List of subscriptionIds in matching order to subscriptionQueries</returns>
        public List<string> SubscribeFor(List<string> subscriptionQueries)
        {
            return executor.Execute<List<string>>(ptr, (int)ConnectionNative.Method.SubscribeFor, new List<object?> { subscriptionQueries });
        }

        /// <summary>
        /// Unsubscribe from events for the given subscriptionId.
        /// </summary>
        /// <param name="subscriptionIds">subscriptionIds List of subscriptionId</param>
        public void UnsubscribeFrom(List<string> subscriptionIds)
        {
            executor.ExecuteVoid(ptr, (int)ConnectionNative.Method.UnsubscribeFrom, new List<object?> { subscriptionIds });
        }

        /// <summary>
        /// Generate subscription Query for the Context events.
        /// </summary>
        /// <param name="eventType">type of event which you listen for</param>
        /// <param name="selectorType">scope on which you listen for events</param>
        /// <param name="selectorId">ID of the selector</param>
        /// <returns>Subscription query string</returns>
        public string BuildSubscriptionQuery(EventType eventType, EventSelectorType selectorType, string selectorId)
        {
            return executor.Execute<string>(ptr, (int)ConnectionNative.Method.BuildSubscriptionQuery, new List<object?> { eventType, selectorType, selectorId });
        }

        /// <summary>
        /// Disconnects from the PrivMX Bridge.
        /// </summary>
        public void Disconnect()
        {
            executor.ExecuteVoid(ptr, (int)ConnectionNative.Method.Disconnect, new List<object?> { });
        }

        /// <summary>
        /// !!Not implemented Yet - Sets user's custom verification callback.
        /// </summary>
        /// <param name="verifier"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetUserVerifier(UserVerifierInterface verifier)
        {
            executor.ExecuteVoid(ptr, (int)ConnectionNative.Method.SetUserVerifier, new List<object?> { verifier });
        }
    }
}
