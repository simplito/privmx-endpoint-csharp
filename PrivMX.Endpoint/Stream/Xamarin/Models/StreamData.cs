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

using System.Collections.Generic;
using Org.Webrtc;
using PrivMX.Endpoint.Stream.Models.StreamApiLow;

namespace PrivMX.Endpoint.Stream.Xamarin.Models
{
    internal class StreamData
    {
        public long StreamHandle { get; set; }
        public string StreamRoomId { get; set; }
        public StreamStatus StreamStatus { get; set; }
        public Dictionary<string, IVideoCapturer> streamCapturers { get; set; } = new Dictionary<string, IVideoCapturer>();
        public WebRTCImpl WebRTC { get; set; }
    }
}

#endif