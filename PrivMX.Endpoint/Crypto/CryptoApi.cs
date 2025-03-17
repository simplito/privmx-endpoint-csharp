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

using PrivMX.Endpoint.Core.Internal;
using PrivMX.Endpoint.Crypto.Internal;
using System;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Crypto
{
    public class CryptoApi : ICryptoApi
    {
        private readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new CryptoApiNative());

        /// <summary>
        /// Creates an instance of the <see cref="CryptoApi"/>.
        /// </summary>
        /// <returns>Created instance of the <see cref="CryptoApi"/>.</returns>
        static public CryptoApi Create()
        {
            CryptoApi cryptoApi = new CryptoApi();
            cryptoApi.executor.ExecuteVoid(cryptoApi.ptr, (int)CryptoApiNative.Method.Create, new List<object>{});
            return cryptoApi;
        }

        private CryptoApi()
        {
            CryptoApiNative.privmx_endpoint_newCryptoApi(out ptr);
        }

        ~CryptoApi()
        {
            CryptoApiNative.privmx_endpoint_freeCryptoApi(ptr);
        }

        /// <summary>
        /// Creates a signature of data using given key.
        /// </summary>
        /// <param name="data">Data to sign.</param>
        /// <param name="privateKey">The private key used to sign data.</param>
        /// <returns>Signature of data.</returns>
        public byte[] SignData(byte[] data, string privateKey)
        {
            return executor.Execute<byte[]>(ptr, (int)CryptoApiNative.Method.SignData, new List<object>{data, privateKey});
        }

        /// <summary>
        /// Verifies a signature of data using given key.
        /// </summary>
        /// <param name="data">Signed data.</param>
        /// <param name="signature">Signature of data.</param>
        /// <param name="publicKey">The public key used to verify the signature.</param>
        /// <returns>Verification status.</returns>
        public bool VerifySignature(byte[] data, byte[] signature, string publicKey)
        {
            return executor.Execute<bool>(ptr, (int)CryptoApiNative.Method.VerifySignature, new List<object>{data, signature, publicKey});
        }

        /// <summary>
        /// Generates a new random private key.
        /// 
        /// The returned key is private key of elliptic curve cryptography.
        /// </summary>
        /// <param name="randomSeed">(optional) String used as the seed of random generator.</param>
        /// <returns>Generated private key in WIF format.</returns>
        public string GeneratePrivateKey(string randomSeed = null)
        {
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.GeneratePrivateKey, new List<object>{randomSeed});
        }

        /// <summary>
        /// Derives a private key from a password and salt.
        /// 
        /// The returned key is private key of elliptic curve cryptography. PBKDF2 algorithm is used to derive the key.
        /// 
        /// This method is deprecated. Use <see cref="CryptoApi.DerivePrivateKey2"/> method instead.
        /// </summary>
        /// <param name="password">The password used to derive from.</param>
        /// <param name="salt">The random additional data used to derive.</param>
        /// <returns>Derived private key in WIF format.</returns>
        [Obsolete("Use CryptoApi.DerivePrivateKey2() instead")]
        public string DerivePrivateKey(string password, string salt)
        {
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.DerivePrivateKey, new List<object>{password, salt});
        }

        /// <summary>
        /// Derives a private key from a password and salt.
        /// 
        /// The returned key is private key of elliptic curve cryptography. PBKDF2 algorithm is used to derive the key.
        /// 
        /// Compared to <see cref="CryptoApi.DerivePrivateKey"/> method, this version of the derive function has an increased number of rounds.
        /// This makes using this function a safer choice, but it makes the derived key different than in the previous version.
        /// </summary>
        /// <param name="password">The password used to derive from.</param>
        /// <param name="salt">The random additional data used to derive.</param>
        /// <returns>Derived private key in WIF format.</returns>
        public string DerivePrivateKey2(string password, string salt)
        {
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.DerivePrivateKey2, new List<object>{password, salt});
        }

        /// <summary>
        /// Derives public key from a private key.
        /// 
        /// The returned key is public key of elliptic curve cryptography.
        /// </summary>
        /// <param name="privateKey">The private key in WIF format.</param>
        /// <returns>Public key in Base58 format.</returns>
        public string DerivePublicKey(string privateKey)
        {
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.DerivePublicKey, new List<object>{privateKey});
        }

        /// <summary>
        /// Generates a new random key for symmetric cryptography.
        /// </summary>
        /// <returns>Generated symmetric key.</returns>
        public byte[] GenerateKeySymmetric()
        {
            return executor.Execute<byte[]>(ptr, (int)CryptoApiNative.Method.GenerateKeySymmetric, new List<object>{});
        }

        /// <summary>
        /// Encrypts data using a symmetric key.
        /// 
        /// AES algorithm is used to encrypt data.
        /// </summary>
        /// <param name="data">Data to encrypt.</param>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <returns>Encrypted data.</returns>
        public byte[] EncryptDataSymmetric(byte[] data, byte[] symmetricKey)
        {
            return executor.Execute<byte[]>(ptr, (int)CryptoApiNative.Method.EncryptDataSymmetric, new List<object>{data, symmetricKey});
        }

        /// <summary>
        /// Decrypts data using a symmetric key.
        /// </summary>
        /// <param name="data">Data to decrypt.</param>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <returns>Decrypted data.</returns>
        public byte[] DecryptDataSymmetric(byte[] data, byte[] symmetricKey)
        {
            return executor.Execute<byte[]>(ptr, (int)CryptoApiNative.Method.DecryptDataSymmetric, new List<object>{data, symmetricKey});
        }

        /// <summary>
        /// Converts a private key in PEM format to WIF format.
        /// </summary>
        /// <param name="pemKey">The private key in PEM format.</param>
        /// <returns>Converted private key to WIF format.</returns>
        public string ConvertPEMKeytoWIFKey(string pemKey)
        {
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.ConvertPEMKeytoWIFKey, new List<object>{pemKey});
        }
    }
}
