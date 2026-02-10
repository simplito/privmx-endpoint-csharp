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
using System.Linq;
using Org.Webrtc;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Stream.Models.StreamApiLow;
using PrivMX.Endpoint.Stream.Xamarin.Models;
using Context = Android.Content.Context;

namespace PrivMX.Endpoint.Stream.Xamarin
{
    public class StreamApi
    {
        public static string VIDEO_TRACK_ID = "ARDAMSv0";
        public static string AUDIO_TRACK_ID = "ARDAMSa0";
        public static string VIDEO_TRACK_TYPE = "video";
        private static string TAG = "StreamApi";

        //private Context appContext; // only for gettting media devices and addTrack
        private PeerConnectionManager peerConnectionManager;
        private StreamApiLow streamApiLow;
        private FrameCryptorKeyProvider frameCryptorKeyProvider;
        
        private StreamMap streamMap;

        public StreamApi(PeerConnectionManager peerConnectionManager, StreamApiLow streamApiLow)
        {
            this.peerConnectionManager = peerConnectionManager;
            this.streamApiLow = streamApiLow;
            
            streamMap = new StreamMap();
        }

        public PagingList<StreamRoom> ListStreamRooms(string contextId, PagingQuery pagingQuery)
        {
            return streamApiLow.ListStreamRooms(contextId, pagingQuery);
        }

        public void JoinStreamRoom(string streamRoomId, ITrackObserver trackObserver)
        {
            StreamData streamData = streamMap.Create(trackObserver, streamRoomId, peerConnectionManager);
            Console.WriteLine("Stream data ok");
            streamApiLow.JoinStreamRoom(streamRoomId, streamData.WebRTC);
        }

        public StreamHandle CreateStream(string streamRoomId)
        {
            PeerConnection2 pc2 = peerConnectionManager.GetJanusSessions().Values.First().Sender;
            long streamHandle = streamApiLow.CreateStream(streamRoomId);
            Console.WriteLine("StreamApi::CreateStream returned: " + streamHandle);
            return new StreamHandle(streamHandle);
        }

        private IVideoCapturer? CreateVideoCapturer(ICameraEnumerator enumerator)
        {
            string[] deviceNames = enumerator.GetDeviceNames();
            Console.WriteLine("Looking for front facing cameras.");
            foreach (string deviceName in deviceNames)
            {
                if (enumerator.IsFrontFacing(deviceName))
                {
                    Console.WriteLine("Creating front facing camera capturer.");
                    IVideoCapturer videoCapturer = enumerator.CreateCapturer(deviceName, null);

                    if (videoCapturer != null)
                    {
                        Console.WriteLine("videoCapturer OK");
                        return videoCapturer;
                    }
                }
            }
            
            Console.WriteLine("Looking for other cameras.");
            foreach (string deviceName in deviceNames)
            {
                if (!enumerator.IsFrontFacing(deviceName))
                {
                    Console.WriteLine("Creating other camera capturer.");
                    IVideoCapturer videoCapturer = enumerator.CreateCapturer(deviceName, null);

                    if (videoCapturer != null)
                    {
                        return videoCapturer;
                    }
                }
            }

            return null;
        }

        public void AddTrack(Context context, IVideoSink localSink, StreamHandle streamHandle, MediaDevice track)
        {
            StreamData streamData = streamMap.GetFirst();

            switch (track.Type)
            {
                case DeviceType.Video:
                    SurfaceTextureHelper surfaceTextureHelper = SurfaceTextureHelper.Create("CaptureThread", 
                        peerConnectionManager.GetEglBase().EglBaseContext);
                    VideoSource videoSource = peerConnectionManager.GetPeerConnectionFactory()
                        .CreateVideoSource(false, false);
                    IVideoCapturer capturer = CreateVideoCapturer(new Camera2Enumerator(peerConnectionManager.GetAppContext()))!;
                    capturer.Initialize(surfaceTextureHelper, peerConnectionManager.GetAppContext(), videoSource.CapturerObserver);
                    VideoTrack videoTrack = peerConnectionManager.GetPeerConnectionFactory()
                        .CreateVideoTrack(track.Name, videoSource);
                    videoTrack.SetEnabled(true);
                    //videoTrack.AddSink(localSink);
                    Console.WriteLine("before webrtc addVideoTrack");
                    
                    streamData!.WebRTC.AddVideoTrack(streamData.StreamRoomId, videoTrack, track.Id);
                    Console.WriteLine("after webrtc addVideoTrack");
                    
                    Console.WriteLine("before streamCapturers.add");
                    streamData.streamCapturers.Add(track.Id, capturer);
                    Console.WriteLine("after streamCapturers.add");
                    
                    Console.WriteLine("before start capturer + " + (streamData.StreamStatus == StreamStatus.Online));
                    if (streamData.StreamStatus == StreamStatus.Online)
                    {
                        capturer.StartCapture(1280, 720, 30);
                    }
                    Console.WriteLine("after start capturer");
                    break;
            }
        }

        public void PublishStream(StreamHandle localStreamId)
        {
            streamApiLow.PublishStream(localStreamId.GetValue());
        }
    }
}

#endif