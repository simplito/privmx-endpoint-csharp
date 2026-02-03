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
using PrivMX.Endpoint.Crypto.Models;

namespace PrivMX.Endpoint.Crypto
{
    /// <summary>
    /// 'CryptoApi' is a class representing Endpoint's API for cryptographic operations.
    /// </summary>
    public class CryptoApi : ICryptoApi
    {
        public readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new CryptoApiNative());

        /// <summary>
        /// Creates an instance of the <see cref="CryptoApi"/>.
        /// </summary>
        /// <returns>Created instance of the <see cref="CryptoApi"/>.</returns>
        public static CryptoApi Create()
        {
            CryptoApi cryptoApi = new CryptoApi();
            cryptoApi.executor.ExecuteVoid(cryptoApi.ptr, (int)CryptoApiNative.Method.Create, new List<object?> { });
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
            return executor.Execute<byte[]>(ptr, (int)CryptoApiNative.Method.SignData, 
                new List<object?> { data, privateKey });
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
            return executor.ExecuteValue<bool>(ptr, (int)CryptoApiNative.Method.VerifySignature, 
                new List<object?> { data, signature, publicKey });
        }

        /// <summary>
        /// Generates a new private ECC key.
        /// </summary>
        /// <param name="randomSeed">(optional) string used as the base to generate the new key</param>
        /// <returns>generated ECC key in WIF format</returns>
        public string GeneratePrivateKey(string? randomSeed = null)
        {
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.GeneratePrivateKey, 
                new List<object?> { randomSeed });
        }

        /// <summary>
        /// Generates a new private ECC key from a password using pbkdf2.
        /// 
        /// This method is deprecated. Use <see cref="CryptoApi.DerivePrivateKey2"/> method instead.
        /// </summary>
        /// <param name="password">The password used to derive from.</param>
        /// <param name="salt">The random additional data used to derive.</param>
        /// <returns>Derived private key in WIF format.</returns>
        [Obsolete("Use CryptoApi.DerivePrivateKey2() instead")]
        public string DerivePrivateKey(string password, string salt)
        {
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.DerivePrivateKey, 
                new List<object?> { password, salt });
        }

        /// <summary>
        /// Generates a new private ECC key from a password using pbkdf2. 
        /// This version of the derive function has a rounds count increased to 200k. This makes using this function
        /// a safer choice, but it makes the received key different than in the original version.
        /// </summary>
        /// <param name="password">The password used to derive from.</param>
        /// <param name="salt">The random additional data used to derive.</param>
        /// <returns>Derived private key in WIF format.</returns>
        public string DerivePrivateKey2(string password, string salt)
        {
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.DerivePrivateKey2, 
                new List<object?> { password, salt });
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
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.DerivePublicKey, 
                new List<object?> { privateKey });
        }

        /// <summary>
        /// Generates a new random key for symmetric cryptography.
        /// </summary>
        /// <returns>Generated symmetric key.</returns>
        public byte[] GenerateKeySymmetric()
        {
            return executor.Execute<byte[]>(ptr, (int)CryptoApiNative.Method.GenerateKeySymmetric, 
                new List<object?> {});
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
            return executor.Execute<byte[]>(ptr, (int)CryptoApiNative.Method.EncryptDataSymmetric, 
                new List<object?> { data, symmetricKey });
        }

        /// <summary>
        /// Decrypts data using a symmetric key.
        /// </summary>
        /// <param name="data">Data to decrypt.</param>
        /// <param name="symmetricKey">The symmetric key.</param>
        /// <returns>Decrypted data.</returns>
        public byte[] DecryptDataSymmetric(byte[] data, byte[] symmetricKey)
        {
            return executor.Execute<byte[]>(ptr, (int)CryptoApiNative.Method.DecryptDataSymmetric, 
                new List<object?> { data, symmetricKey });
        }

        /// <summary>
        /// Converts a private key in PEM format to WIF format.
        /// </summary>
        /// <param name="pemKey">The private key in PEM format.</param>
        /// <returns>Converted private key to WIF format.</returns>
        public string ConvertPEMKeytoWIFKey(string pemKey)
        {
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.ConvertPEMKeytoWIFKey, 
                new List<object?> { pemKey });
        }

        /// <summary>
        /// Converts given public key in PGP format to its base58DER format.
        /// </summary>
        /// <param name="pgpKey">public key to convert</param>
        /// <returns></returns>
        public string ConvertPGPAsn1KeyToBase58DERKey(string pgpKey)
        {
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.ConvertPGPAsn1KeyToBase58DERKey, 
                new List<object?> { pgpKey });
        }

        /// <summary>
        /// Generates ECC key and BIP-39 mnemonic from a password using BIP-39.
        /// </summary>
        /// <param name="strength">Size of BIP-39 entropy, must be a multiple of 32 between 128 and 256.</param>
        /// <param name="password">The password used to generate the Key</param>
        /// <returns>BIP39_t object containing ECC Key and associated with it BIP-39 mnemonic and entropy</returns>
        public BIP39 GenerateBip39(UIntPtr strength, string password = "")
        {
            return executor.Execute<BIP39>(ptr, (int)CryptoApiNative.Method.GenerateBip39,
                new List<object?> { strength, password });
        }

        /// <summary>
        /// Generates ECC key using BIP-39 mnemonic.
        /// </summary>
        /// <param name="mnemonic">The BIP-39 mnemonic used to generate the Key</param>
        /// <param name="password">The password used to generate the Key</param>
        /// <returns>BIP39_t object containing ECC Key and associated with it BIP-39 mnemonic and entropy</returns>
        public BIP39 FromMnemonic(string mnemonic, string password = "")
        {
            return executor.Execute<BIP39>(ptr, (int)CryptoApiNative.Method.FromMnemonic,
                new List<object?> { mnemonic, password });
        }

        /// <summary>
        /// Generates ECC key using BIP-39 entropy.
        /// </summary>
        /// <param name="entropy">The BIP-39 entropy used to generate the Key</param>
        /// <param name="password">The password used to generate the Key</param>
        /// <returns>BIP39_t object containing ECC Key and associated with it BIP-39 mnemonic and entropy</returns>
        public BIP39 FromEntropy(byte[] entropy, string password = "")
        {
            return executor.Execute<BIP39>(ptr, (int)CryptoApiNative.Method.FromEntropy,
                new List<object?> { entropy, password });
        }

        /// <summary>
        /// Converts BIP-39 entropy to mnemonic.
        /// </summary>
        /// <param name="entropy">BIP-39 entropy</param>
        /// <returns>BIP-39 mnemonic</returns>
        public string EntropyToMnemonic(byte[] entropy)
        {
            return executor.Execute<string>(ptr, (int)CryptoApiNative.Method.EntropyToMnemonic,
                new List<object?> { entropy });
        }

        /// <summary>
        /// Converts BIP-39 mnemonic to entropy.
        /// </summary>
        /// <param name="mnemonic">mnemonic BIP-39 mnemonic</param>
        /// <returns>BIP-39 entropy</returns>
        public byte[] MnemonicToEntropy(string mnemonic)
        {
            return executor.Execute<byte[]>(ptr, (int)CryptoApiNative.Method.MnemonicToEntropy,
                new List<object?> { mnemonic });
        }

        /// <summary>
        /// Generates a seed used to generate a key using BIP-39 mnemonic with PBKDF2.
        /// </summary>
        /// <param name="mnemonic">BIP-39 mnemonic</param>
        /// <param name="password">The password used to generate the seed</param>
        /// <returns>Generated seed</returns>
        public byte[] MnemonicToSeed(string mnemonic, string password = "")
        {
            return executor.Execute<byte[]>(ptr, (int)CryptoApiNative.Method.MnemonicToSeed,
                new List<object?> { mnemonic, password });
        }
    }
}
