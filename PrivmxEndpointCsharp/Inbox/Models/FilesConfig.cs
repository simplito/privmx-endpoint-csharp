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

namespace PrivMX.Endpoint.Inbox.Models
{
    /// <summary>
    /// Represents the file configuration for creating a new entry in the Inbox.
    /// </summary>
    public class FilesConfig
    {
        /// <summary>
        /// Minimal number of files to create a new entry.
        /// </summary>
        public long MinCount { get; set; }

        /// <summary>
        /// Maximum number of files to create a new entry.
        /// </summary>
        public long MaxCount { get; set; }

        /// <summary>
        /// Maximum file size to create file in a new entry.
        /// </summary>
        public long MaxFileSize { get; set; }

        /// <summary>
        /// Maximum total size of files to create a new entry.
        /// </summary>
        public long MaxWholeUploadSize { get; set; }
    }
}
