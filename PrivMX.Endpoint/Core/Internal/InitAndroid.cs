using System.Runtime.InteropServices;

#if ANDROID

namespace PrivMX.Endpoint.Core.Internal
{
    public static class InitAndroid
    {
        static InitAndroid()
        {
            privmx_endpoint_android_init();
        }
        
        [DllImport("libprivmxendpointandroid")]
        private static extern int privmx_endpoint_android_init();
    }
}

#endif