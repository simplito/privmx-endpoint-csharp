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
    /// Connection subscription event types to listen for 
    /// </summary>
    public enum EventType : long
    {
        USER_ADD = 0,
        USER_REMOVE = 1,
        USER_STATUS = 2
    }
}