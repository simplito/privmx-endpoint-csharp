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
        static public Connection Connect(string userPrivKey, string solutionId, string bridgeUrl)
        {
            Connection connection = new Connection();
            connection.executor.ExecuteVoid(connection.ptr, (int)ConnectionNative.Method.Connect, new List<object> { userPrivKey, solutionId, bridgeUrl });
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
            connection.executor.ExecuteVoid(connection.ptr, (int)ConnectionNative.Method.ConnectPublic, new List<object> { solutionId, bridgeUrl });
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
            return executor.Execute<long>(ptr, (int)ConnectionNative.Method.GetConnectionId, new List<object> { });
        }

        /// <summary>
        /// Gets a list of Contexts available for the user.
        /// </summary>
        /// <param name="pagingQuery">List query parameters.</param>
        /// <returns>List of contexts.</returns>
        public PagingList<Context> ListContexts(PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<Context>>(ptr, (int)ConnectionNative.Method.ListContexts, new List<object> { pagingQuery });
        }

        /// <summary>
        /// Disconnects from the PrivMX Bridge.
        /// </summary>
        public void Disconnect()
        {
            executor.ExecuteVoid(ptr, (int)ConnectionNative.Method.Disconnect, new List<object> { });
        }
    }
}
