using EndpointCSharpTests.Utils;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Store;
using PrivMX.Endpoint.Store.Models;
using PrivMX.Endpoint.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using File = PrivMX.Endpoint.Store.Models.File;

namespace EndpointCSharpTests
{
    internal class StoreTest : BaseTest
    {
        private Connection connection = null;
        private StoreApi storeApi = null;

        [SetUp]
        public virtual void Setup()
        {
            ConnectAs(ref connection, ConnectionType.User1);
            storeApi = StoreApi.Create(connection);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Disconnect(ref connection);
        }

        [Test, Order(0), Description("Get store by providing incorrect input data. 1 try")]
        public void GetStore_Incorrect()
        {
            Store store = new Store();
            bool didGetStore_IncorrectStoreId = false;

            // incorrect storeId
            try
            {
                store = storeApi.GetStore(config.Read("contextId", "Context_1"));
                didGetStore_IncorrectStoreId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to get store. Try: incorrect storeId.\nMessage: {e.Message}");
            }
            Assert.That(didGetStore_IncorrectStoreId, Is.False);
        }

        [Test, Order(1), Description("Get store by providing correct input data. 1 try")]
        public void GetStore_Correct()
        {
            Store store = new Store();
            bool didGetStore_CorrectStoreId = false;

            // correct storeId
            try
            {
                store = storeApi.GetStore(config.Read("storeId", "Store_1"));
                didGetStore_CorrectStoreId = true;

                Assert.Multiple(() =>
                {
                    Assert.That(store.ContextId, Is.EqualTo(config.Read("contextId", "Store_1")));
                    Assert.That(store.StoreId, Is.EqualTo(config.Read("storeId", "Store_1")));
                    Assert.That(store.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Store_1"))));
                    Assert.That(store.Creator, Is.EqualTo(config.Read("creator", "Store_1")));
                    Assert.That(store.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Store_1"))));
                    Assert.That(store.LastFileDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastFileDate", "Store_1"))));
                    Assert.That(store.LastModifier, Is.EqualTo(config.Read("lastModifier", "Store_1")));
                    Assert.That(store.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Store_1"))));
                    Assert.That(store.FilesCount, Is.EqualTo((long)Convert.ToDouble(config.Read("filesCount", "Store_1"))));
                    Assert.That(store.StatusCode, Is.EqualTo(0));
                    Assert.That(ByteArrayToString(store.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Store_1")));
                    Assert.That(ByteArrayToString(store.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Store_1")));
                    Assert.That(ByteArrayToString(store.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Store_1")));
                    Assert.That(ByteArrayToString(store.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Store_1")));

                    Assert.That(store.Version, Is.EqualTo(1));
                    Assert.That(store.Creator, Is.EqualTo(config.Read("user_1_id", "Login")));
                    Assert.That(store.LastModifier, Is.EqualTo(config.Read("user_1_id", "Login")));
                    Assert.That(store.Users, Has.Count.EqualTo(1));
                    if (store.Users.Count == 1)
                    {
                        Assert.That(store.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(store.Managers, Has.Count.EqualTo(1));
                    if (store.Managers.Count == 1)
                    {
                        Assert.That(store.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to get store. Try: correct storeId.\nMessage: {e.Message}");
            }
            Assert.That(didGetStore_CorrectStoreId, Is.True);
        }

        [Test, Order(2), Description("List stores by providing incorrect input data. 5 tries")]
        public void ListStores_IncorrectInputData()
        {
            bool didListStores_IncorrectContextId = false;
            bool didListStores_LimitLessThan0 = false;
            bool didListStores_LimitEqual0 = false;
            bool didListStores_IncorrectSortOrder = false;
            bool didListStores_IncorrectLastId = false;

            // incorrect contextId
            try
            {
                storeApi.ListStores(config.Read("storeId", "Store_1"), SetPagingQuery(0, 1, "desc"));
                didListStores_IncorrectContextId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list stores. Try: incorrect contextId\nMessage: {e.Message}");
            }
            Assert.That(didListStores_IncorrectContextId, Is.False);

            // limit < 0
            try
            {
                storeApi.ListStores(config.Read("contextId", "Context_1"), SetPagingQuery(0, -1, "desc"));
                didListStores_LimitLessThan0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list stores. Try: limit < 0.\nMessage: {e.Message}");
            }
            Assert.That(didListStores_LimitLessThan0, Is.False);   

            // limit == 0
            try
            {
                storeApi.ListStores(config.Read("contextId", "Context_1"), SetPagingQuery(0, 0, "desc"));
                didListStores_LimitEqual0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list stores. Try: limit == 0.\nMessage: {e.Message}");
            }
            Assert.That(didListStores_LimitEqual0 , Is.False);

            // incorrect sortOrder
            try
            {
                storeApi.ListStores(config.Read("contextId", "Context_1"), SetPagingQuery(0, 0, "BLACH"));
                didListStores_IncorrectSortOrder = true;    
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list stores. Try: incorrect sortOrder.\nMessage: {e.Message}");
            }
            Assert.That(didListStores_IncorrectSortOrder , Is.False);

            // incorrect lastId
            try
            {
                storeApi.ListStores(config.Read("contextId", "Context_1"), 
                    SetPagingQuery(0, 1, "BLACH", config.Read("contextId", "Context_1")));
                didListStores_IncorrectLastId = true;            
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list stores. Try: incorrect lastId.\nMessage: {e.Message}");
            }
            Assert.That(didListStores_IncorrectLastId , Is.False);
        }

        [Test, Order(3), Description("List stores by providing correct input data. 3 tries")]
        public void ListStores_CorrectInputData()
        {
            // {.skip=4, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<Store> listStores = storeApi.ListStores(config.Read("contextId", "Context_1"), SetPagingQuery(4, 1, "desc"));

                Assert.Multiple(() =>
                {
                    Assert.That(listStores.TotalAvailable, Is.EqualTo(3));
                    Assert.That(listStores.ReadItems, Has.Count.EqualTo(0));
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list stores. Try 1.\nMessage: {e.Message}");
            }

            // {.skip=0, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<Store> listStores = storeApi.ListStores(config.Read("contextId", "Context_1"), SetPagingQuery(0, 1, "desc"));

                Assert.Multiple(() =>
                {
                    Assert.That(listStores.TotalAvailable, Is.EqualTo(3));
                    Assert.That(listStores.ReadItems, Has.Count.EqualTo(1));
                    if(listStores.ReadItems.Count >= 1)
                    {
                        Store store = listStores.ReadItems[0];
                        Assert.That(store.ContextId, Is.EqualTo(config.Read("contextId", "Store_3")));
                        Assert.That(store.StoreId, Is.EqualTo(config.Read("storeId", "Store_3")));
                        Assert.That(store.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Store_3"))));
                        Assert.That(store.Creator, Is.EqualTo(config.Read("creator", "Store_3")));
                        Assert.That(store.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Store_3"))));
                        Assert.That(store.LastFileDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastFileDate", "Store_3"))));
                        Assert.That(store.LastModifier, Is.EqualTo(config.Read("lastModifier", "Store_3")));
                        Assert.That(store.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Store_3"))));
                        Assert.That(store.FilesCount, Is.EqualTo((long)Convert.ToDouble(config.Read("filesCount", "Store_3"))));
                        Assert.That(store.StatusCode, Is.EqualTo(0));
                        Assert.That(ByteArrayToString(store.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Store_3")));
                        Assert.That(ByteArrayToString(store.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Store_3")));
                        Assert.That(ByteArrayToString(store.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Store_3")));
                        Assert.That(ByteArrayToString(store.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Store_3")));
                        Assert.That(store.Version, Is.EqualTo(1));
                        Assert.That(store.Creator, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(store.LastModifier, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(store.Users, Has.Count.EqualTo(2));
                        if(store.Users.Count == 2)
                        {
                            Assert.That(store.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(store.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                        Assert.That(store.Managers, Has.Count.EqualTo(1));
                        if (store.Users.Count == 1)
                        {
                            Assert.That(store.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        }
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list stores. Try 2.\nMessage: {e.Message}");
            }

            // {.skip=1, .limit=3, .sortOrder="asc"}
            try
            {
                PagingList<Store> listStores = storeApi.ListStores(config.Read("contextId", "Context_1"), SetPagingQuery(1, 3, "asc"));

                Assert.Multiple(() =>
                {
                    Assert.That(listStores.TotalAvailable, Is.EqualTo(3));
                    Assert.That(listStores.ReadItems, Has.Count.EqualTo(2));
                    if (listStores.ReadItems.Count >= 1)
                    {
                        Store store = listStores.ReadItems[0];
                        Assert.That(store.ContextId, Is.EqualTo(config.Read("contextId", "Store_2")));
                        Assert.That(store.StoreId, Is.EqualTo(config.Read("storeId", "Store_2")));
                        Assert.That(store.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Store_2"))));
                        Assert.That(store.Creator, Is.EqualTo(config.Read("creator", "Store_2")));
                        Assert.That(store.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Store_2"))));
                        Assert.That(store.LastFileDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastFileDate", "Store_2"))));
                        Assert.That(store.LastModifier, Is.EqualTo(config.Read("lastModifier", "Store_2")));
                        Assert.That(store.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Store_2"))));
                        Assert.That(store.FilesCount, Is.EqualTo((long)Convert.ToDouble(config.Read("filesCount", "Store_2"))));
                        Assert.That(store.StatusCode, Is.EqualTo(0));
                        Assert.That(ByteArrayToString(store.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Store_2")));
                        Assert.That(ByteArrayToString(store.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Store_2")));
                        Assert.That(ByteArrayToString(store.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Store_2")));
                        Assert.That(ByteArrayToString(store.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Store_2")));
                        Assert.That(store.Version, Is.EqualTo(1));
                        Assert.That(store.Creator, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(store.LastModifier, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(store.Users, Has.Count.EqualTo(2));
                        if (store.Users.Count == 2)
                        {
                            Assert.That(store.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(store.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                        Assert.That(store.Managers, Has.Count.EqualTo(2));
                        if (store.Users.Count == 2)
                        {
                            Assert.That(store.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(store.Managers[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                    }
                    if (listStores.ReadItems.Count >= 2)
                    {
                        Store store = listStores.ReadItems[1];
                        Assert.That(store.ContextId, Is.EqualTo(config.Read("contextId", "Store_3")));
                        Assert.That(store.StoreId, Is.EqualTo(config.Read("storeId", "Store_3")));
                        Assert.That(store.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Store_3"))));
                        Assert.That(store.Creator, Is.EqualTo(config.Read("creator", "Store_3")));
                        Assert.That(store.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Store_3"))));
                        Assert.That(store.LastFileDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastFileDate", "Store_3"))));
                        Assert.That(store.LastModifier, Is.EqualTo(config.Read("lastModifier", "Store_3")));
                        Assert.That(store.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Store_3"))));
                        Assert.That(store.FilesCount, Is.EqualTo((long)Convert.ToDouble(config.Read("filesCount", "Store_3"))));
                        Assert.That(store.StatusCode, Is.EqualTo(0));
                        Assert.That(ByteArrayToString(store.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Store_3")));
                        Assert.That(ByteArrayToString(store.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Store_3")));
                        Assert.That(ByteArrayToString(store.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Store_3")));
                        Assert.That(ByteArrayToString(store.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Store_3")));
                        Assert.That(store.Version, Is.EqualTo(1));
                        Assert.That(store.Creator, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(store.LastModifier, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(store.Users, Has.Count.EqualTo(2));
                        if (store.Users.Count == 2)
                        {
                            Assert.That(store.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(store.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                        Assert.That(store.Managers, Has.Count.EqualTo(1));
                        if (store.Users.Count == 1)
                        {
                            Assert.That(store.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        }
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list stores. Try 3.\nMessage: {e.Message}");
            }
        }

        // error - invalidNumberOfParams
        [Test, Order(4), Description("Create store - 4 incorrect tries")]
        public void CreateStore_Incorrect()
        {
            string storeId = string.Empty;
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            bool didCreate_IncorrectContectId = false;
            bool didCreate_IncorrectUsers = false;
            bool didCreate_IncorrectManagers = false;
            bool didCreate_NoManagers = false;

            // incorrect contextId
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeId = storeApi.CreateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta
                );
                didCreate_IncorrectContectId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create store failed. Try: incorrect contextId.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_IncorrectContectId, Is.False);

            // incorrect users
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeId = storeApi.CreateStore(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta
                );
                didCreate_IncorrectUsers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create store failed. Try: incorrect users.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_IncorrectUsers, Is.False);

            // incorrect managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeId = storeApi.CreateStore(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta
                );
                didCreate_IncorrectManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create store failed. Try: incorrect managers.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_IncorrectManagers, Is.False);

            // no managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeId = storeApi.CreateStore(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                    },
                    publicMeta,
                    privateMeta
                );
                didCreate_NoManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create store failed. Try: no managers.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_NoManagers, Is.False);
        }

        // error - invalidNumberOfParams
        [Test, Order(5), Description("Create store - 2 correct tries.")]
        public void CreateStore_Correct()
        {
            string storeId = string.Empty;
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            bool didCreate_DifUsersAndManagers = false;
            bool didCreate_SameUsersAndManagers = false;

            // different users and managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeId = storeApi.CreateStore(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta
                );
                didCreate_DifUsersAndManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create store failed. Try: different users and managers.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_DifUsersAndManagers, Is.True);

            try
            {
                Store store = storeApi.GetStore(storeId);
                Assert.Multiple(() =>
                {
                    Assert.That(store.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(store.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(store.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(store.Users, Has.Count.EqualTo(1));
                    if (store.Users.Count == 1)
                    {
                        Assert.That(store.Users[0], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    Assert.That(store.Managers, Has.Count.EqualTo(1));
                    if (store.Managers.Count == 1)
                    {
                        Assert.That(store.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Get store failed.\nMessage: {e.Message}");
            }

            // same users and managers
            storeId = string.Empty;
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeId = storeApi.CreateStore(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta
                );
                didCreate_SameUsersAndManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create store failed. Try: same users and managers.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_SameUsersAndManagers, Is.True);

            try
            {
                Store store = storeApi.GetStore(storeId);
                Assert.Multiple(() =>
                {
                    Assert.That(store.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(store.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(store.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(store.Users, Has.Count.EqualTo(1));
                    if (store.Users.Count == 1)
                    {
                        Assert.That(store.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(store.Managers, Has.Count.EqualTo(1));
                    if (store.Managers.Count == 1)
                    {
                        Assert.That(store.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Get store failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(6), Description("Update store by providing incorrect input data. 5 tries.")]
        public void UpdateStore_IncorrectInput()
        {
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            bool didUpdate_IncorrectStoreId = false;
            bool didUpdate_IncorrectUsers = false;
            bool didUpdate_IncorrectManagers = false;
            bool didUpdate_NoManagers = false;
            bool didUpdate_IncorrectVersion = false;

            // incorrect storeId
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    1,
                    false,
                    false
                );
                didUpdate_IncorrectStoreId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update store failed. Try: incorrect contextId.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectStoreId, Is.False);

            // incorrect users
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    1,
                    false,
                    false
                );
                didUpdate_IncorrectUsers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update store failed. Try: incorrect users.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectUsers, Is.False);

            // incorrect managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    1,
                    false,
                    false
                );
                didUpdate_IncorrectManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update store failed. Try: incorrect managers.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectManagers, Is.False);

            // no managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                    },
                    publicMeta,
                    privateMeta,
                    1,
                    false,
                    false
                );
                didUpdate_NoManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update store failed. Try: no managers.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_NoManagers, Is.False);

            //incorrect version force false
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    2,
                    false,
                    false
                );
                didUpdate_IncorrectVersion = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update store failed. Try: incorrect version.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectVersion, Is.False);
        }

        // error - invalidNumberOfParams
        [Test, Order(7), Description("Update store by providing correct input data. 4 tries.")]
        public void UpdateStore_CorrectInput()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didUpdate_NewUsers = false;
            bool didUpdate_NewManagers = false;
            bool didUpdate_LessUsers = false;
            bool didUpdate_LessManagers = false;

            // new users
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        },
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    1,
                    false,
                    false
                );
                didUpdate_NewUsers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update store failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_NewUsers, Is.True);

            try
            {
                Store store = storeApi.GetStore(config.Read("threadId", "Thread_1"));
                Assert.Multiple(() =>
                {
                    Assert.That(store.StoreId, Is.EqualTo(config.Read("storeId", "Store_1")));
                    Assert.That(store.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(store.Version, Is.EqualTo(2));
                    Assert.That(store.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(store.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(store.Users, Has.Count.EqualTo(2));
                    if (store.Users.Count == 2)
                    {
                        Assert.That(store.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(store.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    Assert.That(store.Managers, Has.Count.EqualTo(1));
                    if (store.Managers.Count == 1)
                    {
                        Assert.That(store.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting store failed.\nMessage: {e.Message}");
            }

            // new managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        },
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        },
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    2,
                    false,
                    false
                );
                didUpdate_NewManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update store failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_NewManagers, Is.True);

            try
            {
                Store store = storeApi.GetStore(config.Read("storeId", "Store_1"));
                Assert.Multiple(() =>
                {
                    Assert.That(store.StoreId, Is.EqualTo(config.Read("storeId", "Store_1")));
                    Assert.That(store.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(store.Version, Is.EqualTo(3));
                    Assert.That(store.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(store.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(store.Users, Has.Count.EqualTo(2));
                    if (store.Users.Count == 2)
                    {
                        Assert.That(store.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(store.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    Assert.That(store.Managers, Has.Count.EqualTo(2));
                    if (store.Managers.Count == 2)
                    {
                        Assert.That(store.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(store.Managers[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting store failed.\nMessage: {e.Message}");
            }

            // less users
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_2"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        },
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    1,
                    false,
                    false
                );
                didUpdate_LessUsers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update store failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_LessUsers, Is.True);

            try
            {
                Store store = storeApi.GetStore(config.Read("storeId", "Store_2"));
                Assert.Multiple(() =>
                {
                    Assert.That(store.StoreId, Is.EqualTo(config.Read("storeId", "Store_2")));
                    Assert.That(store.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(store.Version, Is.EqualTo(2));
                    Assert.That(store.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(store.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(store.Users, Has.Count.EqualTo(1));
                    if (store.Users.Count == 1)
                    {
                        Assert.That(store.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(store.Managers, Has.Count.EqualTo(1));
                    if (store.Managers.Count == 1)
                    {
                        Assert.That(store.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(store.Managers[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting store failed.\nMessage: {e.Message}");
            }

            // less managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_2"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    2,
                    false,
                    false
                );
                didUpdate_LessManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update store failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_LessManagers, Is.True);

            try
            {
                Store store = storeApi.GetStore(config.Read("storeId", "Store_2"));
                Assert.Multiple(() =>
                {
                    Assert.That(store.StoreId, Is.EqualTo(config.Read("storeId", "Store_2")));
                    Assert.That(store.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(store.Version, Is.EqualTo(3));
                    Assert.That(store.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(store.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(store.Users, Has.Count.EqualTo(1));
                    if (store.Users.Count == 1)
                    {
                        Assert.That(store.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(store.Managers, Has.Count.EqualTo(1));
                    if (store.Managers.Count == 1)
                    {
                        Assert.That(store.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting store failed.\nMessage: {e.Message}");
            }
        }

        // error - connection problems while disconnecting from user 1 and connecting to user 2 afterwards
        [Test, Order(8), Description("Delete store - 2 incorrect tries.")]
        public void DeleteStore_Incorrect()
        {
            bool didDelete_IncorrectStoreId = false;
            bool didDelete_AsUser = false;

            // incorrect storeId
            try
            {
                storeApi.DeleteStore(config.Read("contextId", "Context_1"));
                didDelete_IncorrectStoreId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete store failed. Try: Incorrect storeId.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_IncorrectStoreId, Is.False);

            // as user
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User2);
            storeApi = StoreApi.Create(connection);
            try
            {
                storeApi.DeleteStore(config.Read("storeId", "Store_3"));
                didDelete_AsUser = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete store failed. Try: as user.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_AsUser, Is.False);
        }

        [Test, Order(9), Description("Delete store - 1 correct try.")]
        public void DeleteStore_Correct()
        {
            bool didDelete_AsManager = false;

            // as manager
            try
            {
                storeApi.DeleteStore(config.Read("storeId", "Store_1"));
                didDelete_AsManager = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete store failed. Try: as manager.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_AsManager, Is.True);
        }

        [Test, Order(10), Description("Get file from store. 1 incorrect try")]
        public void GetFile_Incorrect()
        {
            bool didGetFile_IncorrectFileId = false;

            // incorrect fileId
            try
            {
                storeApi.GetFile(config.Read("contextId", "Context_1"));
                didGetFile_IncorrectFileId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Get file failed. Try: incorrect fileId.\nMessage: {e.Message}");
            }
            Assert.That(didGetFile_IncorrectFileId, Is.False);
        }

        //serialize file to json wrong?, error caused by the update function - invalidNumberOfParams
        [Test, Order(11), Description("Get file from store. 2 correct tries")]
        public void GetFile_Correct()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didGetFile_CorrcetFileId = false;
            bool didGetFile_AfterForceKeyGenOnStore = false;

            // correct fileId
            try
            {
                File file = storeApi.GetFile(config.Read("info_fileId", "File_1"));
                didGetFile_CorrcetFileId = true;

                Assert.Multiple(() =>
                {
                    Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("info_storeId", "File_1")));
                    Assert.That(file.Info.FileId, Is.EqualTo(config.Read("info_fileId", "File_1")));
                    Assert.That(file.Info.CreateDate, Is.EqualTo(StringToInt64(config.Read("info_createDate", "File_1"))));
                    Assert.That(file.Info.Author, Is.EqualTo(config.Read("info_author", "File_1")));
                    Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "File_1")));
                    Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "File_1")));
                    Assert.That(file.AuthorPubKey, Is.EqualTo(config.Read("authorPubKey", "File_1")));
                    Assert.That(file.StatusCode, Is.EqualTo(0));
                    //Assert.That(System.Text.Json.JsonSerializer.Serialize(file), Is.EqualTo(config.Read("JSON_data", "File_1")));
                    Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "File_1")));
                    Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "File_1")));
                    Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("uploaded_size", "File_1"))));
                });
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Get file failed. Try: correct fileId.\nMessage: {e.Message}");
            }
            Assert.That(didGetFile_CorrcetFileId, Is.True);

            // after force key generation on store
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    1,
                    true,
                    true
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update store failed.\nMessage: {e.Message}");
            }

            try
            {
                File file = storeApi.GetFile(config.Read("info_fileId", "File_1"));
                didGetFile_AfterForceKeyGenOnStore = true;

                Assert.Multiple(() =>
                {
                    Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("info_storeId", "File_1")));
                    Assert.That(file.Info.FileId, Is.EqualTo(config.Read("info_fileId", "File_1")));
                    Assert.That(file.Info.CreateDate, Is.EqualTo(StringToInt64(config.Read("info_createDate", "File_1"))));
                    Assert.That(file.Info.Author, Is.EqualTo(config.Read("info_author", "File_1")));
                    Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "File_1")));
                    Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "File_1")));
                    Assert.That(file.AuthorPubKey, Is.EqualTo(config.Read("authorPubKey", "File_1")));
                    Assert.That(file.StatusCode, Is.EqualTo(0));
                    //Assert.That(System.Text.Json.JsonSerializer.Serialize(file), Is.EqualTo(config.Read("JSON_data", "File_1")));
                    Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "File_1")));
                    Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "File_1")));
                    Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("uploaded_size", "File_1"))));
                });
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Get file failed. Try: after force key generation.\nMessage: {e.Message}");
            }
            Assert.That(didGetFile_AfterForceKeyGenOnStore, Is.True);
        }

        [Test, Order(12), Description("List files by providing incorrect input data, 5 tries.")]
        public void ListFiles_IncorrectInputData()
        {
            bool didListFiles_IncorrectStoreId = false;
            bool didListFiles_LimitLessThan0 = false;
            bool didListFiles_Limit0 = false;
            bool didListFiles_IncorrectSortOrder = false;
            bool didListFiles_IncorrectLastId = false;

            // incorrect storeId
            try
            {
                storeApi.ListFiles(config.Read("info_fileId", "File_1"), SetPagingQuery(0, 1, "desc"));
                didListFiles_IncorrectStoreId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list files. Try: incorrect storeId.\nMessage: {e.Message}");
            }
            Assert.That(didListFiles_IncorrectStoreId, Is.False);

            // limit < 0
            try
            {
                storeApi.ListFiles(config.Read("info_storeId", "Store_1"), SetPagingQuery(0, -1, "desc"));
                didListFiles_LimitLessThan0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list files. Try: limit < 0.\nMessage: {e.Message}");
            }
            Assert.That(didListFiles_LimitLessThan0, Is.False);

            // limit == 0
            try
            {
                storeApi.ListFiles(config.Read("info_storeId", "Store_1"), SetPagingQuery(0, 0, "desc"));
                didListFiles_Limit0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list files. Try: limit == 0.\nMessage: {e.Message}");
            }
            Assert.That(didListFiles_Limit0, Is.False);

            // incorrect sortOrder
            try
            {
                storeApi.ListFiles(config.Read("info_storeId", "Store_1"), SetPagingQuery(0, 1, "BLACH"));
                didListFiles_IncorrectSortOrder = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list files. Try: incorrect sortOrder.\nMessage: {e.Message}");
            }
            Assert.That(didListFiles_IncorrectSortOrder, Is.False);

            // incorrect lastId
            try
            {
                storeApi.ListFiles(config.Read("info_storeId", "Store_1"), SetPagingQuery(0, 1, "desc", config.Read("storeId", "Store_1")));
                didListFiles_IncorrectLastId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list files. Try: incorrect lastId.\nMessage: {e.Message}");
            }
            Assert.That(didListFiles_IncorrectLastId, Is.False);
        }

        // serialize file to json wrong?, error caused by the update function - invalidNumberOfParams
        [Test, Order(13), Description("List files by providing correct input data, 3 tries.")]
        public void ListFiles_CorrectInputData()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            // {.skip=4, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<File> listFiles = storeApi.ListFiles(
                    config.Read("storeId", "Store_1"),
                    SetPagingQuery(4, 1, "desc")
                );

                Assert.That(listFiles.TotalAvailable, Is.EqualTo(2));
                Assert.That(listFiles.ReadItems, Has.Count.EqualTo(0));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Listing files failed. Try 1.\nMessage: {e.Message}");
            }

            // {.skip=1, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<File> listFiles = storeApi.ListFiles(
                    config.Read("storeId", "Store_1"),
                    SetPagingQuery(1, 1, "desc")
                );

                Assert.That(listFiles.TotalAvailable, Is.EqualTo(2));
                Assert.That(listFiles.ReadItems, Has.Count.EqualTo(1));
                if(listFiles.ReadItems.Count >= 1)
                {
                    File file = listFiles.ReadItems[0];
                    Assert.Multiple(() =>
                    {
                        Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("info_storeId", "File_1")));
                        Assert.That(file.Info.FileId, Is.EqualTo(config.Read("info_fileId", "File_1")));
                        Assert.That(file.Info.CreateDate, Is.EqualTo(StringToInt64(config.Read("info_createDate", "File_1"))));
                        Assert.That(file.Info.Author, Is.EqualTo(config.Read("info_author", "File_1")));
                        Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "File_1")));
                        Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "File_1")));
                        Assert.That(file.AuthorPubKey, Is.EqualTo(config.Read("authorPubKey", "File_1")));
                        Assert.That(file.StatusCode, Is.EqualTo(0));
                        //Assert.That(System.Text.Json.JsonSerializer.Serialize(file), Is.EqualTo(config.Read("JSON_data", "File_1")));
                        Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "File_1")));
                        Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "File_1")));
                        Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("uploaded_size", "File_1"))));
                    });
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Listing files failed. Try 2.\nMessage: {e.Message}");
            }

            // {.skip=0, .limit=3, .sortOrder="asc"}, after force key generation on store
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    1,
                    true,
                    true
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update store failed.\nMessage: {e.Message}");
            }

            try
            {
                PagingList<File> listFiles = storeApi.ListFiles(
                    config.Read("storeId", "Store_1"),
                    SetPagingQuery(0, 3, "asc")
                );

                Assert.That(listFiles.TotalAvailable, Is.EqualTo(2));
                Assert.That(listFiles.ReadItems, Has.Count.EqualTo(2));
                if (listFiles.ReadItems.Count >= 1)
                {
                    File file = listFiles.ReadItems[0];
                    Assert.Multiple(() =>
                    {
                        Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("info_storeId", "File_1")));
                        Assert.That(file.Info.FileId, Is.EqualTo(config.Read("info_fileId", "File_1")));
                        Assert.That(file.Info.CreateDate, Is.EqualTo(StringToInt64(config.Read("info_createDate", "File_1"))));
                        Assert.That(file.Info.Author, Is.EqualTo(config.Read("info_author", "File_1")));
                        Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "File_1")));
                        Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "File_1")));
                        Assert.That(file.AuthorPubKey, Is.EqualTo(config.Read("authorPubKey", "File_1")));
                        Assert.That(file.StatusCode, Is.EqualTo(0));
                        //Assert.That(System.Text.Json.JsonSerializer.Serialize(file), Is.EqualTo(config.Read("JSON_data", "File_1")));
                        Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "File_1")));
                        Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "File_1")));
                        Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("uploaded_size", "File_1"))));
                    });
                }
                if (listFiles.ReadItems.Count >= 2)
                {
                    File file = listFiles.ReadItems[1];
                    Assert.Multiple(() =>
                    {
                        Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("info_storeId", "File_2")));
                        Assert.That(file.Info.FileId, Is.EqualTo(config.Read("info_fileId", "File_2")));
                        Assert.That(file.Info.CreateDate, Is.EqualTo(StringToInt64(config.Read("info_createDate", "File_2"))));
                        Assert.That(file.Info.Author, Is.EqualTo(config.Read("info_author", "File_2")));
                        Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "File_2")));
                        Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "File_2")));
                        Assert.That(file.AuthorPubKey, Is.EqualTo(config.Read("authorPubKey", "File_2")));
                        Assert.That(file.StatusCode, Is.EqualTo(0));
                        //Assert.That(System.Text.Json.JsonSerializer.Serialize(file), Is.EqualTo(config.Read("JSON_data", "File_2")));
                        Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "File_2")));
                        Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "File_2")));
                        Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("uploaded_size", "File_2"))));
                    });
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Listing files failed. Try 3.\nMessage: {e.Message}");
            }
        }

        [Test, Order(14), Description("Delefe file, 2 incorrect tries")]
        public void DeleteFile_Incorrect()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didDelete_IncorrectFileId = false;
            bool didDelete_UserNotAuthToDeleteFile = false;

            // incorrect fileId
            try
            {
                storeApi.DeleteFile(config.Read("storeId", "Store_1"));
                didDelete_IncorrectFileId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete file failed. Try: Incorrect fileId.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_IncorrectFileId, Is.False);

            // change privileges
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        },
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    1,
                    true,
                    false
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update store failed.\nMessage: {e.Message}");
            }
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User2);
            storeApi = StoreApi.Create(connection);
            // as user not created by me
            try
            {
                storeApi.DeleteFile(config.Read("info_fileId", "File_2"));
                didDelete_UserNotAuthToDeleteFile = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete file failed. Try:User not auth to delete file.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_UserNotAuthToDeleteFile, Is.False);
        }

        // error caused by the update function - invalidNumberOfParams
        [Test, Order(15), Description("Delefe file, 2 correct tries")]
        public void DeleteFile_Correct()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didDelete_UserAuthToDeleteFile = false;
            bool didDelete_AsManager = false;

            // change privileges
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User1);
            storeApi = StoreApi.Create(connection);
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        },
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        },
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    1,
                    true,
                    false
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update store failed.\nMessage: {e.Message}");
            }
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User2);
            storeApi = StoreApi.Create(connection);
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        },
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    1,
                    true,
                    false
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update store failed.\nMessage: {e.Message}");
            }

            // as user created by me
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User1);
            storeApi = StoreApi.Create(connection);
            try
            {
                storeApi.DeleteFile(config.Read("info_fileId", "File_2"));
                didDelete_UserAuthToDeleteFile = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete file failed. Try: user auth to delete file.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_UserAuthToDeleteFile, Is.True);

            // as manager no created by me
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User2);
            storeApi = StoreApi.Create(connection);
            try
            {
                storeApi.DeleteFile(config.Read("info_fileId", "File_1"));
                didDelete_AsManager = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete file failed. Try: as manager.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_AsManager, Is.True);
        }

        [Test, Order(16), Description("Open file - 1 incorrect try")]
        public void OpenFile_Incorrect()
        {
            bool didOpen = false;

            // openFile incorrect fileId
            try
            {
                storeApi.OpenFile(config.Read("storeId", "Store_1"));
                didOpen = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't open the file.\nMessage: {e.Message}");
            }
            Assert.That(didOpen, Is.False);
        }

        /*Invalid request exception - on openFile ??*/
        [Test, Order(17), Description("Open file - 1 correct try")]
        public void OpenFile_Correct()
        {
            bool didOpen = false;

            // openFile correct fileId
            try
            {
                storeApi.OpenFile(config.Read("info_fileId", "File_1"));
                didOpen = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't open the file.\nMessage: {e.Message}");
            }
            Assert.That(didOpen, Is.True);
        }

        [Test, Order(18), Description("Read file - 1 incorrect try")]
        public void ReadFile_Incorrect()
        {
            bool didRead = false;

            // readFromFile incorrect handle (not exist)
            try
            {
                storeApi.ReadFromFile(1, 2);
                didRead = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't read from file.\nMessage: {e.Message}");
            }
            Assert.That(didRead, Is.False);
        }

        [Test, Order(19), Description("Read file - 2 correct tries")]
        public void ReadFile_Correct()
        {
            long handle = 0;
            byte[] data = [];

            bool didRead_FileLenEqFileSize = false;
            bool didRead_LengthHalfTheFileSize = false;

            // open file to get the handle
            try
            {
                handle = storeApi.OpenFile(config.Read("info_fileId", "File_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't open the file.\nMessage: {e.Message}");
            }

            // readFromFile length == file.size
            try
            {
                storeApi.ReadFromFile(handle, StringToInt64(config.Read("size", "File_1")));
                didRead_FileLenEqFileSize = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't read the file (length == fileSize).\nMessage: {e.Message}");
            }
            Assert.That(didRead_FileLenEqFileSize, Is.True);

            // readFromFile length == 50% file.size
            try
            {
                data = storeApi.ReadFromFile(handle, StringToInt64(config.Read("size", "File_1")) / 2);
                didRead_LengthHalfTheFileSize = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't seek in file (length == 50% fileSize).\nMessage: {e.Message}");
            }
            Assert.Multiple(() =>
            {
                Assert.That(didRead_LengthHalfTheFileSize, Is.True);
                Assert.That(ByteArrayToString(data), Is.EqualTo(config.Read("uploaded_data_inHex", "File_1")));
            });
        }

        [Test, Order(20), Description("Seek file - 4 incorrect tries")]
        public void SeekFile_Incorrect()
        {
            long handle = 0;

            bool didSeek_IncorrectHandle = false;
            bool didSeek_PosLessThan0 = false;
            bool didSeek_PosMoreThanFileSize = false;
            bool didSeek_PosHalfTheFileSize = false;

            // open file to get the handle
            try
            {
                handle = storeApi.OpenFile(config.Read("info_fileId", "File_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't open the file.\nMessage: {e.Message}");
            }

            // seekInFile incorrect handle (not exist)
            try
            {
                storeApi.SeekInFile(1, 2);
                didSeek_IncorrectHandle = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't seek in file.\nMessage: {e.Message}");
            }
            Assert.That(didSeek_IncorrectHandle, Is.False);

            // seekInFile pos < 0
            try
            {
                storeApi.SeekInFile(handle, -1);
                didSeek_PosLessThan0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't seek the file (pos < 0).\nMessage: {e.Message}");
            }
            Assert.That(didSeek_PosLessThan0, Is.False);

            // seekInFile pos > file.size
            try
            {
                storeApi.SeekInFile(handle, StringToInt64(config.Read("size", "File_1")) + 1);
                didSeek_PosMoreThanFileSize = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't seek the file (pos > fileSize).\nMessage: {e.Message}");
            }
            Assert.That(didSeek_PosMoreThanFileSize, Is.False);

            // seekInFile pos == 50% file.size
            try
            {
                storeApi.SeekInFile(handle, StringToInt64(config.Read("size", "File_1")) / 2);
                didSeek_PosHalfTheFileSize = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't seek the file (pos =  1/2 fileSize).\nMessage: {e.Message}");
            }
            Assert.That(didSeek_PosHalfTheFileSize, Is.False);
        }

        [Test, Order(21), Description("Seek file - 1 correct try")]
        public void SeekFile_Correct()
        {
            long handle = 0;

            bool didSeek_PosEq0 = false;

            // open file to get the handle
            try
            {
                handle = storeApi.OpenFile(config.Read("info_fileId", "File_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't open the file.\nMessage: {e.Message}");
            }

            // seekInFile pos == 0
            try
            {
                storeApi.SeekInFile(handle, 0);
                didSeek_PosEq0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't seek in file (pos == 0).\nMessage: {e.Message}");
            }
            Assert.That(didSeek_PosEq0, Is.True);
        }

        [Test, Order(22), Description("Create file - 2 incorrect tries")]
        public void CreateFile_Incorrect()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didCreate_IncorrectStoreId = false;
            bool didCreate_SizeLessThan0 = false;

            // createFile incorrect storeId
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.CreateFile(
                    config.Read("contextId", "Context_1"),
                    publicMeta,
                    privateMeta,
                    64
                );
                didCreate_IncorrectStoreId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't create file. Try: Incorrect storeId.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_IncorrectStoreId, Is.False);

            // createFile size < 0
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.CreateFile(
                    config.Read("storeId", "Store_1"),
                    publicMeta,
                    privateMeta,
                    -1
                );
                didCreate_SizeLessThan0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't create file. Try: Incorrect storeId.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_SizeLessThan0 , Is.False);
        }

        [Test, Order(23), Description("Create file - 1 correct try")]
        public void CreateFile_Correct()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didCreate_CorrextStoreId = false;
            bool didCreate_FileSize0 = false;

            // createFile correct storeId
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.CreateFile(
                    config.Read("storeId", "Store_1"),
                    publicMeta,
                    privateMeta,
                    64
                );
                didCreate_CorrextStoreId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't create file.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_CorrextStoreId, Is.True);
        }

        [Test, Order(24), Description("Close file - 1 incorrect try")]
        public void CloseFile_Incorrect()
        {
            bool didClose_IncorrectHandle = false;

            // closeFile incorrect handle
            try
            {
                storeApi.CloseFile(0);
                didClose_IncorrectHandle = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't close the file.\nMessage: {e.Message}");
            }
            Assert.That(didClose_IncorrectHandle, Is.False);
        }

        [Test, Order(25), Description("Close file - 1 correct try")]
        public void CloseFile_Correct()
        {
            long handle = 0;
            bool didClose = false;

            // open file to get the handle
            try
            {
                handle = storeApi.OpenFile(config.Read("info_fileId", "File_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't open the file.\nMessage: {e.Message}");
            }

            // closeFile
            try
            {
                storeApi.CloseFile(handle);
                didClose = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't close the file.\nMessage: {e.Message}");
            }
            Assert.That(didClose, Is.True);
        }

        [Test, Order(26), Description("Update file - 2 incorrect tries")]
        public void UpdateFile_Incorrect()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didUpdate_IncorrectFileId = false;
            bool didUpdate_SizeLessThan0 = false;

            // updateFile incorrect fileId
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.UpdateFile(
                    config.Read("contextId", "Context_1"),
                    publicMeta,
                    privateMeta,
                    64
                );
                didUpdate_IncorrectFileId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't update the file.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectFileId, Is.False);

            // updateFile size < 0
            try
            {
                storeApi.UpdateFile(
                    config.Read("info_fileId", "File_1"),
                    publicMeta,
                    privateMeta,
                    -1
                );
                didUpdate_SizeLessThan0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't update the file.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_SizeLessThan0, Is.False);
        }

        [Test, Order(27), Description("Update file - 0 correct tries")]
        public void UpdateFile_Correct()
        {
            try
            {

            }
            catch (EndpointNativeException e)
            {

            }
        }

        [Test, Order(28), Description("Write to file - 3 incorrect tries")]
        public void WriteToFile_Incorrect()
        {
            long handle = 0;
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] data = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("BLACH");
            string totalDataSent = "";

            bool didWrite_HandleNotExist = false;
            bool didWrite_IncorrectHandle = false;
            bool didWrite_TotalSizeLargerThanDecSize = false;
            bool didWrite_TotalSizeLessThanDecSize = false;

            // writeToFile incorrect handle (no exist)
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.WriteToFile(
                    0,
                    data
                );
                didWrite_HandleNotExist = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't write to file.\nMessage: {e.Message}");
            }
            Assert.That(didWrite_HandleNotExist, Is.False);

            // open file to get the handle
            try
            {
                handle = storeApi.OpenFile(config.Read("info_fileId", "File_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't open the file.\nMessage: {e.Message}");
            }

            // writeToFile incorrect handle (read handle)
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                storeApi.WriteToFile(
                    handle,
                    data
                );
                didWrite_IncorrectHandle = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't write to file.\nMessage: {e.Message}");
            }
            Assert.That(didWrite_IncorrectHandle, Is.False);

            // create new file and try to write to it
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                handle = storeApi.CreateFile(
                    config.Read("storeId", "Store_1"),
                    publicMeta,
                    privateMeta,
                    128 * 1024 * 1024
                );
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't create file.\nMessage: {e.Message}");
            }
            Assert.That(handle, Is.EqualTo(1));

            if (handle == 1)
            {
                // writeToFile total.size < declared size <-FALSE
                try
                {
                    for (int i = 0; i < 1024 * 64; i++)
                    {
                        // msg total data bigger then 1MB
                        byte[] randomData = new byte[1024];
                        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                        {
                            rng.GetBytes(randomData);
                        };

                        storeApi.WriteToFile(handle, randomData);
                        totalDataSent += randomData;
                    }
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Couldn't write to file.\nMessage: {e.Message}");
                }

                try
                {
                    // closeFile size < declared size
                    storeApi.CloseFile(handle);
                    didWrite_TotalSizeLargerThanDecSize = true;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Couldn't write to file.\nMessage: {e.Message}");
                }
                Assert.That(didWrite_TotalSizeLargerThanDecSize, Is.False);

                // writeToFile total.size > declared <-FALSE
                try
                {
                    byte[] randomData = new byte[1];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(randomData);
                    };
                    storeApi.WriteToFile(handle, randomData);
                    didWrite_TotalSizeLessThanDecSize = true;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Couldn't write to file.\nMessage: {e.Message}");
                }
                Assert.That(didWrite_TotalSizeLessThanDecSize, Is.False);

            }
        }

        [Test, Order(29), Description("Write to file - 2 correct tries")]
        public void WriteToFile_Correct()
        {
            long handle = 0;
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            string fileId = "";
            string totalDataSent = "";

            bool didWrite_NewFile = false;
            bool didWrite_UpdatedFile = false;

            // create new file and try to write to it with size = 128*1024*1024
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                handle = storeApi.CreateFile(
                    config.Read("storeId", "Store_1"),
                    publicMeta,
                    privateMeta,
                    128 * 1024 * 1024
                );
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't create file.\nMessage: {e.Message}");
            }
            Assert.That(handle, Is.EqualTo(1));

            if(handle == 1)
            {
                // writeToFile total.size == declared
                try
                {
                    byte[] randomData = new byte[1024 * 64];
                    for (int i = 0; i < 1024 * 64; i++)
                    {
                        // msg total data bigger then 1MB
                        byte[] randomDataTmp = new byte[1024];
                        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                        {
                            rng.GetBytes(randomDataTmp);
                        };
                        randomData = randomData.Concat(randomDataTmp).ToArray();
                    }
                    storeApi.WriteToFile(handle, randomData);
                    totalDataSent += randomData;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Couldn't write to file.\nMessage: {e.Message}");
                }

                try
                {
                    // closeFile size == declared size
                    fileId = storeApi.CloseFile(handle);
                    didWrite_NewFile = true;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Couldn't close the file.\nMessage: {e.Message}");
                }
                Assert.That(didWrite_NewFile, Is.True);

                // validate uploaded data
                if (fileId != "") 
                {
                    try
                    {
                        File file = storeApi.GetFile(fileId);
                        long readHandle;

                        if(file.Info.FileId != string.Empty)
                        {
                            Assert.Multiple(() =>
                            {
                                Assert.That(file.Info.FileId, Is.EqualTo(fileId));
                                Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(ByteArrayToString(publicMeta)));
                                Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(ByteArrayToString(privateMeta)));
                                Assert.That(file.Size, Is.EqualTo(128 * 1024 * 1024));
                            });
                        }

                        try
                        {
                            readHandle = storeApi.OpenFile(fileId);
                            string total_data_read = "";

                            for (int i = 0; i < 1024 * 128; i++)
                            {
                                total_data_read += storeApi.ReadFromFile(readHandle, 1024);
                            }
                            Assert.That(totalDataSent, Has.Length.EqualTo(total_data_read.Length));
                        }
                        catch (EndpointNativeException e)
                        {
                            Assert.Fail($"Couldn't read file.\nMessage: {e.Message}");
                        }
                    }
                    catch (EndpointNativeException e)
                    {
                        Assert.Fail($"Couldn't get file.\nMessage: {e.Message}");
                    }
                }
            }

            // update new file and try to write to it with size = 128*1024*1024
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                handle = storeApi.UpdateFile(
                    config.Read("storeId", "Store_1"),
                    publicMeta,
                    privateMeta,
                    128 * 1024 * 1024
                );
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't create file.\nMessage: {e.Message}");
            }
            Assert.That(handle, Is.EqualTo(1));

