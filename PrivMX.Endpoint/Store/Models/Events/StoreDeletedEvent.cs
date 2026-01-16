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
    /// Holds data of event that arrives when Store is deleted.
    /// </summary>
    public class StoreDeletedEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public StoreDeletedEvent() : base("storeDeleted")
        {
            
        }
        
        /// <summary>
        /// Metadata of the deleted Store.
        /// </summary>
        public StoreDeletedEventData Data { get; set; } = null!;
    }
}
