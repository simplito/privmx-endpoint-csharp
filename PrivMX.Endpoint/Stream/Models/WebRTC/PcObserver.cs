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
using Java.Interop;
using Java.Util;
using Java.Util.Functions;
using Org.Webrtc;
using PrivMX.Endpoint.Stream.Models.WebRTC.Adapters;

namespace PrivMX.Endpoint.Stream.Models.WebRTC
{
    public class PcObserver : PeerConnection.IObserver
    {
        private Dictionary<string, PmxFrameCryptor> FrameCryptorMap;
        private PeerConnectionFactory peerConnectionFactory;
        public ITrackObserver TrackObserver;
        private string streamRoomId;
        private PmxFrameCryptor.PmxFrameCryptorOptions options;
        
        private IBiConsumer onAddTrack;
        
        public PcObserver()
        {
            FrameCryptorMap = new Dictionary<string, PmxFrameCryptor>();
            onAddTrack = new BiConsumerAdapter<Java.Lang.Object, RtpReceiver>(
                new BiConsumerAdapter_MediaStreamList_Receiver()
            );
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