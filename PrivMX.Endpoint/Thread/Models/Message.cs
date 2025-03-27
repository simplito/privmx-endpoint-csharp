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

namespace PrivMX.Endpoint.Thread.Models
{
    /// <summary>
    /// Represents a message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Server metadata.
        /// </summary>
        public ServerMessageInfo Info { get; set; } = null!;

        /// <summary>
        /// Public metadata.
        /// </summary>
        public byte[] PublicMeta { get; set; } = null!;

        /// <summary>
        /// Private metadata.
        /// </summary>
        public byte[] PrivateMeta { get; set; } = null!;

        /// <summary>
        /// Payload of the message.
        /// </summary>
        public byte[] Data { get; set; } = null!;

        /// <summary>
        /// Verified public key of the PublicMeta, PrivateMeta and Data author.
        /// </summary>
        public string AuthorPubKey { get; set; } = null!;

        /// <summary>
        /// Status code of decryption and verification of the message.
        /// 
        /// If value is equal 0, then the message is successfully decrypted and verified. Otherwise, status code is compatible with codes of exceptions.
        /// </summary>
        public long StatusCode {get;set;}
    }
}
