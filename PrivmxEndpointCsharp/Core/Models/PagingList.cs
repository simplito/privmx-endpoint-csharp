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

namespace PrivMX.Endpoint.Core.Models
{
    /// <summary>
    /// Represents a resulting list of items used in lists methods.
    /// </summary>
    /// <typeparam name="T">Resource type.</typeparam>
    public class PagingList<T>
    {
        /// <summary>
        /// Total number of all items available to list.
        /// </summary>
        public long TotalAvailable { get; set; }

        /// <summary>
        /// Resulting items listed according to the paging parameters passed to a method.
        /// </summary>
        public List<T> ReadItems { get; set; }
    }
}
