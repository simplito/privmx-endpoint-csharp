using EndpointCSharpTests.Utils;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Crypto;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace EndpointCSharpTests
{
    internal class CryptoTest : BaseTest
    {
        private CryptoApi cryptoApi = null;

        [SetUp]
        public virtual void Setup()
        {
            cryptoApi = CryptoApi.Create();
        }

        private byte[] HexToByteArray(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        [Test, Order(1), Description("Add data to signature and verify it.")]
        public void VerifySignData()
        {
            byte[] sign = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("1bf40aa304605ed8f94f3dc03564332f341a7620f419839c762151c50342366377207cf547e1c3837e796dd1b29e13d0772b3b9d0d53315b28fba7a4ed385d1fc4");
            byte[] data = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data");
            string pubKey = "8Qsc1FF9xQp3ziWLEVpAoAp4RcpBpiQ4E9oBbuKfwdqRC5KpHq";
            string privKey = "L2TUveYrXgohLcLVcvrYd48Nwy25cZNGEuGYjxwWnai2uW9KNpPb";

            bool dataSigned = false;
            bool status = false;

            try
            {
                sign = cryptoApi.SignData(data, privKey);
                dataSigned = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"SignData failed.\nMessage: {e.Message}");
            }
            Assert.That(dataSigned, Is.True);

            try
            {
                status = cryptoApi.VerifySignature(data, sign, pubKey);
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"VerifySignature failed.\nMessage: {e.Message}");
            }
            Assert.That(status, Is.True);
        }

        // privmx::crypto::PrivateKey::fromWIF(keyInWIF); not checked
        [Test, Order(2), Description("Generate the private key.")]
        public void GeneratePrivateKey()
        {
            string keyWIF = string.Empty;

            try
            {
                keyWIF = cryptoApi.GeneratePrivateKey(null);
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"GeneratePrivateKey failed.\nMessage: {e.Message}");
            }
        }

        // privmx::crypto::PrivateKey::fromWIF(keyInWIF); not checked
        [Test, Order(2), Description("Derive the private key.")]
        public void DerivePrivateKey()
        {
            string keyWIF = string.Empty;

            try
            {
                keyWIF = cryptoApi.DerivePrivateKey("pass", "salt");
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DerivePrivateKey failed.\nMessage: {e.Message}");
            }
            Assert.That(keyWIF, Is.EqualTo("L2TUveYrXgohLcLVcvrYd48Nwy25cZNGEuGYjxwWnai2uW9KNpPb"));
        }

        // privmx::crypto::PublicKey::fromBase58DER(keyInBase58DER) not checked
        [Test, Order(3), Description("Derive the public key.")]
        public void DerivePublicKey()
        {
            string keyInBase58DER = string.Empty;

            try
            {
                keyInBase58DER = cryptoApi.DerivePublicKey("L2TUveYrXgohLcLVcvrYd48Nwy25cZNGEuGYjxwWnai2uW9KNpPb");
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DerivePrivateKey failed.\nMessage: {e.Message}");
            }
            Assert.That(keyInBase58DER, Is.EqualTo("8Qsc1FF9xQp3ziWLEVpAoAp4RcpBpiQ4E9oBbuKfwdqRC5KpHq"));
        }

        [Test, Order(4), Description("Generate key symmetric.")]
        public void GenerateKeySymmetric()
        {
            try
            {
                cryptoApi.GenerateKeySymmetric();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"GenerateKeySymmetric failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(5), Description("Encrypt data symmetric, decrypt data symmetric.")]
        public void EncryptDataSymmetric_DecryptDataSymmetric()
        {
            byte[] key = HexToByteArray("3ad696c8c37f286adbbd66b2f31e90041850ae2d3ec30250020c0209085f8c62");
            byte[] data = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data");
            byte[] encryptedData;
            byte[] decyptedData;

            try
            {
                encryptedData = cryptoApi.EncryptDataSymmetric(data, key);

                try
                {
                    decyptedData = cryptoApi.DecryptDataSymmetric(encryptedData, key);

                    Assert.That(decyptedData, Is.EqualTo(data));
                }
                catch (EndpointNativeException e)
                {
                    Assert.Fail($"DecryptDataSymmetric failed.\nMessage: {e.Message}");
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"EncryptDataSymmetric failed.\nMessage: {e.Message}");
            }
        }

        // privmx::crypto::PrivateKey::fromWIF(keyInWIF); not checked
        [Test, Order(6), Description("Encrypt data symmetric, decrypt data symmetric.")]
        public void CovertPEWtoWIFKey()
        {
            string keyInWIF = string.Empty;
            string PEMkey = string.Empty;
            PEMkey += "-----BEGIN EC PRIVATE KEY-----\n";
            PEMkey += "MHcCAQEEIDn+OxAnJ2hpn6DvIKPd7pZP7+icpLeob5rgkfqhhvvgoAoGCCqGSM49\n";
            PEMkey += "AwEHoUQDQgAEpjMTeBBo5FaUueJ2xdkVNDaxYYnl3PGkUMvlel20gGLuQJ8PubAd\n";
            PEMkey += "UEgv4yQFIxwLTNp7QlYqdaQTRbGjAblu9g==\n";
            PEMkey += "-----END EC PRIVATE KEY-----\n";

            try
            {
                keyInWIF = cryptoApi.ConvertPEMKeytoWIFKey(PEMkey);
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"ConvertPEMKeytoWIFKey failed.\nMessage: {e.Message}");
            }
            Assert.That(keyInWIF, Is.EqualTo("KyASahKYZjCyKJBB7ixVQbrQ7o56Vxo2PJgCuTL3YLFGBqxfPFAC"));
        }
    }
}
