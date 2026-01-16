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

namespace PrivMX.Endpoint.Store.Models
{
    /// <summary>
    /// Holds information about the file change.
    /// </summary>
    public class FileChange
    {
        /// <summary>
        /// position of the first changed chunk
        /// </summary>
        public long Pos { get; set; }
        
        /// <summary>
        /// length aligned to full chunks
        /// </summary>
        public long Length { get; set; }
        
        /// <summary>
        /// remove all data 
        /// </summary>
        public bool Truncate { get; set; }
    }
}