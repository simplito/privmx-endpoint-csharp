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

namespace PrivMX.Endpoint.Crypto
{
    public interface ICryptoApi
    {
        byte[] SignData(byte[] data, byte[] privateKey);
        bool VerifySignature(byte[] data, byte[] signature, string publicKey);
        byte[] GeneratePrivateKey(byte[]? randomSeed = null);
        [Obsolete("Use ICryptoApi.DerivePrivateKey2() instead")]
        byte[] DerivePrivateKey(byte[] password, byte[] salt);
        byte[] DerivePrivateKey2(byte[] password, byte[] salt);
        string DerivePublicKey(byte[] privateKey);
        byte[] GenerateKeySymmetric();
        byte[] EncryptDataSymmetric(byte[] data, byte[] symmetricKey);
        byte[] DecryptDataSymmetric(byte[] data, byte[] symmetricKey);
        byte[] ConvertPEMKeytoWIFKey(byte[] pemKey);
    }
}
