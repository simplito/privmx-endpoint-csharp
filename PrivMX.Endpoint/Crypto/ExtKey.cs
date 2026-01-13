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
using PrivMX.Endpoint.Crypto.Internal;

namespace PrivMX.Endpoint.Crypto
{
    /// <summary>
    /// 'ExtKey' is a class representing Extended keys and operations on it.
    /// </summary>
    public class ExtKey : IExtKey
    {
        public readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new ExtKeyNative());
        
        /// <summary>
        /// Creates ExtKey from given seed.
        /// </summary>
        /// <param name="seed">The seed used to generate Key</param>
        /// <returns>ExtKey object</returns>
        public static ExtKey FromSeed(byte[] seed)
        {
            ExtKey extKey = new ExtKey();
            extKey = extKey.executor.Execute<ExtKey>(extKey.ptr, (int)ExtKeyNative.Method.FromSeed, new List<object?> { seed } );
            return extKey;
        }
        
        /// <summary>
        /// Decodes ExtKey from Base58 format.
        /// </summary>
        /// <param name="base58">The ExtKey in Base58</param>
        /// <returns>ExtKey object</returns>
        public static ExtKey FromBase58(byte[] base58)
        {
            ExtKey extKey = new ExtKey();
            extKey = extKey.executor.Execute<ExtKey>(extKey.ptr, (int)ExtKeyNative.Method.FromBase58, new List<object?> { base58 } );
            return extKey;
        }

        /// <summary>
        /// Generates a new ExtKey.
        /// </summary>
        /// <returns>ExtKey object</returns>
        public static ExtKey GenerateRandom()
        {
            ExtKey extKey = new ExtKey();
            extKey = extKey.executor.Execute<ExtKey>(extKey.ptr, (int)ExtKeyNative.Method.GenerateRandom, new List<object?> { } );
            return extKey;
        }
        
        private ExtKey()
        {
            ExtKeyNative.privmx_endpoint_newExtKey(out ptr);
        }

        ~ExtKey()
        {
            ExtKeyNative.privmx_endpoint_freeExtKey(ptr);
        }

        /// <summary>
        /// Generates child ExtKey from a current ExtKey using BIP32.
        /// </summary>
        /// <param name="index">Number from 0 to 2^31-1</param>
        /// <returns>ExtKey object</returns>
        public IExtKey Derive(uint index)
        {
            return executor.Execute<ExtKey>(ptr, (int)ExtKeyNative.Method.Derive, new List<object?> { index } );
        }

        /// <summary>
        /// Generates hardened child ExtKey from a current ExtKey using BIP32.
        /// </summary>
        /// <param name="index">Number from 0 to 2^31-1</param>
        /// <returns>ExtKey object</returns>
        public IExtKey DeriveHardened(uint index)
        {
            return executor.Execute<ExtKey>(ptr , (int)ExtKeyNative.Method.DeriveHardened, new List<object?> { index } );
        }

        /// <summary>
        /// Converts ExtKey to Base58 string.
        /// </summary>
        /// <returns>ExtKey in Base58 format</returns>
        public string GetPrivatePartAsBase58()
        {
            return executor.Execute<string>(ptr, (int)ExtKeyNative.Method.GetPrivatePartAsBase58, new List<object?> { });
        }

        /// <summary>
        /// Converts the public part of ExtKey to Base58 string.
        /// </summary>
        /// <returns>ExtKey in Base58 format</returns>
        public string GetPublicPartAsBase58()
        {
            return executor.Execute<string>(ptr, (int)ExtKeyNative.Method.GetPublicPartAsBase58, new List<object?> { });
        }

        /// <summary>
        /// Extracts ECC PrivateKey.
        /// </summary>
        /// <returns>ECC key in WIF format</returns>
        public string GetPrivateKey()
        {
            return executor.Execute<string>(ptr, (int)ExtKeyNative.Method.GetPrivateKey, new List<object?> { });
        }

        /// <summary>
        /// Extracts ECC PublicKey.
        /// </summary>
        /// <returns>ECC key in BASE58DER format</returns>
        public string GetPublicKey()
        {
            return executor.Execute<string>(ptr, (int)ExtKeyNative.Method.GetPublicKey, new List<object?> { });
        }

        /// <summary>
        /// Extracts ECC PublicKey Address.
        /// </summary>
        /// <returns>ECC Address in BASE58 format</returns>
        public byte[] GetPrivateEncKey()
        {
            return executor.Execute<byte[]>(ptr, (int)ExtKeyNative.Method.GetPrivateEncKey, new List<object?> { });
        }

        /// <summary>
        /// Extracts raw ECC PrivateKey.
        /// </summary>
        /// <returns>ECC PrivateKey</returns>
        public string GetPublicKeyAsBase58Address()
        {
            return executor.Execute<string>(ptr, (int)ExtKeyNative.Method.GetPublicKeyAsBase58Address, new List<object?> { });
        }

        /// <summary>
        /// Validates a signature of a message.
        /// </summary>
        /// <returns></returns>
        public byte[] GetChainCode()
        {
            return executor.Execute<byte[]>(ptr, (int)ExtKeyNative.Method.GetChainCode, new List<object?> { });
        }

        /// <summary>
        /// Validates a signature of a message.
        /// </summary>
        /// <param name="message">Data used on validation</param>
        /// <param name="signature">Signature of data to verify</param>
        /// <returns>Message validation result</returns>
        public bool VerifyCompactSignatureWithHash(byte[] message, byte[] signature)
        {
            return executor.ExecuteValue<bool>(ptr, (int)ExtKeyNative.Method.VerifyCompactSignatureWithHash, 
                new List<object?> { message, signature } );
        }

        /// <summary>
        /// Checks if ExtKey is Private.
        /// </summary>
        /// <returns>Returns true if ExtKey is private</returns>
        public bool IsPrivate()
        {
            return executor.ExecuteValue<bool>(ptr,  (int)ExtKeyNative.Method.IsPrivate, new List<object?> { });
        }
    }
}