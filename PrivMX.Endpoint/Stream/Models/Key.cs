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

namespace PrivMX.Endpoint.Stream.Models
{
    public class Key
    {
        public string KeyId { get; set; }
        public byte[] key { get; set; }
        public KeyType Type { get; set; }
    }
}