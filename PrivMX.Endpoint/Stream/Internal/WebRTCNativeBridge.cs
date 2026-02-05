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
using System.Runtime.InteropServices;
using PrivMX.Endpoint.Core.Internal;
using PrivMX.Endpoint.Stream.Models.StreamApiLow;

namespace PrivMX.Endpoint.Stream.Internal
{

    internal enum CKeyType
    {
        LOCAL,
        REMOTE
    }

    internal struct CKey
    {
        public string keyId;
        public IntPtr key;
        public IntPtr keySize;
        public CKeyType type;
    }

    internal class WebRTCNativeBridge
    {
        private struct WebRTCInterface
        {
            public IntPtr ctx;
            public CreateOfferAndSetLocalDescriptionDelegate createOfferAndSetLocalDescription;
            public CreateAnswerAndSetDescriptionsDelegate createAnswerAndSetDescriptions;
            public SetAnswerAndSetRemoteDescriptionDelegate setAnswerAndSetRemoteDescription;
            public UpdateSessionIdDelegate updateSessionId;
            public CloseDelegate close;
            public UpdateKeysDelegate updateKeys;
        }

        private class ProxyWebRTC
        {
            private readonly IWebRTC webRTC;
            public readonly IntPtr ptr;

            public ProxyWebRTC(IWebRTC webRTC)
            {
                this.webRTC = webRTC;
                WebRTCInterface webRTCInterface = new WebRTCInterface() {
                    ctx = new IntPtr(),
                    createOfferAndSetLocalDescription = (ctx, streamRoomId) => webRTC.CreateOfferAndSetLocalDescription(streamRoomId),
                    createAnswerAndSetDescriptions = (ctx, streamRoomId, sdp, type) => webRTC.CreateAnswerAndSetDescriptions(streamRoomId, sdp, type),
                    setAnswerAndSetRemoteDescription = (ctx, streamRoomId, sdp, type) => webRTC.SetAnswerAndSetRemoteDescription(streamRoomId, sdp, type),
                    updateSessionId = (ctx, streamRoomId, sessionId, connectionType) => webRTC.UpdateSessionId(streamRoomId, sessionId, connectionType),
                    close = (ctx, streamRoomId) => webRTC.Close(streamRoomId),
                    updateKeys = (ctx, streamRoomId, keys, keysSize) => webRTC.UpdateKeys(streamRoomId, mapKeys(keys, keysSize)),
                };
                privmx_endpoint_stream_newProxyWebRTC(
                    webRTCInterface,
                    out ptr
                );
            }

            ~ProxyWebRTC()
            {
                privmx_endpoint_stream_freeProxyWebRTC(ptr);
            }
        }

        private readonly Dictionary<string, ProxyWebRTC> proxyMap = new Dictionary<string, ProxyWebRTC>();

        public IntPtr Create(string streamRoomId, IWebRTC webRTC)
        {
            ProxyWebRTC proxy = new ProxyWebRTC(webRTC);
            proxyMap.Add(streamRoomId, proxy);
            return proxy.ptr;
        }

        public void Free(string streamRoomId)
        {
            proxyMap.Remove(streamRoomId);
        }

        private static List<Key> mapKeys([In] CKey[] keys, IntPtr keySize)
        {
            var result = new List<Key>();
            foreach (var key in keys)
            {
                byte[] keyVal = new byte[key.keySize.ToInt32()];
                Marshal.Copy(key.key, keyVal, 0, key.keySize.ToInt32());
                var newKey = new Key() {
                    KeyId = key.keyId,
                    key = keyVal,
                    Type = mapKeyType(key.type),
                };
                result.Add(newKey);
            }
            return result;
        }

        private static PrivMX.Endpoint.Stream.Models.StreamApiLow.KeyType mapKeyType(CKeyType type)
        {
            switch (type) {
                case CKeyType.LOCAL:
                    return PrivMX.Endpoint.Stream.Models.StreamApiLow.KeyType.LOCAL;
                case CKeyType.REMOTE:
                    return PrivMX.Endpoint.Stream.Models.StreamApiLow.KeyType.REMOTE;
            }
            throw new Exception("Unknown key type");
        }


        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate string CreateOfferAndSetLocalDescriptionDelegate(IntPtr ctx, string streamRoomId);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate string CreateAnswerAndSetDescriptionsDelegate(IntPtr ctx, string streamRoomId, string sdp, string type);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetAnswerAndSetRemoteDescriptionDelegate(IntPtr ctx, string streamRoomId, string sdp, string type);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void UpdateSessionIdDelegate(IntPtr ctx, string streamRoomId, long sessionId, string connectionType);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void CloseDelegate(IntPtr ctx, string streamRoomId);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void UpdateKeysDelegate(IntPtr ctx, string streamRoomId, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] CKey[] keys, IntPtr keysSize);

        [DllImport("libprivmxendpointstream")]
        private static extern int privmx_endpoint_stream_newProxyWebRTC(
            WebRTCInterface webRTCInterface,
            out IntPtr result
        );

        [DllImport("libprivmxendpointstream")]
        private static extern int privmx_endpoint_stream_freeProxyWebRTC(IntPtr ptr);
    }
}
