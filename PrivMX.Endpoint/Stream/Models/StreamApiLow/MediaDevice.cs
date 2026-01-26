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

namespace PrivMX.Endpoint.Stream.Models.StreamApiLow
{
    /// <summary>
    /// Holds information about media device
    /// </summary>
    public class MediaDevice
    {
        /// <summary>
        /// Name of the device
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// ID of the device
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Type of the device
        /// </summary>
        public DeviceType Type { get; set; }
    }
}