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
    /// Holds streamHandle model
    /// </summary>
    public class StreamHandle
    {
        /// <summary>
        /// Stream handle
        /// </summary>
        private long value;

        /// <summary>
        /// StreamHandle constructor
        /// </summary>
        /// <param name="value">Value</param>
        public StreamHandle(long value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets stream handls
        /// </summary>
        /// <returns>Value of stream handle</returns>
        public long GetValue()
        {
            return value;
        }
    }
}