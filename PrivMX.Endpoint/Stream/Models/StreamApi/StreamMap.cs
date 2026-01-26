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

namespace PrivMX.Endpoint.Stream.Models.StreamApi
{
    public class StreamMap
    {
        private Dictionary<long, StreamData> streamDataMap = new();
        private long currentId = 1;

        public StreamData GetStreamData(long streamId)
        {
            lock (streamDataMap)
            {
                StreamData data = streamDataMap[streamId];
                return data;
            }
        }

        public StreamData? GetFirst()
        {
            lock (streamDataMap)
            {
                return streamDataMap.Values.FirstOrDefault();
            }
        }

        public long GetRandomHandle()
        {
            Random random = new Random(1024);
            long h = random.NextInt64();
            while (streamDataMap.ContainsKey(h))
            {
                h = random.NextInt64();
            }

            return h;
        }
        
        private StreamData Create(ITrackObserver trackObserver)
        {
            lock (streamDataMap)
            {
                long handle = GetRandomHandle();
                StreamData streamData = new StreamData();
                streamData.StreamHandle = handle;
                streamData.WebRTC = new Stream.WebRTC(trackObserver);
                streamData.StreamStatus = StreamStatus.Online;
                streamData.streamCapturers = new Dictionary<long, IVideoCapturer>();
                streamDataMap.Add(handle, streamData);
                
                return streamData;
            }
        }

        private StreamData Create(ITrackObserver trackObserver, string streamRoomId)
        {
            lock (streamDataMap)
            {
                long handle = GetRandomHandle();
                StreamData streamData = new StreamData();
                streamData.StreamHandle = handle;
                streamData.WebRTC = new Stream.WebRTC(trackObserver);
                streamData.WebRTC.CreatePeerConnection(streamRoomId);
                streamData.StreamStatus = StreamStatus.Online;
                streamData.streamCapturers = new Dictionary<long, IVideoCapturer>();
                streamDataMap.Add(handle, streamData);
                
                return streamData;
            }
        }
    }
}

#endif