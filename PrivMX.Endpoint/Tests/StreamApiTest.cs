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
using System.Diagnostics;
using System.Linq;
using Android.App;
using NUnit.Framework;
using Org.Webrtc;
using Org.Webrtc.Audio;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Event;
using PrivMX.Endpoint.Stream;
using PrivMX.Endpoint.Stream.Models.StreamApiLow;

namespace PrivMX.Endpoint.Tests
{
    public class StreamApiTest
    {
        [Test]
        public void TestStreamApi()
        {
            IEglBase rootEglBase = IEglBase.Create();
            PeerConnectionFactory.Initialize(PeerConnectionFactory.InitializationOptions
                .InvokeBuilder(Application.Context).CreateInitializationOptions());

            IAudioDeviceModule adm = JavaAudioDeviceModule.InvokeBuilder(Application.Context).CreateAudioDeviceModule();

            IVideoEncoderFactory encoderFactory = new DefaultVideoEncoderFactory(rootEglBase.EglBaseContext,
                true, false);

            IVideoDecoderFactory decoderFactory = new DefaultVideoDecoderFactory(rootEglBase.EglBaseContext);

            PeerConnectionFactory.Options options = new PeerConnectionFactory.Options();

            PeerConnectionFactory factory = PeerConnectionFactory.InvokeBuilder()
                .SetVideoDecoderFactory(decoderFactory)
                .SetVideoEncoderFactory(encoderFactory)
                .SetOptions(options)
                .SetAudioDeviceModule(adm)
                .CreatePeerConnectionFactory();
            
            adm.Release();
            
            string userPrivKey = "";
            string solutionId = "";
            string bridgeUrl = "";
            string contextId = "";
            
            Connection connection = Connection.Connect(userPrivKey, solutionId, bridgeUrl);
            EventApi eventApi = EventApi.Create(connection);

            try
            {
                StreamApi streamApi = new StreamApi(Application.Context, rootEglBase,
                    StreamApiLow.Create(connection, eventApi), factory);
                
                Console.WriteLine("listStreamRooms-------");

                StreamRoom streamRoom = 
                    streamApi.ListStreamRooms(contextId, 0, 100, "desc", null, null)
                        .ReadItems.FirstOrDefault();
                
                Assert.That(streamRoom != null, nameof(streamRoom) + " != null");
                Debug.Assert(streamRoom != null, nameof(streamRoom) + " != null");
                
                string streamRoomId = streamRoom.StreamRoomId;
                
                
            }
            catch (Exception e)
            {
                Assert.Fail();
                Console.WriteLine(e);
                throw;
            }
            
            Assert.Pass();
        }
    }
}

#endif