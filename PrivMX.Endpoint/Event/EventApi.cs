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
using PrivMX.Endpoint.Event.Internal;
using System;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Event
{
    public class EventApi : IEventApi
    {
        public readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new EventApiNative());

        /// <summary>
        /// Creates an instance of the <see cref="EventApi"/>.
        /// </summary>
        /// <param name="connection">Instance of <see cref="Connection"/></param>
        /// <returns>Created instance of the <see cref="Event"/>.</returns>
        public static EventApi Create(Connection connection)
        {
            EventApi eventApi = new EventApi(connection);
            eventApi.executor.ExecuteVoid(eventApi.ptr, (int)EventApiNative.Method.Create, new List<object>{});
            return eventApi;
        }

        private EventApi(Connection connection)
        {
            EventApiNative.privmx_endpoint_newEventApi(connection.ptr, out ptr);
        }

        ~EventApi()
        {
            EventApiNative.privmx_endpoint_freeEventApi(ptr);
        }

        /// <summary>
        /// Emits the custom event on the given Context and channel.
        /// </summary>
        /// <param name="contextId">ID of the Context.</param>
        /// <param name="users">Array of <see cref="UserWithPubKey"/> which defines the recipients of the event.</param>
        /// <param name="channelName">Name of the Channel.</param>
        /// <param name="eventData">Event's data.</param>
        public void EmitEvent(string contextId, List<UserWithPubKey> users, string channelName, byte[] eventData)
        {
            executor.ExecuteVoid(ptr, (int)EventApiNative.Method.EmitEvent, new List<object>{contextId, users, channelName, eventData});
        }

        /// <summary>
        /// Subscribes for the custom events on the given channel.
        /// </summary>
        /// <param name="threadId">ID of the Thread to subscribe to.</param>
        public void SubscribeForCustomEvents(string contextId, string channelName)
        {
            executor.ExecuteVoid(ptr, (int)EventApiNative.Method.SubscribeForCustomEvents, new List<object>{contextId, channelName});
        }

        /// <summary>
        /// Unsubscribes from the custom events on the given channel.
        /// </summary>
        /// <param name="threadId">ID of the Thread to unsubscribe from.</param>
        public void UnsubscribeFromCustomEvents(string contextId, string channelName)
        {
            executor.ExecuteVoid(ptr, (int)EventApiNative.Method.UnsubscribeFromCustomEvents, new List<object>{contextId, channelName});
        }
    }
}
