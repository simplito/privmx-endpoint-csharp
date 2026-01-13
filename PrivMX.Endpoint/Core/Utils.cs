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
using System.Collections.Generic;
using PrivMX.Endpoint.Core.Internal;

namespace PrivMX.Endpoint.Core
{
    public class Utils : IUtils
    {
        public readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new UtilsNative());

        /// <summary>
        /// Creates an instance of the <see cref="Utils"/>.
        /// </summary>
        /// <returns>Created an instance of the <see cref="Utils"/>.</returns>
        static public Utils Create()
        {
            Utils utils = new Utils();
            return utils;
        }

        private Utils()
        {
            UtilsNative.privmx_endpoint_newUtils(out ptr);
        }

        ~Utils()
        {
            UtilsNative.privmx_endpoint_freeUtils(ptr);
        }
        
        /// <summary>
        /// Encodes buffer to a string in Hex format.
        /// </summary>
        /// <param name="data">byte array to encode</param>
        /// <returns>string in Hex format</returns>
        public string EncodeHex(byte[] data)
        {
            return executor.Execute<string>(ptr, (int)UtilsNative.Method.EncodeHex, new List<object?> { data });
        }

        /// <summary>
        /// Decodes string in Hex to byte array.
        /// </summary>
        /// <param name="data">string to decode</param>
        /// <returns>byte array with decoded data</returns>
        public byte[] DecodeHex(string data)
        {
            return executor.Execute<byte[]>(ptr, (int)UtilsNative.Method.DecodeHex, new List<object?> { data });
        }

        /// <summary>
        /// Checks if given string is in Hex format.
        /// </summary>
        /// <param name="data">string to check</param>
        /// <returns>Data check result</returns>
        public bool IsHex(string data)
        {
            return executor.ExecuteValue<bool>(ptr, (int)UtilsNative.Method.IsHex, new List<object?> { data });
        }

        /// <summary>
        /// Encodes buffer to string in Base32 format.
        /// </summary>
        /// <param name="data">byte array to encode</param>
        /// <returns>string in Base32 format</returns>
        public string EncodeBase32(byte[] data)
        {
            return executor.Execute<string>(ptr, (int)UtilsNative.Method.EncodeBase32, new List<object?> { data });
        }

        /// <summary>
        /// Decodes string in Base32 to byte array.
        /// </summary>
        /// <param name="data">string to decode</param>
        /// <returns>byte array with decoded data</returns>
        public byte[] DecodeBase32(string data)
        {
            return executor.Execute<byte[]>(ptr, (int)UtilsNative.Method.DecodeBase32, new List<object?> { data });
        }

        /// <summary>
        /// Checks if given string is in Base32 format.
        /// </summary>
        /// <param name="data">string to check</param>
        /// <returns>Data check result</returns>
        public bool IsBase32(string data)
        {
            return executor.ExecuteValue<bool>(ptr, (int)UtilsNative.Method.IsBase32, new List<object?> { data });
        }

        /// <summary>
        /// Encodes buffer to string in Base64 format.
        /// </summary>
        /// <param name="data">byte array to encode</param>
        /// <returns>string in Base64 format</returns>
        public string EncodeBase64(byte[] data)
        {
            return executor.Execute<string>(ptr, (int)UtilsNative.Method.EncodeBase64, new List<object?> { data });
        }

        /// <summary>
        /// Decodes string in Base64 to byte array.
        /// </summary>
        /// <param name="data">string to decode</param>
        /// <returns>byte array with decoded data</returns>
        public byte[] DecodeBase64(string data)
        {
            return executor.Execute<byte[]>(ptr, (int)UtilsNative.Method.DecodeBase64, new List<object?> { data });
        }

        /// <summary>
        /// Checks if given string is in Base64 format.
        /// </summary>
        /// <param name="data">string to check</param>
        /// <returns>Data check result</returns>
        public bool IsBase64(string data)
        {
            return executor.ExecuteValue<bool>(ptr, (int)UtilsNative.Method.IsBase64, new List<object?> { data });
        }

        /// <summary>
        /// Removes all trailing whitespaces.
        /// </summary>
        /// <param name="data">string to trim</param>
        /// <returns>copy of string with removed trailing whitespaces.</returns>
        public string Trim(string data)
        {
            return executor.Execute<string>(ptr, (int)UtilsNative.Method.Trim, new List<object?> { data });
        }

        /// <summary>
        /// Splits string by given delimiter (delimiter is removed).
        /// </summary>
        /// <param name="data">string to split</param>
        /// <param name="delimiter">delimiter string which will be split</param>
        /// <returns>List of all split parts</returns>
        public List<string> Split(string data, string delimiter)
        {
            return executor.Execute<List<string>>(ptr, (int)UtilsNative.Method.Split, new List<object?> { data, delimiter });
        }

        /// <summary>
        /// Removes all whitespace from the left of given string.
        /// </summary>
        /// <param name="data">Reference to string</param>
        public void LTrim(string data)
        {
            executor.ExecuteVoid(ptr, (int)UtilsNative.Method.Ltrim, new List<object?> { data });
        }

        /// <summary>
        /// Removes all whitespace from the right of given string.
        /// </summary>
        /// <param name="data">Reference to string</param>
        public void RTrim(string data)
        {
            executor.ExecuteVoid(ptr, (int)UtilsNative.Method.Rtrim, new List<object?> { data });
        }
    }
}