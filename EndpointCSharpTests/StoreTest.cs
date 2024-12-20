using EndpointCSharpTests.Utils;
using PrivMX.Endpoint.Core;
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

        [Test, Description("Get store, 2 tries: 1 incorrect, 1 correct.")]
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

        [Test, Description("List stores by providing incorrect input data. 5 tries")]
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
    }
}
