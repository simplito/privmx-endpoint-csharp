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

namespace PrivMX.Endpoint.Stream.Models.StreamApiLow
{
    /// <summary>
    /// Holds information about stream credentials
    /// </summary>
    public class TurnCredentials
    {
        /// <summary>
        /// Url of the stream
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// Username of the user
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Credentials expiration time
        /// </summary>
        public long ExpirationTime { get; set; }

        /// <summary>
        /// TurnCredentials constructor
        /// </summary>
        public TurnCredentials()
        {
            
        }

        /// <summary>
        /// TurnCredentials constructor
        /// </summary>
        /// <param name="url">Url of the stream</param>
        /// <param name="username">Username of the user</param>
        /// <param name="password">User's password</param>
        /// <param name="expirationTime">Credentials expiration time</param>
        public TurnCredentials(string url, string username, string password, long expirationTime)
        {
            Url = url;
            Username = username;
            Password = password;
            ExpirationTime = expirationTime;
        }
    }
}