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
using Android.Runtime;
using Org.Webrtc;

namespace PrivMX.Endpoint.Stream.Models.StreamApi
{
    [Register("org/webrtc/CustomVideoSink", DoNotGenerateAcw=false)]
    public class CustomVideoSink : Java.Lang.Object, IVideoSink
    {
        private readonly IVideoFrameConsumer consumer;

        public CustomVideoSink(IVideoFrameConsumer consumer)
        {
            this.consumer = consumer;
        }
        
        public void OnFrame(VideoFrame p0)
        {
            Console.WriteLine("Frame: " + p0.RotatedHeight + " " + p0.RotatedWidth);
            byte[] rgba = VideoFrameConverter.ToRGBA(p0);
            
            consumer.OnFrame(
                rgba,
                p0.RotatedWidth,
                p0.RotatedHeight,
                p0.Rotation,
                p0.TimestampNs
            );
            
            p0.Release();
        }
    }
}

#endif