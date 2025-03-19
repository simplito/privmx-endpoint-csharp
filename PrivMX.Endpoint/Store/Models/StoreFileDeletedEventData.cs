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
    /// Represents payload of the StoreFileDeletedEvent.
    /// </summary>
    public class StoreFileDeletedEventData
    {
        /// <summary>
        /// ID of the Context to which the Store belongs.
        /// </summary>
        public string ContextId { get; set; }

        /// <summary>
        /// ID of the Store that the file is deleted from.
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// ID of the deleted file.
        /// </summary>
        public string FileId { get; set; }
    }
}
