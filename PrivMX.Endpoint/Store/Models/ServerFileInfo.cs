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

namespace PrivMX.Endpoint.Store.Models
{
    /// <summary>
    /// Represents server metadata.
    /// </summary>
    public class ServerFileInfo
    {
        /// <summary>
        /// ID of the Store.
        /// </summary>
        public string StoreId { get; set; } = null!;

        /// <summary>
        /// ID of the file.
        /// </summary>
        public string FileId { get; set; } = null!;

        /// <summary>
        /// Server creation timestamp.
        /// </summary>
        public long CreateDate { get; set; }

        /// <summary>
        /// ID of the creator user.
        /// </summary>
        public string Author { get; set; } = null!;
    }
}
