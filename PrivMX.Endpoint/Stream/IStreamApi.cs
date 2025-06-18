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

namespace PrivMX.Endpoint.Stream
{
    public interface IStreamApi
    {
        string CreateStreamRoom(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, ContainerPolicy? policies = null);
        void UpdateStreamRoom(string streamRoomId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, long version, bool force, bool forceGenerateNewKey, ContainerPolicy? policies = null);
        void DeleteStreamRoom(string streamRoomId);
        StreamRoom GetStreamRoom(string streamRoomId);
        PagingList<StreamRoom> ListStreamRooms(string contextId, PagingQuery pagingQuery);
        long CreateStream(string streamRoomId);
        void AddTrack(long streamId, TrackType type, string? parameters = null);
        void PublishStream(long streamId);
        void StreamTrackSendData(long streamId, byte[] data);
        long JoinStream(string streamRoomId, string? settings = null);
        void StreamTrackRecvData(long streamId);
        void UnpublishStream(long streamId);
        void LeaveStream(long streamId);
    }
}
