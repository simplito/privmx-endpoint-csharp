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
using System.Threading.Tasks;
using Android.Runtime;
using Java.Interop;
using Org.Webrtc;

namespace PrivMX.Endpoint.Stream.Xamarin.Models
{
    public class PcObserver : Java.Lang.Object, PeerConnection.IObserver
    {
        private Dictionary<string, PmxFrameCryptor> FrameCryptorMap = new Dictionary<string, PmxFrameCryptor>();
        public ITrackObserver TrackObserver;
        private string streamRoomId;
        private PmxFrameCryptor.PmxFrameCryptorOptions options;
        private PeerConnectionManager peerConnectionManager;
        private PmxKeyStore keyStore;
        
        private Action<List<MediaStream>, RtpReceiver> onAddTrack;
        private Action<string> onVideoTrack;
        private Action<string> onRemoveVideoTrack = null;
        
        private Action<PeerConnection.SignalingState> onSignalingState;
        private Action<PeerConnection.PeerConnectionState> onPeerConnectionState;
        private Action<PeerConnection.IceGatheringState> onIceGatheringState;
        private Action<PeerConnection.IceConnectionState> onIceConnectionState;
        private Action<IceCandidate> onIceCandidate;
        private Action<MediaStream> onAddStream;
        private Action<MediaStream> onRemoveStream;
        private Action<DataChannel> onDataChannel;
        private Task onRenegotiationNeeded;
        private Action<MediaStreamTrack> onTrack;
        private Action<RtpReceiver> onRemoveTrack;
        
        public PcObserver(PeerConnectionManager peerConnectionManager, string streamRoomId, PmxKeyStore peerKeyStore, 
            PmxFrameCryptor.PmxFrameCryptorOptions options, ITrackObserver trackObserver)
        {
            this.peerConnectionManager = peerConnectionManager;
            this.streamRoomId = streamRoomId;
            this.keyStore = peerKeyStore;
            this.options = options;
            this.TrackObserver = trackObserver;
            
            Console.WriteLine("PcObserver OK");
        }
        
        public void OnAddStream(MediaStream p0)
        {
            Console.WriteLine("OnAddStream");
        }

        public void OnDataChannel(DataChannel p0)
        {
            Console.WriteLine("OnDataChannel");
        }

        public void OnIceCandidate(IceCandidate p0)
        {
            Console.WriteLine("OnIceCandidate");
        }

        public void OnIceCandidatesRemoved(IceCandidate[] p0)
        {
            Console.WriteLine("OnIceCandidatesRemoved");
        }

        public void OnIceConnectionChange(PeerConnection.IceConnectionState p0)
        {
            Console.WriteLine("OnIceConnectionChange");
        }

        public void OnStandardizedIceConnectionChange(PeerConnection.IceConnectionState p0)
        {
            Console.WriteLine("OnStandardizedIceConnectionChange");
        }

        public void OnConnectionChange(PeerConnection.PeerConnectionState newState)
        {
            Console.WriteLine("onConnectionChange");
        }

        public void OnIceConnectionReceivingChange(bool p0)
        {
            Console.WriteLine("OnIceConnectionReceivingChange");
        }

        public void OnIceCandidateError(IceCandidateErrorEvent e)
        {
            Console.WriteLine("OnIceCandidateError");
        }

        public void OnSelectedCandidatePairChanged(CandidatePairChangeEvent e)
        {
            Console.WriteLine("OnSelectedCandidatePairChanged");
        }

        public void OnAddTrack(RtpReceiver receiver, MediaStream[] mediaStreams)
        {
            Console.WriteLine("OnAddTrack");
        }

        public void OnRemoveTrack(RtpReceiver receiver)
        {
            Console.WriteLine("OnRemoveTrack");
        }

        public void OnTrack(RtpTransceiver transceiver)
        {
            Console.WriteLine("OnTrack");
        }

        public void OnIceGatheringChange(PeerConnection.IceGatheringState p0)
        {
            Console.WriteLine("OnIceGatheringChange");
        }

        public void OnRemoveStream(MediaStream p0)
        {
            Console.WriteLine("OnRemoveStream");
        }

        public void OnRenegotiationNeeded()
        {
            Console.WriteLine("OnRenegotiationNeeded");
        }

        public void OnSignalingChange(PeerConnection.SignalingState p0)
        {
            Console.WriteLine("OnSignalingChange");
        }
    }
}

#endif