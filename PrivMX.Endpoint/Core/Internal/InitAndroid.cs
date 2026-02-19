using System.Runtime.InteropServices;

namespace PrivMX.Endpoint.Core.Internal
{
    public static class InitAndroid
    {
        static InitAndroid()
        {
            privmx_endpoint_android_init();
        }

        public static void Init()
        {
            privmx_endpoint_android_init();
        }
        
        [DllImport("libprivmxendpointandroid")]
        private static extern int privmx_endpoint_android_init();
    }
}