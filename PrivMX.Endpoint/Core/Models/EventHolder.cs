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

namespace PrivMX.Endpoint.Core.Models
{
    /// <summary>
    /// 'EventHolder' is an helper class containing functions to operate on 'Event' objects.
    /// </summary>
    public class EventHolder
    {
        private readonly Event _event;
        
        /// <summary>
        /// 'EventHolder' constructor
        /// </summary>
        /// <param name="eventToHold">Pointer to the 'Event' object to use in the 'EventHolder'</param>
        public EventHolder(Event eventToHold)
        {
            _event = eventToHold;
        }
        
        /// <summary>
        /// Gets underlying Event's type
        /// </summary>
        public string Type => _event.Type;
        
        /// <summary>
        /// Gets underlying Event's channel
        /// </summary>
        public string Channel => _event.Channel;

        /// <summary>
        /// Serializes an Event to the JSON string
        /// </summary>
        /// <returns>JSON string representation of the 'Event' object</returns>
        public string ToJson()
        {
            return _event.ToJson();
        }
        
        /// <summary>
        /// Gets pointer to the underlying 'Event' object
        /// </summary>
        public Event Get => _event;
    }
}