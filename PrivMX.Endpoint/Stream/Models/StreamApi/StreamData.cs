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

namespace PrivMX.Endpoint.Stream.Models.StreamApi
{
    public class StreamData
    {
        public long StreamHandle { get; set; }
        public string StreamRoomId { get; set; }
        public StreamStatus StreamStatus { get; set; }
        public Dictionary<long, IVideoCapturer> streamCapturers { get; set; } = new Dictionary<long, IVideoCapturer>();
        public Stream.WebRTC WebRTC { get; set; }
    }
}

#endif