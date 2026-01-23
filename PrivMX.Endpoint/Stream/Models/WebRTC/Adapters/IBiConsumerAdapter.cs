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

namespace PrivMX.Endpoint.Stream.Models.WebRTC.Adapters
{
    public interface IBiConsumerAdapter<T1, T2>
    {
        void Accept(T1 t, T2 u);
    }
}

#endif