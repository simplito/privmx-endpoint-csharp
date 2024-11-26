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
    /// Represents the Context.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// ID of the current user in the Context.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// ID of the Context.
        /// </summary>
        public string ContextId { get; set; }
    }
}
