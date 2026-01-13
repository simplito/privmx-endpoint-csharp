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

namespace PrivMX.Endpoint.Crypto
{
    public interface IExtKey
    {
        IExtKey Derive(uint index);
        IExtKey DeriveHardened(uint index);
        string GetPrivatePartAsBase58();
        string GetPublicPartAsBase58();
        string GetPrivateKey();
        string GetPublicKey();
        byte[] GetPrivateEncKey();
        string GetPublicKeyAsBase58Address();
        byte[] GetChainCode();
        bool VerifyCompactSignatureWithHash(byte[] Message, byte[] Signature);
        bool IsPrivate();
    }
}