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

using System;
using System.Collections.Generic;
using System.Linq;
using Org.Webrtc;
using PrivMX.Endpoint.Stream.Models.StreamApi;
using PrivMX.Endpoint.Stream.Models.StreamApiLow;

namespace PrivMX.Endpoint.Stream
{
    internal class WebRTCImpl : IWebRTC
    {
        private PmxKeyStore keyStore;
        private ITrackObserver trackObserver;
        private PeerConnectionManager peerConnectionManager;
        
        public WebRTCImpl(ITrackObserver trackObserver, PeerConnectionManager peerConnectionManager)
        {
            keyStore = PmxFrameCryptorFactory.CreatePmxKeyStore();
            this.trackObserver = trackObserver;
            this.peerConnectionManager = peerConnectionManager;
        }
        
        public string CreateOfferAndSetLocalDescription(string streamRoomId)
        {
            throw new System.NotImplementedException();
        }

        public string CreateAnswerAndSetDescriptions(string streamRoomId, string sdp, string type)
        {
            throw new NotImplementedException();
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

        public void CreatePeerConnections(string streamRoomId, PeerConnectionManager? peerConnectionFactory)
        {
            Console.WriteLine("createPeerConnection: ");
            List<PeerConnection.IceServer> iceServers = new List<PeerConnection.IceServer>();
            PeerConnection.RTCConfiguration rtcConfiguration = new PeerConnection.RTCConfiguration(iceServers);

            if (peerConnectionManager != null)
            {
                CreateRoomJanusSession(streamRoomId, rtcConfiguration);
            }
        }

        public void AddVideoTrack(string streamRoomId, VideoTrack videoTrack, string id)
        {
            PeerConnection2 pc2 = peerConnectionManager.GetJanusSessions().Values.First().Sender;
            Console.WriteLine("Peerconnection: + " + pc2);
            RtpSender rtpSender = pc2.GetPeerConnection().AddTrack(videoTrack);
            PmxFrameCryptor frameCryptor = PmxFrameCryptorFactory.CreatePmxFrameCryptorFromRtpSender(
                peerConnectionManager.GetPeerConnectionFactory(), rtpSender, keyStore);
            pc2.AddVideoTrack(id, new VideoTrackInfo(videoTrack, rtpSender, frameCryptor));        
        }

        private void CreateRoomJanusSession(string streamRoomId, PeerConnection.RTCConfiguration rtcConfiguration)
        {
            RoomJanusSession janusSession = new RoomJanusSession(peerConnectionManager, streamRoomId, keyStore, 
                trackObserver, rtcConfiguration);
                
            peerConnectionManager.GetJanusSessions().Add(streamRoomId, janusSession);
        }
    }
}

#else

namespace PrivMX.Endpoint.Stream
{
    /// <summary>
    /// This version of WebRTC is not implemented yet.
    /// </summary>
    public class WebRTCImpl
    {
        
    }
}

#endif