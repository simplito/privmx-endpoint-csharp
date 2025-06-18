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

using System.Collections.Generic;
using PrivMX.Endpoint.Core.Models;

namespace PrivMX.Endpoint.Stream.Models
{
    /// <summary>
    /// Represents a stream room.
    /// </summary>
    public class StreamData
    {
        public string UserId { get; set; } = null!;
        public byte[] Data { get; set; } = null!;
    }
}
