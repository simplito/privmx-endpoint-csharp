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

namespace PrivMX.Endpoint.Core.Models
{
    /// <summary>
    /// Represents exception data from the native library.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// ID of the native exception.
        /// </summary>
        public long Code { get; set; }

        /// <summary>
        /// Readable error message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Name of the native exception.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Source module.
        /// </summary>
        public string? Scope { get; set; }

        /// <summary>
        /// Addtional detailed description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Serialized exception data.
        /// </summary>
        public string? Full { get; set; }
    }
}
