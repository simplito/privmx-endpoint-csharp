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
    public interface IStreamApiLow
    {
        List<TurnCredentials> GetTurnCredentials();
        string CreateStreamRoom(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers, 
            byte[] publicMeta, byte[] privateMeta, ContainerPolicy? policies = null);
        void UpdateStreamRoom(string streamRoomId, List<UserWithPubKey> users, List<UserWithPubKey> managers, 
            byte[] publicMeta, byte[] privateMeta, long version, bool force, bool forceGenerateNewKey, 
            ContainerPolicy? policies = null);
        PagingList<StreamRoom> ListStreamRooms(string contextId, PagingQuery pagingQuery);
        StreamRoom GetStreamRoom(string streamRoomId);
        void DeleteStreamRoom(string streamRoomId);
        List<StreamInfo> ListStreams(string streamRoomId);
        void JoinStreamRoom(string streamRoomId, IWebRTC webRtc);
        void LeaveStreamRoom(string streamRoomId);
        long CreateStream(string streamRoomId);
        StreamPublishResult PublishStream(long streamHandle);
        StreamPublishResult UpdateStream(long streamHandle);
        void UnPublishStream(long streamHandle);
        void SubscribeToRemoteStreams(string streamRoomId, List<StreamSubscription> subscriptions, Settings options);
        void ModifyRemoteStreamsSubscriptions(string streamRoomId, List<StreamSubscription> subscriptionsToAdd, 
            List<StreamSubscription> subscriptionsToRemove, Settings options);
        void UnsubscribeFromRemoteStreams(string streamRoomId, List<StreamSubscription> subscriptionsToRemove);
        void Trickle(long sessionId, string candidateAsJson);
        void AcceptOfferOnReconfigure(long sessionId, SdpWithTypeModel sdp);
        List<string> SubscribeFor(List<string> subscriptionQueries);
        void UnsubscribeFrom(List<string> subscriptionIds);
        string BuildSubscriptionQuery(EventType eventType, EventSelectorType selectorType, string selectorId);
        void KeyManagement(string streamRoomId, bool disable);
    }
}