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
    /// Holds data of event that arrives when the user is removed from a context specified in 'ContextUserEventData'
    /// </summary>
    public class ContextUserRemovedEvent : Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public ContextUserRemovedEvent() : base("contextUserRemoved")
        {
        }

        /// <summary>
        /// Information about the added user
        /// </summary>
        public ContextUserEventData Data { get; set; } = null!;
    }
}