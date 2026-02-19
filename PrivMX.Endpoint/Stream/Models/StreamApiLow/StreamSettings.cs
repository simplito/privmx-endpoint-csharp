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
    /// Holds information about stream's settings
    /// </summary>
    public class StreamSettings
    {
        /// <summary>
        /// Settings object
        /// </summary>
        public Settings Settings { get; set; }
        
        /// <summary>
        /// Checked if corrupted frames should be dropped
        /// </summary>
        public bool dropCorruptedFrames;

        /// <summary>
        /// StreamSettings constructor
        /// </summary>
        public StreamSettings()
        {
            
        }

        /// <summary>
        /// StreamSettings constructor
        /// </summary>
        /// <param name="settings">Settings object</param>
        /// <param name="dropCorruptedFrames">Checked if corrupted frames should be dropped</param>
        public StreamSettings(Settings settings, bool dropCorruptedFrames = true)
        {
            Settings = settings;
            this.dropCorruptedFrames = dropCorruptedFrames;
        }
    }
}