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
    /// Represents a container and item policy.
    /// </summary>
    public class ContainerPolicy : ContainerPolicyWithoutItem
    {
        /// <summary>
        /// (optional) Item policy.
        /// </summary>
        public ItemPolicy Item { get; set; }
    }
}
