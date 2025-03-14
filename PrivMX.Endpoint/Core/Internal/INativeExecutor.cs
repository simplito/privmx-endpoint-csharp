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

using System;

namespace PrivMX.Endpoint.Core.Internal
{
    internal interface INativeExecutor
    {
        int Exec(IntPtr ptr, int method, IntPtr value, out IntPtr result);
    }
}
