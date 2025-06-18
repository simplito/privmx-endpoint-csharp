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

namespace PrivMX.Endpoint.Stream.Models
{
    /// <summary>
    /// Represents a stream room.
    /// </summary>
    public class StreamRoom
    {
        /// <summary>
        /// ID of the Context.
        /// </summary>
        public string ContextId { get; set; }

        /// <summary>
        /// ID of the stream room.
        /// </summary>
        public string StreamRoomId { get; set; }

        /// <summary>
        /// Server creation timestamp.
        /// </summary>
        public long CreateDate { get; set; }

        /// <summary>
        /// ID of the creator user.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Last modification timestamp.
        /// </summary>
        public long LastModificationDate { get; set; }

        /// <summary>
        /// ID of the user who was last a modifier.
        /// </summary>
        public string LastModifier { get; set; }

        /// <summary>
        /// List of user IDs that have access to the stream room.
        /// </summary>
        public List<string> Users { get; set; }

        /// <summary>
        /// List of user IDs that have management rights to the stream room.
        /// </summary>
        public List<string> Managers { get; set; }

        /// <summary>
        /// Number of the stream room updates.
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// Stream room's policy.
        /// </summary>
        public ContainerPolicy Policy { get; set; }

        /// <summary>
        /// Public metadata.
        /// </summary>
        public byte[] PublicMeta { get; set; }

        /// <summary>
        /// Private metadata.
        /// </summary>
        public byte[] PrivateMeta { get; set; }

        /// <summary>
        /// Status code of decryption and verification of the stream room.
        /// 
        /// If value is equal 0, then the stream room is successfully decrypted and verified. Otherwise, status code is compatible with codes of exceptions.
        /// </summary>
        public long StatusCode { get; set; }
    }
}
