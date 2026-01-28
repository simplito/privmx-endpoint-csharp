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

namespace PrivMX.Endpoint.Stream.Models.StreamApi
{
    public interface IVideoFrameConsumer
    {
        void OnFrame(byte[] rgba, int width, int height, int rotation, long timestampNs);
    }
}

#endif