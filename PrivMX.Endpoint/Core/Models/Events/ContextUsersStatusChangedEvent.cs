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
    /// Holds data of event that arrives when the user user's status is changed in a context specified in 'ContextUserEventData'
    /// </summary>
    public class ContextUsersStatusChangedEvent : Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public ContextUsersStatusChangedEvent() : base("contextUserStatusChanged")
        {
        }

        /// <summary>
        /// Information about the added user
        /// </summary>
        public ContextUsersStatusChangedEventData Data { get; set; } = null!;
    }
}