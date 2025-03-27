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

using PrivMX.Endpoint.Core.Models;

namespace PrivMX.Endpoint.Core
{
    /// <summary>
    /// This exception is thrown when an error occurs in the native library.
    /// </summary>
    public class EndpointNativeException : EndpointException
    {
        /// <summary>
        /// Error information consistent with error codes and error messages in the native library.
        /// </summary>
        public Error Error { get; set; }

        public EndpointNativeException(Error error)
            : base(string.Format("Code = {0}, Message = {1}, Name = {2}, Scope = {3}, Description = {4}, Full = {5}", error.Code, error.Message, error.Name, error.Scope, error.Description, error.Full))
        {
            Error = error;
        }
    }
}
