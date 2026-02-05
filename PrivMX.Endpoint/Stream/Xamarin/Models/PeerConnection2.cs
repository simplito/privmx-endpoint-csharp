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
using System.Data.SqlTypes;
using Org.Webrtc;

namespace PrivMX.Endpoint.Stream.Xamarin.Models
{
    internal class PeerConnection2 : INullable
    {
        private PeerConnection peerConnection;
        private PcObserver pcObserver;
        private PmxKeyStore keys;
        private Dictionary<string, AudioTrackInfo> audioTracks;
        private Dictionary<string, VideoTrackInfo> videoTracks;

        public PeerConnection2(PeerConnection peerConnection, PcObserver pcObserver, PmxKeyStore keys)
        {
            this.peerConnection = peerConnection;
            this.pcObserver = pcObserver;
            this.keys = keys;
            
            audioTracks = new Dictionary<string, AudioTrackInfo>();
            videoTracks = new Dictionary<string, VideoTrackInfo>();
        }

        public PeerConnection GetPeerConnection()
        {
            return peerConnection;
        }

        public void AddVideoTrack(string id, VideoTrackInfo track)
        {
            videoTracks.Add(id, track);
        }

        public bool IsNull => peerConnection.Handle == IntPtr.Zero || pcObserver.Handle == IntPtr.Zero || keys.Handle == IntPtr.Zero;
    }
}

#endif