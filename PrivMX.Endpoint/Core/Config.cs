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

using System.Runtime.InteropServices;

namespace PrivMX.Endpoint.Core
{
    public class Config
    {
        /// <summary>
        /// Sets path to a file containing a bundle of CA certificates in PEM format.
        /// This bundle is used by the library for TLS with HTTPS connections.
        /// </summary>
        /// <param name="certsPath"></param>
        public static void SetCertsPath(string certsPath)
        {
            privmx_endpoint_setCertsPath(certsPath);
        }
        
        [DllImport("libprivmxendpointinterface")]
        private static extern int privmx_endpoint_setCertsPath(string certsPath);
    }
}
