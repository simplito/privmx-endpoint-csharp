using System;
using System.Runtime.InteropServices;

namespace PrivMX.Endpoint.Tests
{
    public static class InitAndroid
    {
        [DllImport("libprivmxendpointandroid")]
        public static extern int privmx_endpoint_android_init();
        
        [DllImport("libprivmxendpointandroid")]
        public static extern IntPtr privmx_endpoint_android_dlopen(IntPtr path, int flags);
        
        [DllImport("libprivmxendpointandroid")]
        public static extern int privmx_endpoint_android_dlclose(IntPtr handle);
        
        [DllImport("libprivmxendpointandroid")]
        public static extern IntPtr privmx_endpoint_android_dlerror();
        
        [DllImport("libprivmxendpointandroid")]
        public static extern IntPtr privmx_endpoint_android_dlsym(IntPtr handle, IntPtr symbol);
        
    }
}