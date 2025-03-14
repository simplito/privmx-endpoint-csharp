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
        byte[] SignData(byte[] data, string privateKey);
        bool VerifySignature(byte[] data, byte[] signature, string publicKey);
        string GeneratePrivateKey(string randomSeed = null);
        [Obsolete("Use ICryptoApi.DerivePrivateKey2() instead")]
        string DerivePrivateKey(string password, string salt);
        string DerivePrivateKey2(string password, string salt);
        string DerivePublicKey(string privateKey);
        byte[] GenerateKeySymmetric();
        byte[] EncryptDataSymmetric(byte[] data, byte[] symmetricKey);
        byte[] DecryptDataSymmetric(byte[] data, byte[] symmetricKey);
        string ConvertPEMKeytoWIFKey(string pemKey);
    }
}
