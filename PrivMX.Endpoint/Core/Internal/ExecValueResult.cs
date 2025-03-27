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

namespace PrivMX.Endpoint.Core.Internal
{
    internal class ExecValueResult<T> where T : struct
    {
        public bool Status { get; set; }
        public T? Result { get; set; }
        public Error? Error { get; set; }
    }
}
