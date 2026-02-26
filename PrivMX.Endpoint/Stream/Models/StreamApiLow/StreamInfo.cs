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

namespace PrivMX.Endpoint.Stream.Models.StreamApiLow
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
        /// List of StreamTrackInfo
        /// </summary>
        public List<StreamTrackInfo> Tracks { get; set; }
        
        /// <summary>
        /// (optional) Stream metadata in JSON format
        /// </summary>
        public string? Metadata { get; set; }
        
        /// <summary>
        /// (optional) Marks if it's a dummy publisher
        /// </summary>
        public bool? Dummy { get; set; }
        
        /// <summary>
        /// (optional) Marks if audio in the stream is active
        /// </summary>
        public bool? Talking { get; set; }

        /// <summary>
        /// StreamInfo constructor
        /// </summary>
        public StreamInfo()
        {
            
        }
        
        /// <summary>
        /// StreamInfo constructor
        /// </summary>
        /// <param name="id">Unique ID of the publisher</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="tracks">List of StreamTrackInfo</param>
        /// <param name="metadata">(optional) Stream metadata in JSON format</param>
        /// <param name="dummy">(optional) Marks if it's a dummy publisher</param>
        /// <param name="talking">(optional) Marks if audio in the stream is active</param>
        public StreamInfo(long id, string userId, List<StreamTrackInfo> tracks, string metadata = "",
            bool dummy = false, bool talking = false)
        {
            Id = id;
            UserId = userId;
            Tracks = tracks;
            Metadata = metadata;
            Dummy = dummy;
            Talking = talking;
        }
    }
}