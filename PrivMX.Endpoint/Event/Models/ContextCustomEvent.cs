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

namespace PrivMX.Endpoint.Event.Models 
{
    /// <summary>
    /// Holds data of event that arrives when custom context event is emitted.
    /// </summary>
    public class ContextCustomEvent : Core.Models.Event
    {
        /// <summary>
        /// Event constructor
        /// </summary>
        public ContextCustomEvent() : base("contextCustom")
        {
        }

        /// <summary>
        /// Event's data
        /// </summary>
        public ContextCustomEventData Data { get; set; } = null!;
    }
}
