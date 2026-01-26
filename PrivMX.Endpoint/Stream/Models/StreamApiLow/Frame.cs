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
    /// Helper class to convert frame formats
    /// </summary>
    public abstract class Frame
    {
        /// <summary>
        /// Convert value to RGBA format
        /// </summary>
        /// <param name="dst_argb"></param>
        /// <param name="dst_stride_argb"></param>
        /// <param name="dest_width"></param>
        /// <param name="dest_height"></param>
        /// <returns>RGBA value</returns>
        public abstract int ConvertToRGBA(uint[] dst_argb, int dst_stride_argb, int dest_width, int dest_height);
    }
}