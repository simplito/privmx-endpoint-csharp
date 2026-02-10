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
using PrivMX.Endpoint.Stream.Models.StreamApiLow;

namespace PrivMX.Endpoint.Stream
{
    public interface IWebRTC
    {
        string CreateOfferAndSetLocalDescription(string streamRoomId);
        string CreateAnswerAndSetDescriptions(string streamRoomId, string sdp, string type);
        void SetAnswerAndSetRemoteDescription(string streamRoomId, string sdp, string type);
        void UpdateSessionId(string streamRoomId, long sessionId, string connectionType);
        void Close(string streamRoomId);
        void UpdateKeys(string streamRoomId, List<StreamKey> keys);
    }
}