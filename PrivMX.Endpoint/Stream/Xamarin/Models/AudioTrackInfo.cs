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

using Org.Webrtc;
using AudioTrack = Android.Media.AudioTrack;

namespace PrivMX.Endpoint.Stream.Xamarin.Models
{
    public class AudioTrackInfo
    {
        public AudioTrack Track { get; }
        public RtpSender Sender { get; }
        public PmxFrameCryptor FrameCryptor { get; }

        public AudioTrackInfo(AudioTrack track, RtpSender sender, PmxFrameCryptor frameCryptor)
        {
            Track = track;
            Sender = sender;
            FrameCryptor = frameCryptor;
        }
    }
}

#endif