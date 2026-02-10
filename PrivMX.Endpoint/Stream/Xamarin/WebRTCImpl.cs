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
using System.Threading;
using System.Threading.Tasks;
using Org.Webrtc;
using PrivMX.Endpoint.Stream.Models.StreamApiLow;
using PrivMX.Endpoint.Stream.Xamarin.Models;

namespace PrivMX.Endpoint.Stream.Xamarin
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
            Console.WriteLine("CreateOfferAndSetLocalDescription: {0}", streamRoomId);
            var tcs = new TaskCompletionSource<string>();

            PeerConnection2 pc2 = peerConnectionManager.GetJanusSessions().Values.First().Sender;
            pc2.GetPeerConnection().CreateOffer(
                new SdpObserver( tcs, pc2.GetPeerConnection()),
                new MediaConstraints()
            );

            try
            {
                string result = tcs.Task.Result;
                Console.WriteLine("Result: " + result);
                return result;
            }
            catch
            {
                Console.WriteLine("No Result");
                return "";
            }
        }

        public string CreateAnswerAndSetDescriptions(string streamRoomId, string sdp, string type)
        {
            Console.WriteLine("CreateOfferAndSetLocalDescription: {0} {1} {2}", streamRoomId, sdp, type);
            return "res:CreateOfferAndSetLocalDescription";
        }

        public void SetAnswerAndSetRemoteDescription(string streamRoomId, string sdp, string type)
        {
            var tcs = new TaskCompletionSource<string>();
            PeerConnection2 pc2 = peerConnectionManager.GetJanusSessions().Values.First().Sender;
            pc2.GetPeerConnection().SetRemoteDescription(new SdpObserver(tcs, pc2.GetPeerConnection()), new SessionDescription(SessionDescription.Type.Answer, sdp));
            Console.WriteLine("SetAnswerAndSetRemoteDescription: {0} {1} {2}", streamRoomId, sdp, type);
        }

        public void UpdateSessionId(string streamRoomId, long sessionId, string connectionType)
        {
            Console.WriteLine("updateSessionId: {0} {1} {2}", streamRoomId, sessionId, connectionType);
        }

        public void Close(string streamRoomId)
        {
            Console.WriteLine("Close: {0}", streamRoomId);
        }

        public void UpdateKeys(string streamRoomId, List<StreamKey> keys)
        {
            Console.WriteLine("UpdateKeys: {0} {1}", streamRoomId, keys.Count);
            foreach (var key in keys)
            {
                Console.WriteLine("Key: {0} {1} {2}", key.KeyId, key.Key.Length, (int)key.Type);
            }
        }

        public void CreatePeerConnections(string streamRoomId, PeerConnectionManager? peerConnectionFactory)
        {
            List<PeerConnection.IceServer> iceServers = new List<PeerConnection.IceServer>();
            PeerConnection.RTCConfiguration rtcConfiguration = new PeerConnection.RTCConfiguration(iceServers);

            if (peerConnectionManager != null)
            {
                CreateRoomJanusSession(streamRoomId, rtcConfiguration);
                Console.WriteLine("Created janus session ok");
            }
            
            Console.WriteLine("CreatePeerConnection ok");
        }

        public void AddVideoTrack(string streamRoomId, VideoTrack videoTrack, string id)
        {
            Console.WriteLine("Janus sessions: " + peerConnectionManager.GetJanusSessions().Values.Count);
            foreach (var session in peerConnectionManager.GetJanusSessions().Keys)
            {
                Console.WriteLine("Session: " + session);
            }
            
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

#endif