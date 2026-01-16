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

namespace PrivMX.Endpoint.Store.Models.Events
{
    /// <summary>
    /// Holds data of event that arrives when Store file is deleted.
    /// </summary>
    public class StoreFileDeletedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public StoreFileDeletedEvent() : base("storeFileDeleted")
        {
            
        }
        
        /// <summary>
        /// Metadata of the deleted file.
        /// </summary>
        public StoreFileDeletedEventData Data { get; set; } = null!;
    }
}
