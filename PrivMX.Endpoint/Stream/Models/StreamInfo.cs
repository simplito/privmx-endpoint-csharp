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

using System;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Stream.Models
{
    /// <summary>
    /// Holds information about the stream
    /// </summary>
    public class StreamInfo
    {
        /// <summary>
        /// Unique ID of the publisher
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// ID of the user
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// (optional) Stream metadata in JSON format
        /// </summary>
        public string? Metadata { get; set; }
        
        /// <summary>
        /// (optional) Marks if it's a dummy publisher
        /// </summary>
        public bool? Dummy { get; set; }

        /// <summary>
        /// List of streamTracts
        /// </summary>
        public List<StreamTrackInfo> Tracks { get; set; }
        
        /// <summary>
        /// (optional) Marks if audio in the stream is active
        /// </summary>
        [Obsolete("Field Talking is deprecated and shouldn't be used.")]
        public bool? Talking { get; set; }
    }
}