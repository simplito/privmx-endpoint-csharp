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
    [Register("org/webrtc/PcObserver", DoNotGenerateAcw=true)]
    internal class PcObserver : Java.Lang.Object, PeerConnection.IObserver
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
        }
        
        public void Dispose()
        {
            FrameCryptorMap.Clear();
        }

        public IntPtr Handle { get; }
        public void SetJniIdentityHashCode(int value)
        {
            throw new NotImplementedException();
        }

        public void SetPeerReference(JniObjectReference reference)
        {
            throw new NotImplementedException();
        }

        public void SetJniManagedPeerState(JniManagedPeerStates value)
        {
            throw new NotImplementedException();
        }

        public void UnregisterFromRuntime()
        {
            throw new NotImplementedException();
        }

        public void DisposeUnlessReferenced()
        {
            throw new NotImplementedException();
        }

        public void Disposed()
        {
            throw new NotImplementedException();
        }

        public void Finalized()
        {
            throw new NotImplementedException();
        }

        public int JniIdentityHashCode { get; }
        public JniObjectReference PeerReference { get; }
        public JniPeerMembers JniPeerMembers { get; }
        public JniManagedPeerStates JniManagedPeerState { get; }
        public void OnAddStream(MediaStream p0)
        {
            throw new NotImplementedException();
        }

        public void OnDataChannel(DataChannel p0)
        {
            throw new NotImplementedException();
        }

        public void OnIceCandidate(IceCandidate p0)
        {
            throw new NotImplementedException();
        }

        public void OnIceCandidatesRemoved(IceCandidate[] p0)
        {
            throw new NotImplementedException();
        }

        public void OnIceConnectionChange(PeerConnection.IceConnectionState p0)
        {
            throw new NotImplementedException();
        }

        public void OnIceConnectionReceivingChange(bool p0)
        {
            throw new NotImplementedException();
        }

        public void OnIceGatheringChange(PeerConnection.IceGatheringState p0)
        {
            throw new NotImplementedException();
        }

        public void OnRemoveStream(MediaStream p0)
        {
            throw new NotImplementedException();
        }

        public void OnRenegotiationNeeded()
        {
            throw new NotImplementedException();
        }

        public void OnSignalingChange(PeerConnection.SignalingState p0)
        {
            throw new NotImplementedException();
        }
    }
}

#endif