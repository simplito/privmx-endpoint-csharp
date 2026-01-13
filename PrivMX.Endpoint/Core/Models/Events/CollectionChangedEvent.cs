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

namespace PrivMX.Endpoint.Core.Models.Events
{
    /// <summary>
    /// Holds data of event that arrives when the collection is changed.
    /// </summary>
    public class CollectionChangedEvent : Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public CollectionChangedEvent() : base("collectionChanged")
        {
        }
        
        /// <summary>
        /// Information about the changed collection.
        /// </summary>
        public CollectionChangedEventData Data { get; set; } = null!;
    }
}