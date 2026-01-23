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

#if ANDROID

using Org.Webrtc;

namespace PrivMX.Endpoint.Stream.Models.WebRTC
{
    public class PeerConnection2
    {
        public PeerConnection peerConnection;
        public PmxKeyStore keys;
    }
}

#endif