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
using Org.Webrtc;

namespace PrivMX.Endpoint.Stream.Xamarin.Models
{
    internal class RoomJanusSession
    {
        private PeerConnection2? sender;
        private PeerConnection2? receiver;
        
        private readonly PeerConnectionManager peerConnectionManager;
        private readonly string streamRoomId;
        private readonly PmxKeyStore keyStore;
        private readonly ITrackObserver trackObserver;
        private readonly PeerConnection.RTCConfiguration rtcConfiguration;

        public RoomJanusSession(PeerConnectionManager peerConnectionManager, string streamRoomId, PmxKeyStore keyStore, 
            ITrackObserver trackObserver, PeerConnection.RTCConfiguration rtcConfiguration)
        {
            this.peerConnectionManager = peerConnectionManager;
            this.streamRoomId = streamRoomId;
            this.keyStore = keyStore;
            this.trackObserver = trackObserver;
            this.rtcConfiguration = rtcConfiguration;
        }
        
        public PeerConnection2 Sender
        {
            get
            {
                if (sender is null)
                {
                    Console.WriteLine("Sender is null");
                    PmxFrameCryptor.PmxFrameCryptorOptions options = new PmxFrameCryptor.PmxFrameCryptorOptions();
                    Console.WriteLine("Options set");
                    PcObserver senderObserver = new PcObserver(peerConnectionManager, streamRoomId, keyStore, 
                        options, trackObserver);

                    Console.WriteLine("Setting sender...");
                    sender = new PeerConnection2(peerConnectionManager.GetPeerConnectionFactory()
                        .CreatePeerConnection(rtcConfiguration, senderObserver), senderObserver, keyStore);
                    
                    Console.WriteLine("createPeerConnection (sender): " + sender);
                }
                return sender;
            }
        }

        public PeerConnection2 Receiver
        {
            get
            {
                if (receiver is null)
                {
                    PcObserver receiverObserver = new PcObserver(peerConnectionManager, streamRoomId, keyStore, 
                        new PmxFrameCryptor.PmxFrameCryptorOptions(), trackObserver);

                    receiver = new PeerConnection2(peerConnectionManager.GetPeerConnectionFactory()
                        .CreatePeerConnection(rtcConfiguration, receiverObserver), receiverObserver, keyStore);
            
                    Console.WriteLine("createPeerConnection: (receiver)" + receiver);
                }
                return receiver;
            }
        }
    }
}

#endif