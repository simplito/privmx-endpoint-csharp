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
    /// Represents a container policy.
    /// </summary>
    public class ContainerPolicyWithoutItem
    {
        /// <summary>
        /// (optional) Determines who can get the container.
        /// </summary>
        public string? Get { get; set; }

        /// <summary>
        /// (optional) Determines who can update the container.
        /// </summary>
        public string? Update { get; set; }

        /// <summary>
        /// (optional) Determines who can delete the container.
        /// </summary>
        public string? Delete_ { get; set; }

        /// <summary>
        /// (optional) Determines who can update this policy.
        /// </summary>
        public string? UpdatePolicy { get; set; }

        /// <summary>
        /// (optional) Determines whether the updater can be removed from the list of managers.
        /// </summary>
        public string? UpdaterCanBeRemovedFromManagers { get; set; }

        /// <summary>
        /// (optional) Determines whether the owner can be removed from the list of managers.
        /// </summary>
        public string? OwnerCanBeRemovedFromManagers { get; set; }
    }
}
