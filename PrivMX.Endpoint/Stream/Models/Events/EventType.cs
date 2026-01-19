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

namespace PrivMX.Endpoint.Stream.Models.Events
{
    /// <summary>
    /// StreamApi subscription event types to listen for 
    /// </summary>
    public enum EventType : long
    {
        STREAMROOM_CREATE = 0,
        STREAMROOM_UPDATE = 1,
        STREAMROOM_DELETE = 2,
        STREAM_JOIN = 4,
        STREAM_LEAVE = 5,
        STREAM_PUBLISH = 6,
        STREAM_UNPUBLISH = 7,
    }
}