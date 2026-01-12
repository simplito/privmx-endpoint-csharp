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

namespace PrivMX.Endpoint.Core
{
    public interface IConnection
    {
        long GetConnectionId();
        PagingList<Context> ListContexts(PagingQuery pagingQuery);
        PagingList<UserInfo> ListContextUsers(string contextId, PagingQuery pagingQuery);
        List<string> SubscribeFor(List<string> subscriptionQueries);
        void UnsubscribeFrom(List<string> subscriptionIds);
        string BuildSubscriptionQuery(EventType eventType, EventSelectorType selectorType, string selectorId);
        void Disconnect();
        void SetUserVerifier(UserVerifierInterface verifier);
        
    }
}
