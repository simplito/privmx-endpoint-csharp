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

namespace PrivMX.Endpoint.Core.Internal
{
    internal enum Method
    {
        WaitEvent                   = 0,
        GetEvent                    = 1,
        PlatformConnect             = 2,
        PlatformDisconnect          = 3,
        ContextList                 = 4,
        ThreadCreate                = 5,
        ThreadGet                   = 6,
        ThreadList                  = 7,
        ThreadMessageSend           = 8,
        ThreadMessagesGet           = 9,
        StoreList                   = 10,
        StoreGet                    = 11,
        StoreCreate                 = 12,
        StoreFileGet                = 13,
        StoreFileList               = 14,
        StoreFileCreate             = 15,
        StoreFileRead               = 16,
        StoreFileWrite              = 17,
        StoreFileDelete             = 18,
        CryptoPrivKeyNew            = 19,
        CryptoPubKeyNew             = 20,
        CryptoEncrypt               = 21,
        CryptoDecrypt               = 22,
        CryptoSign                  = 23,
        SetCertsPath                = 24,
        CryptoKeyConvertPEMToWIF    = 25,
        BackendRequest              = 26,
        SubscribeToChannel          = 27,
        UnsubscribeFromChannel      = 28,
        ThreadDelete                = 29,
        ThreadMessageDelete         = 30,
        ThreadMessageGet            = 31,
        StoreDelete                 = 32,
        StoreFileUpdate             = 33,
        StoreFileOpen               = 34,
        StoreFileSeek               = 35,
        StoreFileClose              = 36,
        CryptoPrivKeyNewPbkdf2      = 37
    }
}