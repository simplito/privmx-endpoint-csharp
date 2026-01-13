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

using System.Collections.Generic;

namespace PrivMX.Endpoint.Core
{
    public interface IUtils
    {
        string EncodeHex(byte[] data);
        byte[] DecodeHex(string data);
        bool IsHex(string data);
        string EncodeBase32(byte[] data);
        byte[] DecodeBase32(string data);
        bool IsBase32(string data);
        string EncodeBase64(byte[] data);
        byte[] DecodeBase64(string data);
        bool IsBase64(string data);
        string Trim(string data);
        List<string> Split(string data, string delimiter);
        void LTrim(string data);
        void RTrim(string data);
    }
}