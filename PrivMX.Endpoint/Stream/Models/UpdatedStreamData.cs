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

namespace PrivMX.Endpoint.Stream.Models
{
    /// <summary>
    /// Holds data of an updated stream
    /// </summary>
    public class UpdatedStreamData
    {
        /// <summary>
        /// Marks of updated stream is active
        /// </summary>
        public bool Active { get; set; }
        
        /// <summary>
        /// (optional) Stream's codes ("opus", "vp8", etc.)
        /// </summary>
        public string? Codec { get; set; }
        
        /// <summary>
        /// ID of the stream feed
        /// </summary>
        public long? StreamId { get; set; }
        
        /// <summary>
        /// Stream's feed mid
        /// </summary>
        public string? StreamMid { get; set; }
        
        /// <summary>
        /// Stream's feed display
        /// </summary>
        public string? Stream_display { get; set; }
        
        /// <summary>
        /// Stream's mindex
        /// </summary>
        public long Mindex { get; set; }
        
        /// <summary>
        /// Stream's mid
        /// </summary>
        public string Mid { get; set; }
        
        /// <summary>
        /// Marks if stream is sending information
        /// </summary>
        public bool Send { get; set; }
        
        /// <summary>
        /// Marks if stream is ready
        /// </summary>
        public bool Ready { get; set; }
    }
}