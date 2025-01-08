using EndpointCSharpTests.Utils;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Inbox;
using PrivMX.Endpoint.Inbox.Models;
using PrivMX.Endpoint.Store;
using PrivMX.Endpoint.Store.Models;
using PrivMX.Endpoint.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace EndpointCSharpTests
{
    internal class InboxTest : BaseTest
    {
        private Connection connection = null;
        private ThreadApi threadApi = null;
        private StoreApi storeApi = null;
        private InboxApi inboxApi = null;

        [SetUp]
        public virtual void Setup()
        {
            CustomConnect(ref connection, ConnectionType.User1);
        }

        [TearDown]
        public virtual void TearDown()
        {
            CustomDisconnect(ref connection);
        }

        private void CustomConnect(ref Connection connection, ConnectionType connectionType)
        {
            ConnectAs(ref connection, connectionType);
            threadApi = ThreadApi.Create(connection);
            storeApi = StoreApi.Create(connection);
            inboxApi = InboxApi.Create(connection, threadApi, storeApi);
        }

        private void CustomDisconnect(ref Connection connection)
        {
            Disconnect(ref connection);
            threadApi = null;
            storeApi = null;
            inboxApi = null;
        }

        [Test, Order(0), Description("Get inbox by providing incorrect input data. 1 try")]
        public void GetInbox_Incorrect()
        {
            bool didGetInbox_IncorrectId = false;

            try
            {
                inboxApi.GetInbox(config.Read("contextId", "Context_1"));
                didGetInbox_IncorrectId = true;
            }
            catch (EndpointNativeException e) 
            {
                Console.WriteLine($"Getting inbox failed. Try: incorrect id.\nMessage: {e.Message}");
            }
            Assert.That(didGetInbox_IncorrectId, Is.False);
        }

        [Test, Order(1), Description("Get inbox by providing correct input data. 1 try")]
        public void GetInbox_Correct()
        {
            bool didGetInbox = false;

            try
            {
                Inbox inbox = inboxApi.GetInbox(config.Read("inboxId", "Inbox_1"));
                didGetInbox = true;

                Assert.Multiple(() =>
                {
                    Assert.That(inbox.InboxId, Is.EqualTo(config.Read("inboxId", "Inbox_1")));
                    Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Inbox_1")));
                    Assert.That(inbox.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Inbox_1"))));
                    Assert.That(inbox.Creator, Is.EqualTo(config.Read("creator", "Inbox_1")));
                    Assert.That(inbox.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Inbox_1"))));
                    Assert.That(inbox.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Inbox_1"))));
                    Assert.That(inbox.StatusCode, Is.EqualTo(0));
                    Assert.That(ByteArrayToString(inbox.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Inbox_1")));
                    Assert.That(ByteArrayToString(inbox.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Inbox_1")));
                    Assert.That(ByteArrayToString(inbox.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Inbox_1")));
                    Assert.That(ByteArrayToString(inbox.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Inbox_1")));
                    Assert.That(inbox.Version, Is.EqualTo(1));
                    Assert.That(inbox.Users, Has.Count.EqualTo(1));
                    if (inbox.Users.Count == 1)
                    {
                        Assert.That(inbox.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(inbox.Managers, Has.Count.EqualTo(1));
                    if (inbox.Managers.Count == 1)
                    {
                        Assert.That(inbox.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Getting inbox failed. Try: correct id.\nMessage: {e.Message}");
            }
            Assert.That(didGetInbox, Is.True);
        }

        [Test, Order(2), Description("List inboxes by providing incorrect input data. 5 tries")]
        public void ListInboxes_IncorrectInputData()
        {
            bool didListInboxes_IncorrectContextId = false;
            bool didListInboxes_LimitLessThan0 = false;
            bool didListInboxes_LimitEqual0 = false;
            bool didListInboxes_IncorrectSortOrder = false;
            bool didListInboxes_IncorrectLastId = false;

            // incorrect contextId
            try
            {
                inboxApi.ListInboxes(config.Read("inboxId", "Inbox_1"), SetPagingQuery(0, 1, "desc"));
                didListInboxes_IncorrectContextId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list inboxes. Try: incorrect contextId\nMessage: {e.Message}");
            }
            Assert.That(didListInboxes_IncorrectContextId, Is.False);

            // limit < 0
            try
            {
                inboxApi.ListInboxes(config.Read("contextId", "Context_1"), SetPagingQuery(0, -1, "desc"));
                didListInboxes_LimitLessThan0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list inboxes. Try: limit < 0.\nMessage: {e.Message}");
            }
            Assert.That(didListInboxes_LimitLessThan0, Is.False);

            // limit == 0
            try
            {
                inboxApi.ListInboxes(config.Read("contextId", "Context_1"), SetPagingQuery(0, 0, "desc"));
                didListInboxes_LimitEqual0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list inboxes. Try: limit == 0.\nMessage: {e.Message}");
            }
            Assert.That(didListInboxes_LimitEqual0, Is.False);

            // incorrect sortOrder
            try
            {
                inboxApi.ListInboxes(config.Read("contextId", "Context_1"), SetPagingQuery(0, 0, "BLACH"));
                didListInboxes_IncorrectSortOrder = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list inboxes. Try: incorrect sortOrder.\nMessage: {e.Message}");
            }
            Assert.That(didListInboxes_IncorrectSortOrder, Is.False);

            // incorrect lastId
            try
            {
                inboxApi.ListInboxes(config.Read("contextId", "Context_1"),
                    SetPagingQuery(0, 1, "BLACH", config.Read("contextId", "Context_1")));
                didListInboxes_IncorrectLastId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list inboxes. Try: incorrect lastId.\nMessage: {e.Message}");
            }
            Assert.That(didListInboxes_IncorrectLastId, Is.False);
        }

        [Test, Order(3), Description("List inboxes by providing correct input data. 3 tries")]
        public void ListInboxes_CorrectInputData()
        {
            bool didListMessages_try1 = false;
            bool didListMessages_try2 = false;
            bool didListMessages_try3 = false;

            // {.skip=4, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<Inbox> listInboxes = inboxApi.ListInboxes(config.Read("contextId", "Context_1"), SetPagingQuery(4, 1, "desc"));
                didListMessages_try1 = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list inboxes. Try 1.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages_try1, Is.True);

            // {.skip=0, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<Inbox> listInboxes = inboxApi.ListInboxes(config.Read("contextId", "Context_1"), SetPagingQuery(0, 1, "desc"));
                didListMessages_try2 = true;

                Assert.Multiple(() =>
                {
                    Assert.That(listInboxes.TotalAvailable, Is.EqualTo(3));
                    Assert.That(listInboxes.ReadItems, Has.Count.EqualTo(1));
                });

                if (listInboxes.ReadItems.Count >= 1)
                {
                    Inbox inbox = listInboxes.ReadItems[0];
                    Assert.Multiple(() =>
                    {
                        Assert.That(inbox.InboxId, Is.EqualTo(config.Read("inboxId", "Inbox_3")));
                        Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Inbox_3")));
                        Assert.That(inbox.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Inbox_3"))));
                        Assert.That(inbox.Creator, Is.EqualTo(config.Read("creator", "Inbox_3")));
                        Assert.That(inbox.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Inbox_3"))));
                        Assert.That(inbox.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Inbox_3"))));
                        Assert.That(inbox.StatusCode, Is.EqualTo(0));
                        Assert.That(ByteArrayToString(inbox.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Inbox_3")));
                        Assert.That(ByteArrayToString(inbox.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Inbox_3")));
                        Assert.That(ByteArrayToString(inbox.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Inbox_3")));
                        Assert.That(ByteArrayToString(inbox.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Inbox_3")));
                        Assert.That(inbox.Version, Is.EqualTo(1));
                        Assert.That(inbox.StatusCode, Is.EqualTo(0));
                        Assert.That(inbox.Users, Has.Count.EqualTo(2));
                        if (inbox.Users.Count == 2)
                        {
                            Assert.That(inbox.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(inbox.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                        Assert.That(inbox.Managers, Has.Count.EqualTo(1));
                        if (inbox.Managers.Count == 1)
                        {
                            Assert.That(inbox.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        }
                    });
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list inboxes. Try 2.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages_try2, Is.True);

            // {.skip=1, .limit=3, .sortOrder="asc"}
            try
            {
                PagingList<Inbox> listInboxes = inboxApi.ListInboxes(config.Read("contextId", "Context_1"), SetPagingQuery(1, 3, "asc"));
                didListMessages_try3 = true;

                Assert.Multiple(() =>
                {
                    Assert.That(listInboxes.TotalAvailable, Is.EqualTo(3));
                    Assert.That(listInboxes.ReadItems, Has.Count.EqualTo(2));
                });

                if (listInboxes.ReadItems.Count >= 1)
                {
                    Inbox inbox = listInboxes.ReadItems[0];
                    Assert.Multiple(() =>
                    {
                        Assert.That(inbox.InboxId, Is.EqualTo(config.Read("inboxId", "Inbox_2")));
                        Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Inbox_2")));
                        Assert.That(inbox.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Inbox_2"))));
                        Assert.That(inbox.Creator, Is.EqualTo(config.Read("creator", "Inbox_2")));
                        Assert.That(inbox.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Inbox_2"))));
                        Assert.That(inbox.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Inbox_2"))));
                        Assert.That(inbox.StatusCode, Is.EqualTo(0));
                        Assert.That(ByteArrayToString(inbox.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Inbox_2")));
                        Assert.That(ByteArrayToString(inbox.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Inbox_2")));
                        Assert.That(ByteArrayToString(inbox.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Inbox_2")));
                        Assert.That(ByteArrayToString(inbox.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Inbox_2")));
                        Assert.That(inbox.Version, Is.EqualTo(1));
                        Assert.That(inbox.StatusCode, Is.EqualTo(0));
                        Assert.That(inbox.Users, Has.Count.EqualTo(2));
                        if (inbox.Users.Count == 2)
                        {
                            Assert.That(inbox.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(inbox.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                        Assert.That(inbox.Managers, Has.Count.EqualTo(2));
                        if (inbox.Managers.Count == 2)
                        {
                            Assert.That(inbox.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(inbox.Managers[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                    });
                }
                if (listInboxes.ReadItems.Count >= 2)
                {
                    Inbox inbox = listInboxes.ReadItems[1];
                    Assert.Multiple(() =>
                    {
                        Assert.That(inbox.InboxId, Is.EqualTo(config.Read("inboxId", "Inbox_3")));
                        Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Inbox_3")));
                        Assert.That(inbox.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Inbox_3"))));
                        Assert.That(inbox.Creator, Is.EqualTo(config.Read("creator", "Inbox_3")));
                        Assert.That(inbox.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Inbox_3"))));
                        Assert.That(inbox.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Inbox_3"))));
                        Assert.That(inbox.StatusCode, Is.EqualTo(0));
                        Assert.That(ByteArrayToString(inbox.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Inbox_3")));
                        Assert.That(ByteArrayToString(inbox.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Inbox_3")));
                        Assert.That(ByteArrayToString(inbox.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Inbox_3")));
                        Assert.That(ByteArrayToString(inbox.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Inbox_3")));
                        Assert.That(inbox.Version, Is.EqualTo(1));
                        Assert.That(inbox.StatusCode, Is.EqualTo(0));
                        Assert.That(inbox.Users, Has.Count.EqualTo(2));
                        if (inbox.Users.Count == 2)
                        {
                            Assert.That(inbox.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(inbox.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                        Assert.That(inbox.Managers, Has.Count.EqualTo(1));
                        if (inbox.Managers.Count == 1)
                        {
                            Assert.That(inbox.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        }
                    });
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list inboxes. Try 3.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages_try3, Is.True);
        }

        [Test, Order(4), Description("Create inbox by providing incorrect input data. 4 tries")]
        public void CreateInbox_Incorrect()
        {
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta;

            bool didCreate_IncorrectContextId = false;
            bool didCreate_IncorrectUsers = false;
            bool didCreate_IncorrectManagers = false;
            bool didCreate_NoManagers = false;

            // incorrect contextId
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.CreateInbox(
                    config.Read("inboxId", "Inbox_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null
                );
                didCreate_IncorrectContextId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create inbox failed. Try: incorrect contextId.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_IncorrectContextId, Is.False);

            // incorrect users
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.CreateInbox(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_2_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null
                );
                didCreate_IncorrectUsers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create inbox failed. Try: incorrect users.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_IncorrectUsers, Is.False);

            // incorrect managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.CreateInbox(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_2_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null
                );
                didCreate_IncorrectManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create inbox failed. Try: incorrect managers.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_IncorrectManagers, Is.False);

            // no managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.CreateInbox(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                    },
                    publicMeta,
                    privateMeta,
                    null
                );
                didCreate_NoManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create inbox failed. Try: incorrect no managers.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_NoManagers, Is.False);
        }

        [Test, Order(5), Description("Create inbox by providing correct input data. 2 tries")]
        public void CreateInbox_Correct()
        {
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            string inboxId = string.Empty;

            bool didCreate_DifUsersAndManagers = false;
            bool didCreate_SameUsersAndManagers = false;

            // different users and managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxId = inboxApi.CreateInbox(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_2_id", "Login"),
                            PubKey = config.Read("user_2_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null
                );
                didCreate_DifUsersAndManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create inbox failed. Try: different users and managers.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_DifUsersAndManagers, Is.True);

            try
            {
                Inbox inbox = inboxApi.GetInbox(inboxId);
                Assert.Multiple(() =>
                {
                    Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(inbox.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(inbox.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(inbox.Users, Has.Count.EqualTo(1));
                    if (inbox.Users.Count == 1)
                    {
                        Assert.That(inbox.Users[0], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    Assert.That(inbox.Managers, Has.Count.EqualTo(1));
                    if (inbox.Managers.Count == 1)
                    {
                        Assert.That(inbox.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Get inbox failed.\nMessage: {e.Message}");
            }

            // same users and managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxId = inboxApi.CreateInbox(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null
                );
                didCreate_SameUsersAndManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create inbox failed. Try: same users and managers.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_SameUsersAndManagers, Is.True);

            try
            {
                Inbox inbox = inboxApi.GetInbox(inboxId);
                Assert.Multiple(() =>
                {
                    Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(inbox.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(inbox.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(inbox.Users, Has.Count.EqualTo(1));
                    if (inbox.Users.Count == 1)
                    {
                        Assert.That(inbox.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(inbox.Managers, Has.Count.EqualTo(1));
                    if (inbox.Managers.Count == 1)
                    {
                        Assert.That(inbox.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Get inbox failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(6), Description("Update inbox by providing incorrect input data. 5 tries")]
        public void UpdateInbox_Incorrect()
        {
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            bool didUpdate_IncorrectInboxId = false;
            bool didUpdate_IncorrectUsers = false;
            bool didUpdate_IncorrectManagers = false;
            bool didUpdate_NoManagers = false;
            bool didUpdate_IncorrectVersion = false;

            // incorrect inboxId
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.UpdateInbox(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null,
                    1,
                    false,
                    false
                );
                didUpdate_IncorrectInboxId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update inbox failed. Try: incorrect inboxId.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectInboxId, Is.False);

            // incorrect users
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.UpdateInbox(
                    config.Read("inboxId", "Inbox_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_2_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null,
                    1,
                    false,
                    false
                );
                didUpdate_IncorrectUsers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update inbox failed. Try: incorrect users.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectUsers, Is.False);

            // incorrect managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.UpdateInbox(
                    config.Read("inboxId", "Inbox_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_2_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null,
                    1,
                    false,
                    false
                );
                didUpdate_IncorrectManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update inbox failed. Try: incorrect managers.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectManagers, Is.False);

            // no managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.UpdateInbox(
                    config.Read("inboxId", "Inbox_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                    },
                    publicMeta,
                    privateMeta,
                    null,
                    1,
                    false,
                    false
                );
                didUpdate_NoManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update inbox failed. Try: no managers.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_NoManagers, Is.False);

            // incorrect version force false
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.UpdateInbox(
                    config.Read("inboxId", "Inbox_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                    },
                    publicMeta,
                    privateMeta,
                    null,
                    2,
                    false,
                    false
                );
                didUpdate_IncorrectVersion = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update inbox failed. Try: incorrect version force false.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectVersion, Is.False);
        }

        [Test, Order(7), Description("Update inbox by providing correct input data. 4 tries")]
        public void UpdateInbox_Correct()
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
                inboxApi.UpdateInbox(
                    config.Read("inboxId", "Inbox_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        },
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_2_id", "Login"),
                            PubKey = config.Read("user_2_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null,
                    1,
                    false,
                    false
                );
                didUpdate_NewUsers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_NewUsers, Is.True);

            try
            {
                Inbox inbox = inboxApi.GetInbox(config.Read("inboxId", "Inbox_1"));
                Assert.Multiple(() =>
                {
                    Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(inbox.Version, Is.EqualTo(2));
                    Assert.That(inbox.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(inbox.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(inbox.Users, Has.Count.EqualTo(2));
                    if (inbox.Users.Count == 2)
                    {
                        Assert.That(inbox.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(inbox.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    Assert.That(inbox.Managers, Has.Count.EqualTo(1));
                    if (inbox.Managers.Count == 1)
                    {
                        Assert.That(inbox.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
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
                inboxApi.UpdateInbox(
                    config.Read("inboxId", "Inbox_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        },
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_2_id", "Login"),
                            PubKey = config.Read("user_2_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null,
                    2,
                    false,
                    false
                );
                didUpdate_NewManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_NewManagers, Is.True);

            try
            {
                Inbox inbox = inboxApi.GetInbox(config.Read("inboxId", "Inbox_1"));
                Assert.Multiple(() =>
                {
                    Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(inbox.Version, Is.EqualTo(3));
                    Assert.That(inbox.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(inbox.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(inbox.Users, Has.Count.EqualTo(1));
                    if (inbox.Users.Count == 1)
                    {
                        Assert.That(inbox.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(inbox.Managers, Has.Count.EqualTo(2));
                    if (inbox.Managers.Count == 2)
                    {
                        Assert.That(inbox.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(inbox.Managers[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting inbox failed.\nMessage: {e.Message}");
            }

            // less users
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.UpdateInbox(
                    config.Read("inboxId", "Inbox_2"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        },
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_2_id", "Login"),
                            PubKey = config.Read("user_2_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null,
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
                Inbox inbox = inboxApi.GetInbox(config.Read("inboxId", "Inbox_2"));
                Assert.Multiple(() =>
                {
                    Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(inbox.Version, Is.EqualTo(2));
                    Assert.That(inbox.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(inbox.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(inbox.Users, Has.Count.EqualTo(2));
                    if (inbox.Users.Count == 2)
                    {
                        Assert.That(inbox.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(inbox.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    Assert.That(inbox.Managers, Has.Count.EqualTo(1));
                    if (inbox.Managers.Count == 1)
                    {
                        Assert.That(inbox.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting inbox failed.\nMessage: {e.Message}");
            }

            // less managers
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.UpdateInbox(
                    config.Read("inboxId", "Inbox_2"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            UserId = config.Read("user_1_id", "Login"),
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null,
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
                Inbox inbox = inboxApi.GetInbox(config.Read("inboxId", "Inbox_2"));
                Assert.Multiple(() =>
                {
                    Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(inbox.Version, Is.EqualTo(3));
                    Assert.That(inbox.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(inbox.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(inbox.Users, Has.Count.EqualTo(1));
                    if (inbox.Users.Count == 1)
                    {
                        Assert.That(inbox.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(inbox.Managers, Has.Count.EqualTo(1));
                    if (inbox.Managers.Count == 1)
                    {
                        Assert.That(inbox.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting inbox failed.\nMessage: {e.Message}");
            }
        }
    }
}
