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
    /// Represents the event of type "libBreak".
    /// 
    /// This event is emitted when <see cref="EventQueue.EmitBreakEvent()"/> method is called.
    /// It is useful for breaking an event processing loop.
    /// </summary>
    public class LibBreakEvent : Event {}
}