            if (handle == 1)
            {
                // writeToFile total.size == declared
                try
                {
                    byte[] randomData = new byte[1024 * 64];
                    for (int i = 0; i < 1024 * 64; i++)
                    {
                        // msg total data bigger then 1MB
                        byte[] randomDataTmp = new byte[1024];
                        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                        {
                            rng.GetBytes(randomDataTmp);
                        };
                        randomData = randomData.Concat(randomDataTmp).ToArray();
                    }
                    storeApi.WriteToFile(handle, randomData);
                    totalDataSent += randomData;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Couldn't write to file.\nMessage: {e.Message}");
                }

                try
                {
                    // closeFile size == declared size
                    fileId = storeApi.CloseFile(handle);
                    didWrite_UpdatedFile = true;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Couldn't close the file.\nMessage: {e.Message}");
                }
                Assert.That(didWrite_UpdatedFile, Is.True);

                // validate uploaded data
                if (fileId != "")
                {
                    try
                    {
                        File file = storeApi.GetFile(fileId);
                        long readHandle;

                        if (file.Info.FileId != string.Empty)
                        {
                            Assert.Multiple(() =>
                            {
                                Assert.That(file.Info.FileId, Is.EqualTo(fileId));
                                Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(ByteArrayToString(publicMeta)));
                                Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(ByteArrayToString(privateMeta)));
                                Assert.That(file.Size, Is.EqualTo(128 * 1024 * 1024));
                            });
                        }

                        try
                        {
                            readHandle = storeApi.OpenFile(fileId);
                            string total_data_read = "";

                            for (int i = 0; i < 1024 * 128; i++)
                            {
                                total_data_read += storeApi.ReadFromFile(readHandle, 1024);
                            }
                            Assert.That(totalDataSent, Has.Length.EqualTo(total_data_read.Length));
                        }
                        catch (EndpointNativeException e)
                        {
                            Assert.Fail($"Couldn't read file.\nMessage: {e.Message}");
                        }
                    }
                    catch (EndpointNativeException e)
                    {
                        Assert.Fail($"Couldn't get file.\nMessage: {e.Message}");
                    }
                }
            }
        }
    }
}
