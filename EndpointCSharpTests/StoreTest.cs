using EndpointCSharpTests.Utils;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Store;
using PrivMX.Endpoint.Store.Models;
using PrivMX.Endpoint.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Test, Order(0), Description("Get store, 2 tries: 1 incorrect, 1 correct.")]
        public void GetStore()
        {
            Store store = new Store();
            bool didGetStore_IncorrectStoreId = false;
            bool didGetStore_CorrectStoreId = false;

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
                    if(store.Users.Count == 1)
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

        [Test, Order(1), Description("List stores by providing incorrect input data. 5 tries")]
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

        [Test, Order(2), Description("List stores by providing correct input data. 3 tries")]
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

        [Test, Order(3), Description("Create store - 6 tries: 5 incorrect, 1 correct.")]
        public void CreateStore()
        {
            string storeId = string.Empty;
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            bool didCreate_IncorrectContectId = false;
            bool didCreate_IncorrectUsers = false;
            bool didCreate_IncorrectManagers = false;
            bool didCreate_NoManagers = false;
            bool didCreate_DifUsersAndManagers = false;
            bool didCreate_SameUsersAndManagers = false;

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

        [Test, Order(4), Description("Update store by providing incorrect input data. 5 tries.")]
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

        [Test, Order(5), Description("Update store by providing correct input data. 4 tries")]
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
    }
}
