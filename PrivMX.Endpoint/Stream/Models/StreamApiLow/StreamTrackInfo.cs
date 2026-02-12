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

namespace PrivMX.Endpoint.Stream.Models.StreamApiLow
{
    /// <summary>
    /// Holds information about a stream track
    /// </summary>
    public class StreamTrackInfo
    {
        /// <summary>
        /// type of the track - "audio" | "video" | "data"
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Unique stream's mindex
        /// </summary>
        public long Mindex { get; set; }
        
        /// <summary>
        /// Unique mid
        /// </summary>
        public string Mid { get; set; }
        
        /// <summary>
        /// (optional) Marks if the stream is disabled
        /// </summary>
        public bool? Disabled { get; set; }
        
        /// <summary>
        /// (optional) Stream's codes ("opus", "vp8", etc.)
        /// </summary>
        public string? Codec { get; set; }
        
        /// <summary>
        /// (optional) Description of the stream
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// (optional) Marks if the stream is moderated
        /// </summary>
        public bool? Moderated { get; set; }
        
        /// <summary>
        /// (optional) Marks if the stream uses simulcast
        /// </summary>
        public bool? Simulcast { get; set; }
        
        /// <summary>
        /// (optional) Marks if audio in the stream is active
        /// </summary>
        public bool? Talking { get; set; }
        
        /// <summary>
        /// StreamTrackInfo constructor
        /// </summary>
        /// <param name="type">type of the track - "audio" | "video" | "data"</param>
        /// <param name="mindex">Unique stream's mindex</param>
        /// <param name="mid">Unique mid</param>
        /// <param name="disabled">(optional) Marks if the stream is disabled</param>
        /// <param name="codec">(optional) Stream's codes ("opus", "vp8", etc.)</param>
        /// <param name="description">(optional) Description of the stream</param>
        /// <param name="moderated">(optional) Marks if the stream is moderated</param>
        /// <param name="simulcast">(optional) Marks if the stream uses simulcast</param>
        /// <param name="talking">(optional) Marks if audio in the stream is active</param>
        public StreamTrackInfo(string type, long mindex, string mid, bool? disabled, string? codec, 
            string? description, bool? moderated, bool? simulcast, bool? talking)
        {
            Type = type;
            Mindex = mindex;
            Mid = mid;
            Disabled = disabled ?? false;
            Codec = codec ?? string.Empty;
            Description = description ?? string.Empty;
            Moderated = moderated ?? false;
            Simulcast = simulcast ?? false;
            Talking = talking ?? false;
        }
    }
}