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

using System.Collections.Generic;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Stream.Models;
using PrivMX.Endpoint.Stream.Models.Events;

namespace PrivMX.Endpoint.Stream
{
    public class StreamApi : IStreamApi
    {
        public List<TurnCredentials> GetTurnCredentials()
        {
            throw new System.NotImplementedException();
        }

        public string CreateStreamRoom(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta,
            ContainerPolicy? policies = null)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateStreamRoom(string streamRoomId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta,
            long version, bool force, bool forceGenerateNewKey, ContainerPolicy? policies = null)
        {
            throw new System.NotImplementedException();
        }

        public PagingList<StreamRoom> ListStreamRooms(string contextId, PagingQuery pagingQuery)
        {
            throw new System.NotImplementedException();
        }

        public StreamRoom GetStreamRoom(string streamRoomId)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteStreamRoom(string streamRoomId)
        {
            throw new System.NotImplementedException();
        }

        public List<StreamInfo> ListStreams(string streamRoomId)
        {
            throw new System.NotImplementedException();
        }

        public void JoinStreamRoom(string streamRoomId, WebRTCInterface webRtc)
        {
            throw new System.NotImplementedException();
        }

        public void LeaveStreamRoom(string streamRoomId)
        {
            throw new System.NotImplementedException();
        }

        public long CreateStream(string streamRoomId)
        {
            throw new System.NotImplementedException();
        }

        public StreamPublishResult PublishStream(long streamHandle)
        {
            throw new System.NotImplementedException();
        }

        public StreamPublishResult UpdateStream(long streamHandle)
        {
            throw new System.NotImplementedException();
        }

        public void UnPublishStream(long streamHandle)
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeToRemoteStreams(string streamRoomId, List<StreamSubscription> subscriptions, Settings options)
        {
            throw new System.NotImplementedException();
        }

        public void ModifyRemoteStreamsSubscriptions(string streamRoomId, List<StreamSubscription> subscriptionsToAdd, List<StreamSubscription> subscriptionsToRemove,
            Settings options)
        {
            throw new System.NotImplementedException();
        }

        public void UnsubscribeFromRemoteStreams(string streamRoomId, List<StreamSubscription> subscriptionsToRemove)
        {
            throw new System.NotImplementedException();
        }

        public void Trickle(long sessionId, string candidateAsJson)
        {
            throw new System.NotImplementedException();
        }

        public void AcceptOfferOnReconfigure(long sessionId, SdpWithTypeModel sdp)
        {
            throw new System.NotImplementedException();
        }

        public List<string> SubscribeFor(List<string> subscriptionQueries)
        {
            throw new System.NotImplementedException();
        }

        public void UnsubscribeFrom(List<string> subscriptionIds)
        {
            throw new System.NotImplementedException();
        }

        public string BuildSubscriptionQuery(EventType eventType, EventSelectorType selectorType, string selectorId)
        {
            throw new System.NotImplementedException();
        }

        public void KeyManagement(string streamRoomId, bool disable)
        {
            throw new System.NotImplementedException();
        }
    }
}