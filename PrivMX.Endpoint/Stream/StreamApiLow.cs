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
using PrivMX.Endpoint.Event;
using PrivMX.Endpoint.Stream.Internal;
using PrivMX.Endpoint.Stream.Models;
using PrivMX.Endpoint.Stream.Models.Events;

namespace PrivMX.Endpoint.Stream
{
    public class StreamApiLow : IStreamApiLow
    {
        public readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new StreamApiNative());

        public static StreamApiLow Create(Connection connection, EventApi eventApi)
        {
            StreamApiLow streamApiLow = new StreamApiLow(connection, eventApi);
            streamApiLow.executor.ExecuteVoid(streamApiLow.ptr, (int)StreamApiNative.Method.Create, new List<object?>{});
            return streamApiLow;
        }

        private StreamApiLow(Connection connection, EventApi eventApi)
        {
            StreamApiNative.privmx_endpoint_newStreamApi(connection.ptr, eventApi.ptr, out ptr);
        }

        ~StreamApiLow()
        {
            StreamApiNative.privmx_endpoint_freeStreamApi(ptr);
        }
        
        public List<TurnCredentials> GetTurnCredentials()
        {
            return executor.Execute<List<TurnCredentials>>(ptr, (int)StreamApiNative.Method.GetTurnCredentials, 
                new List<object?>{});
        }

        public string CreateStreamRoom(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers, 
            byte[] publicMeta, byte[] privateMeta, ContainerPolicy? policies = null)
        {
            return executor.Execute<string>(ptr, (int)StreamApiNative.Method.CreateStreamRoom, 
                new List<object?> { contextId, users, managers, publicMeta, privateMeta, policies });
        }

        public void UpdateStreamRoom(string streamRoomId, List<UserWithPubKey> users, List<UserWithPubKey> managers, 
            byte[] publicMeta, byte[] privateMeta, long version, bool force, bool forceGenerateNewKey, 
            ContainerPolicy? policies = null)
        {
            executor.ExecuteVoid(ptr,  (int)StreamApiNative.Method.UpdateStreamRoom, 
                new List<object?>{ streamRoomId, users, managers, publicMeta, privateMeta, version, force, 
                    forceGenerateNewKey, policies });
        }

        public PagingList<StreamRoom> ListStreamRooms(string contextId, PagingQuery pagingQuery)
        {
            return executor.Execute<PagingList<StreamRoom>>(ptr,  (int)StreamApiNative.Method.ListStreamRooms,
                new List<object?> { contextId, pagingQuery });
        }

        public StreamRoom GetStreamRoom(string streamRoomId)
        {
            return executor.Execute<StreamRoom>(ptr,  (int)StreamApiNative.Method.GetStreamRoom, 
                new List<object?> { streamRoomId });
        }

        public void DeleteStreamRoom(string streamRoomId)
        {
            executor.ExecuteVoid(ptr,   (int)StreamApiNative.Method.DeleteStreamRoom, new List<object?> {streamRoomId});
        }

        public List<StreamInfo> ListStreams(string streamRoomId)
        {
            return executor.Execute<List<StreamInfo>>(ptr, (int)StreamApiNative.Method.ListStreams,
                new List<object?> { streamRoomId });
        }

        public void JoinStreamRoom(string streamRoomId, IWebRTC webRtc)
        {
            executor.ExecuteVoid(ptr, (int)StreamApiNative.Method.JoinStreamRoom, 
                new List<object?> { streamRoomId, webRtc });
        }
        
        public void LeaveStreamRoom(string streamRoomId)
        {
            executor.ExecuteVoid(ptr, (int)StreamApiNative.Method.LeaveStreamRoom, new List<object?> { streamRoomId });
        }

        public long CreateStream(string streamRoomId)
        {
            return executor.ExecuteValue<long>(ptr, (int)StreamApiNative.Method.CreateStream, 
                new List<object?> { streamRoomId });
        }

        public StreamPublishResult PublishStream(long streamHandle)
        {
            return executor.Execute<StreamPublishResult>(ptr, (int)StreamApiNative.Method.PublishStream,
                new List<object?> { streamHandle });
        }

        public StreamPublishResult UpdateStream(long streamHandle)
        {
            return executor.Execute<StreamPublishResult>(ptr, (int)StreamApiNative.Method.UpdateStream,
                new List<object?> { streamHandle });
        }

        public void UnPublishStream(long streamHandle)
        {
            executor.ExecuteVoid(ptr, (int)StreamApiNative.Method.UnpublishStream, new List<object?> { streamHandle });
        }

        public void SubscribeToRemoteStreams(string streamRoomId, List<StreamSubscription> subscriptions, Settings options)
        {
            executor.ExecuteVoid(ptr, (int)StreamApiNative.Method.SubscribeToRemoteStreams, 
                new List<object?>{streamRoomId, subscriptions, options});
        }

        public void ModifyRemoteStreamsSubscriptions(string streamRoomId, List<StreamSubscription> subscriptionsToAdd, List<StreamSubscription> subscriptionsToRemove,
            Settings options)
        {
            executor.ExecuteVoid(ptr, (int)StreamApiNative.Method.ModifyRemoteStreamsSubscriptions, 
                new List<object?>{ streamRoomId, subscriptionsToAdd, subscriptionsToRemove});
        }

        public void UnsubscribeFromRemoteStreams(string streamRoomId, List<StreamSubscription> subscriptionsToRemove)
        {
            executor.ExecuteVoid(ptr, (int)StreamApiNative.Method.UnsubscribeFromRemoteStreams, 
                new List<object?>{streamRoomId, subscriptionsToRemove});
        }

        public void Trickle(long sessionId, string candidateAsJson)
        {
            executor.ExecuteVoid(ptr,  (int)StreamApiNative.Method.Trickle, 
                new List<object?>{sessionId, candidateAsJson});
        }

        public void AcceptOfferOnReconfigure(long sessionId, SdpWithTypeModel sdp)
        {
            executor.ExecuteVoid(ptr,   (int)StreamApiNative.Method.AcceptOfferOnReconfigure, 
                new List<object?>{sessionId, sdp});
        }

        public List<string> SubscribeFor(List<string> subscriptionQueries)
        {
            return executor.Execute<List<string>>(ptr, (int)StreamApiNative.Method.SubscribeFor,
                new List<object?> { subscriptionQueries });
        }

        public void UnsubscribeFrom(List<string> subscriptionIds)
        {
            executor.ExecuteVoid(ptr, (int)StreamApiNative.Method.UnsubscribeFrom, 
                new List<object?>{ subscriptionIds });
        }

        public string BuildSubscriptionQuery(EventType eventType, EventSelectorType selectorType, string selectorId)
        {
            return executor.Execute<string>(ptr, (int)StreamApiNative.Method.BuildSubscriptionQuery,
                new List<object?> { eventType, selectorType, selectorId });
        }

        public void KeyManagement(string streamRoomId, bool disable)
        {
            executor.ExecuteVoid(ptr,   (int)StreamApiNative.Method.KeyManagement, 
                new List<object?>{streamRoomId, disable});
        }
    }
}