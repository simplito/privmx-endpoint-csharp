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

namespace PrivMX.Endpoint.Thread.Models
{
    /// <summary>
    /// Scope on which you listen for events in ThreadApi
    /// </summary>
    public enum EventSelectorType : long
    {
        CONTEXT_ID = 0,
        THREAD_ID = 1,
        MESSAGE_ID = 2,
    }
}