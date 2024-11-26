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
    /// Represents a file.
    /// </summary>
    public class File
    {
        /// <summary>
        /// Server metadata.
        /// </summary>
        public ServerFileInfo Info { get; set; }

        /// <summary>
        /// Public metadata.
        /// </summary>
        public byte[] PublicMeta { get; set; }

        /// <summary>
        /// Private metadata.
        /// </summary>
        public byte[] PrivateMeta { get; set; }
        
        /// <summary>
        /// Size of the file.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Verified public key of the PublicMeta, PrivateMeta and Data author.
        /// </summary>
        public string AuthorPubKey { get; set; }

        /// <summary>
        /// Status code of decryption and verification of the message.
        /// If value is equal 0, then the message is successfully decrypted and verified. Otherwise, status code is compatible with codes of exceptions.
        /// </summary>
        public long StatusCode { get; set; }
    }
}
