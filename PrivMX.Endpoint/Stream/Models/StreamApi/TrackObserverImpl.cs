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
using Org.Webrtc;

namespace PrivMX.Endpoint.Stream.Models.StreamApi
{
    public class TrackObserverImpl : ITrackObserver
    {
        private int remoteSinkId = 0;
        private List<CustomVideoSink> remoteSinks;

        public TrackObserverImpl(List<CustomVideoSink> sinks)
        {
            remoteSinks = sinks;
        }
        
        public void OnTrack(MediaStreamTrack track)
        {
            if (MediaStreamTrack.MediaType.MediaTypeVideo.Name() == track.Kind())
            {
                if (track is VideoTrack s)
                {
                    lock (remoteSinks)
                    {
                        if (remoteSinks.Count > remoteSinkId)
                        {
                            s.AddSink(remoteSinks[remoteSinkId]);
                            ++remoteSinkId;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("track was not a valid type");
                }
            }
        }
    }
}

#endif