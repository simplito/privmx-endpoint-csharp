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

using System;

namespace PrivMX.Endpoint.Stream.Models.StreamApiLow
{
    /// <summary>
    /// Holds information about stream's settings
    /// </summary>
    public class StreamSettings
    {
        public Settings Settings { get; set; }
        public Action<string>? OnVideo { get; set; }
        public Action<long, long, Frame, string>? Onframe { get; set; }
        public Action<string>? OnVideoRemove { get; set; }
        public bool? DropCorruptedFrames { get; set; } = true;
    }
}