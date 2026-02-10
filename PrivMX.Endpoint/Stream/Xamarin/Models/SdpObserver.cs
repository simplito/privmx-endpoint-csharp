#if ANDROID

using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Runtime;
using Org.Webrtc;

namespace PrivMX.Endpoint.Stream.Xamarin.Models
{
    public class SdpObserver : Java.Lang.Object, ISdpObserver
    {
        private readonly TaskCompletionSource<string> _tcs;
        private readonly PeerConnection _pc;
        
        public SdpObserver(TaskCompletionSource<string> tcs, PeerConnection pc)
        {
            _tcs = tcs;
            _pc = pc;
        }
        
        public void OnCreateFailure(string p0)
        {
            Console.WriteLine("SdpObserver::OnCreateFailure");
        }

        public void OnCreateSuccess(SessionDescription p0)
        {
            Console.WriteLine("SdpObserver::onCreateSuccess      " + p0.Description);

            _pc.SetLocalDescription(this, p0);
            _tcs.TrySetResult(p0.Description);
        }

        public void OnSetFailure(string p0)
        {
            Console.WriteLine("SdpObserver::OnSetFailure");
        }

        public void OnSetSuccess()
        {
            Console.WriteLine("SdpObserver::OnSetSuccess");
        }
    }
}

#endif