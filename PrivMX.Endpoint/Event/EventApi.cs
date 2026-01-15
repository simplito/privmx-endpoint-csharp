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
using EventSelectorType = PrivMX.Endpoint.Event.Models.EventSelectorType;

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
            eventApi.executor.ExecuteVoid(eventApi.ptr, (int)EventApiNative.Method.Create, new List<object?>{connection});
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
            executor.ExecuteVoid(ptr, (int)EventApiNative.Method.EmitEvent, new List<object?>{contextId, users, channelName, eventData});
        }

        /// <summary>
        /// Subscribe for the custom events on the given subscription query.
        /// </summary>
        /// <param name="subscriptionQueries">list of queries</param>
        /// <returns>List of subscriptionIds in matching order to subscriptionQueries</returns>
        public List<string> SubscribeFor(List<string> subscriptionQueries)
        {
            return executor.Execute<List<string>>(ptr, (int)EventApiNative.Method.SubscribeFor, new List<object?>{subscriptionQueries});
        }

        /// <summary>
        /// Unsubscribe from events for the given subscriptionId.
        /// </summary>
        /// <param name="subscriptionIds">List of subscriptionId</param>
        public void UnsubscribeFrom(List<string> subscriptionIds)
        {
            executor.ExecuteVoid(ptr, (int)EventApiNative.Method.UnsubscribeFrom, new List<object?>{subscriptionIds});
        }

        /// <summary>
        /// Generate subscription Query for the custom events.
        /// </summary>
        /// <param name="channelName">Name of the Channel</param>
        /// <param name="selectorType">Selector of scope on which you listen for events </param>
        /// <param name="selectorId">ID of the selector</param>
        /// <returns>A subscription query as string</returns>
        public string BuildSubscriptionQuery(string channelName, EventSelectorType selectorType, string selectorId)
        {
            return executor.Execute<string>(ptr, (int)EventApiNative.Method.BuildSubscriptionQuery, new List<object?>{channelName, selectorType, selectorId});
        }
    }
}
