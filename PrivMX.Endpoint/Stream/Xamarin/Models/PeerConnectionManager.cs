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

using System.Collections.Generic;
using Android.Content;
using Org.Webrtc;
using Org.Webrtc.Audio;

namespace PrivMX.Endpoint.Stream.Xamarin.Models
{
    public class PeerConnectionManager
    {
        private PeerConnectionFactory peerConnectionFactory;
        private Dictionary<string, RoomJanusSession> janusSessions;
        private IEglBase rootEglBase;
        private Context appContext;

        public PeerConnectionManager(Context appContext)
        {
            janusSessions = new Dictionary<string, RoomJanusSession>();
            
            this.appContext =  appContext;
            
            rootEglBase = IEglBase.Create();
            PeerConnectionFactory.Initialize(PeerConnectionFactory.InitializationOptions
                .InvokeBuilder(appContext).CreateInitializationOptions());
            
            IVideoEncoderFactory encoderFactory = new DefaultVideoEncoderFactory(rootEglBase.EglBaseContext,
                true, false);

            IVideoDecoderFactory decoderFactory = new DefaultVideoDecoderFactory(rootEglBase.EglBaseContext);

            PeerConnectionFactory.Options options = new PeerConnectionFactory.Options();
            
            IAudioDeviceModule adm = JavaAudioDeviceModule.InvokeBuilder(appContext).CreateAudioDeviceModule();
            
            peerConnectionFactory = PeerConnectionFactory.InvokeBuilder()
                .SetVideoDecoderFactory(decoderFactory)
                .SetVideoEncoderFactory(encoderFactory)
                .SetOptions(options)
                .SetAudioDeviceModule(adm)
                .CreatePeerConnectionFactory();
            
            adm.Release();
        }

        public PeerConnectionFactory GetPeerConnectionFactory()
        {
            return peerConnectionFactory;
        }

        public IEglBase GetEglBase()
        {
            return rootEglBase;
        }

        internal Dictionary<string, RoomJanusSession> GetJanusSessions()
        {
            return janusSessions;
        }

        internal RoomJanusSession GetRoomJanusSession(string roomId)
        {
            return janusSessions[roomId];
        }

        public Context GetAppContext()
        {
            return appContext;
        }
    }
}

#endif