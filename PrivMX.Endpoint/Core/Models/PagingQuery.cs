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
    /// Represents the paging parameters used in lists methods.
    /// </summary>
    public class PagingQuery
    {
        /// <summary>
        /// Number of items to skip when listing.
        /// </summary>
        public long Skip { get; set; }

        /// <summary>
        /// Number of items to list.
        /// </summary>
        public long Limit { get; set; }

        /// <summary>
        /// Sort order of items.
        /// 
        /// Allowed values: "asc" - ascending sort order, "desc" - descending sort order.
        /// Currently, results list items are sorted by creation date.
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// Optional Id of the item from which to list subsequent items.
        /// 
        /// If this parameter is passed, the Skip parameter is not used and can be any number.
        /// </summary>
        public string LastId { get; set; }

        /// <summary>
        /// (optional) Query for filtering by public metadata of items as a JSON formatted object.
        /// </summary>
        public string QueryAsJson { get; set; }
    }
}
