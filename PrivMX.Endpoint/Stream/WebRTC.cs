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
using System.Collections.Generic;
using Org.Webrtc;
using Org.Webrtc.Audio;
using PrivMX.Endpoint.Stream.Models;
using PrivMX.Endpoint.Stream.Models.WebRTC;

namespace PrivMX.Endpoint.Stream
{
    public class WebRTC : IWebRTC
    {
        private PmxKeyStore store;
        private PeerConnection2 peerConnection2;
        
        public WebRTC()
        {
            store = PmxFrameCryptorFactory.CreatePmxKeyStore();
            peerConnection2 = new PeerConnection2();
        }
        
        public string CreateOfferAndSetLocalDescription(string streamRoomId)
        {
            throw new System.NotImplementedException();
        }

        public string CreateAnswerAndSetDescription(string streamRoomId, string sdp, string type)
        {
            throw new System.NotImplementedException();
        }

        public void SetAnswerAndSetRemoteDescription(string streamRoomId, string sdp, string type)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateSessionId(string streamRoomId, long sessionId, string connectionType)
        {
            throw new System.NotImplementedException();
        }

        public void Close(string streamRoomId)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateKeys(string streamRoomId, List<Key> keys)
        {
            throw new System.NotImplementedException();
        }
    }
}
#else

namespace PrivMX.Endpoint.Stream
{
    /// <summary>
    /// This version of WebRTC is not implemented yet.
    /// </summary>
    public class WebRTC
    {
        
    }
}

#endif