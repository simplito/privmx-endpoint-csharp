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
    /// Represents an item policy.
    /// </summary>
    public class ItemPolicy
    {
        /// <summary>
        /// (optional) Determines who can get an item.
        /// </summary>
        public string? Get { get; set; }

        /// <summary>
        /// (optional) Determines who can list items created by me.
        /// </summary>
        public string? ListMy { get; set; }

        /// <summary>
        /// (optional) Determines who can list all items.
        /// </summary>
        public string? ListAll { get; set; }

        /// <summary>
        /// (optional) Determines who can create an item.
        /// </summary>
        public string? Create { get; set; }

        /// <summary>
        /// (optional) Determines who can update an item.
        /// </summary>
        public string? Update { get; set; }

        /// <summary>
        /// (optional) Determines who can delete an item.
        /// </summary>
        public string? Delete_ { get; set; }
    }
}
