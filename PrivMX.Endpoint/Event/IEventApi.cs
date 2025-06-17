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

using System.Collections.Generic;
using PrivMX.Endpoint.Core.Models;

namespace PrivMX.Endpoint.Event
{
    public interface IEventApi
    {
        void EmitEvent(string contextId, List<UserWithPubKey> users, string channelName, byte[] eventData);
        void SubscribeForCustomEvents(string contextId, string channelName);
        void UnsubscribeFromCustomEvents(string contextId, string channelName);
    }
}
