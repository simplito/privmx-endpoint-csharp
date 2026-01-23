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

using System;
using Java.Util.Functions;
using Org.Webrtc;
using Android.Runtime;

namespace PrivMX.Endpoint.Stream.Models.WebRTC.Adapters
{
    internal sealed class BiConsumerAdapter<T1, T2> 
        : Java.Lang.Object, IBiConsumer
        where T1 : Java.Lang.Object
        where T2 : Java.Lang.Object
    {
        private readonly IBiConsumerAdapter<T1, T2> _inner;

        public BiConsumerAdapter(IBiConsumerAdapter<T1, T2> inner)
        {
            _inner = inner;
        }

        public void Accept(Java.Lang.Object? t, Java.Lang.Object? u)
        {
            if (t is null || u is null)
                return;

            _inner.Accept(
                t.JavaCast<T1>(),
                u.JavaCast<T2>()
            );
        }
    }
    
    internal sealed class BiConsumerAdapter_MediaStreamList_Receiver 
        : IBiConsumerAdapter<Java.Util.IList, RtpReceiver>
    {
        public void Accept(Java.Util.IList list, RtpReceiver receiver)
        {
            for (int i = 0; i < list.Size(); i++)
            {
                if (list.Get(i) is not MediaStream)
                    throw new InvalidCastException("Expected Media");
                
                var media = list.Get(i).JavaCast<MediaStream>();
            }
        }
    }
}

#endif