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
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Android.App;
using Org.Webrtc;
using Org.Webrtc.Audio;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Event;
using PrivMX.Endpoint.Stream;
using PrivMX.Endpoint.Stream.Models.StreamApi;
using PrivMX.Endpoint.Stream.Models.StreamApiLow;

namespace PrivMX.Endpoint.Tests
{
    public class StreamApiTest
    {
        /*public void TestStreamApi(string userPrivKey, string solutionId, string bridgeUrl, string contextId, string certPath)
        {
            PeerConnectionManager peerConnectionManager = new PeerConnectionManager(Application.Context);
            
            Connection.SetCertsPath(certPath);
            Connection connection = Connection.Connect(userPrivKey, solutionId, bridgeUrl);
            EventApi eventApi = EventApi.Create(connection);

            try
            {
                StreamApi streamApi = new StreamApi(peerConnectionManager, StreamApiLow.Create(connection, eventApi));
                
                Console.WriteLine("listStreamRooms-------");

                StreamRoom streamRoom = 
                    streamApi.ListStreamRooms(contextId, 0, 100, "desc", null, null)
                        .ReadItems.FirstOrDefault();
                
                Debug.Assert(streamRoom != null, nameof(streamRoom) + " != null");
                
                string streamRoomId = streamRoom.StreamRoomId;
                
                CustomVideoSink sink = new CustomVideoSink(new BasicVideoFrameConsumer());
                var sinks = new List<CustomVideoSink> { sink };
                streamApi.JoinStreamRoom(streamRoomId, new TrackObserverImpl(sinks));

                StreamHandle localStreamId = streamApi.CreateStream(streamRoomId);
                MediaDevice localMediaDevice = new MediaDevice
                {
                    Id = "video_track_0",
                    Name = StreamApi.VIDEO_TRACK_ID,
                    Type = DeviceType.Video
                };

                streamApi.AddTrack(peerConnectionManager.GetAppContext(), sink, localStreamId, localMediaDevice);
                streamApi.PublishStream(localStreamId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }*/
    }
}

#endif