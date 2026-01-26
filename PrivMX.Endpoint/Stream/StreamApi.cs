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

#if ANDROID

using Org.Webrtc;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Stream.Models.StreamApi;
using PrivMX.Endpoint.Stream.Models.StreamApiLow;
using Context = Android.Content.Context;

namespace PrivMX.Endpoint.Stream
{
    public class StreamApi
    {
        public static string VIDEO_TRACK_ID = "ARDAMSv0";
        public static string AUDIO_TRACK_ID = "ARDAMSa0";
        public static string VIDEO_TRACK_TYPE = "video";
        private static string TAG = "StreamApi";

        private Context appContext;
        private IEglBase rootEglBase;
        private StreamApiLow streamApiLow;

        private PeerConnectionFactory peerConnectionFactory;
        private FrameCryptorKeyProvider frameCryptorKeyProvider;

        public StreamApi(Context appContext, IEglBase rootEglBase, StreamApiLow streamApiLow,
            PeerConnectionFactory? peerConnectionFactory)
        {
            this.appContext = appContext;
            this.rootEglBase = rootEglBase;
            this.streamApiLow = streamApiLow;
            this.peerConnectionFactory = peerConnectionFactory;
        }

        public PagingList<StreamRoom> ListStreamRooms(string contextId, long skip, int limit, string sortOrder,
            string lastId, string sortBy)
        {
            PagingQuery pagingQuery = new PagingQuery();
            pagingQuery.Skip = skip;
            pagingQuery.Limit = limit;
            pagingQuery.SortOrder = sortOrder;
            pagingQuery.LastId = lastId;
            
            return streamApiLow.ListStreamRooms(contextId, pagingQuery);
        }

        public void JoinStreamRoom(string streamRoomId, ITrackObserver trackObserver)
        {
            //StreamData
        }
    }
}

#endif