using EndpointCSharpTests.Utils;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Inbox;
using PrivMX.Endpoint.Inbox.Models;
using PrivMX.Endpoint.Store;
using PrivMX.Endpoint.Store.Models;
using PrivMX.Endpoint.Thread;
using File = PrivMX.Endpoint.Store.Models.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Reflection.Metadata;
using System.Security.Cryptography;

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

        [Test, Order(8), Description("Delete inbox by providing incorrect input data. 2 tries")]
        public void DeleteInbox_Incorrect()
        {
            bool didUpdate_IncorrectInboxId = false;
            bool didUpdate_AsUser = false;

            // incorrect inboxId
            try
            {
                inboxApi.DeleteInbox(config.Read("contextId", "Context_1"));
                didUpdate_IncorrectInboxId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete inbox. Try: incorrect inboxId.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectInboxId, Is.False);

            // as user
            CustomDisconnect(ref connection);
            CustomConnect(ref connection, ConnectionType.User2);
            try
            {
                inboxApi.DeleteInbox(config.Read("inboxId", "Inbox_3"));
                didUpdate_AsUser = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete inbox. Try: as user.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_AsUser, Is.False);
        }

        [Test, Order(9), Description("Delete inbox by providing correct input data. 1 try")]
        public void DeleteInbox_Correct()
        {
            bool didDelete_AsManager = false;
            bool didGetInbox_AlreadyDeleted = false;

            try
            {
                inboxApi.DeleteInbox(config.Read("inboxId", "Inbox_3"));
                didDelete_AsManager = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete inbox.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_AsManager, Is.True);

            try
            {
                inboxApi.GetInbox(config.Read("inboxId", "Inbox_3"));
                didGetInbox_AlreadyDeleted = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to get inbox.\nMessage: {e.Message}");
            }
            Assert.That(didGetInbox_AlreadyDeleted, Is.False);
        }

        [Test, Order(10), Description("Get inbox public view by providing incorrect input data. 1 try")]
        public void GetInboxPublicView_Incorrect()
        {
            bool didGet = false;

            // incorrect inboxId
            try
            {
                inboxApi.GetInboxPublicView(config.Read("contextId", "Context_1"));
                didGet = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to get inbox public view.\nMessage: {e.Message}");
            }
            Assert.That(didGet, Is.False);
        }

        [Test, Order(11), Description("Get inbox public view by providing correct input data. 1 try")]
        public void GetInboxPublicView_Correct()
        {
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            string inboxId = string.Empty;

            bool didGet = false;

            // correct inboxId
            try
            {
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
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Create inbox failed.\nMessage: {e.Message}");
            }

            try
            {
                InboxPublicView inboxPublicView = inboxApi.GetInboxPublicView(inboxId);
                didGet = true;

                Assert.Multiple(() =>
                {
                    Assert.That(inboxPublicView.InboxId, Is.EqualTo(inboxId));
                    Assert.That(inboxPublicView.Version, Is.EqualTo(1));
                    Assert.That(inboxPublicView.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(inboxPublicView.PublicMeta, Is.EqualTo(publicMeta));
                });
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to get inbox public view.\nMessage: {e.Message}");
            }
            Assert.That(didGet, Is.True);
        }

        [Test, Order(12), Description("Read entry by providing incorrect input data. 1 try")]
        public void ReadEntry_Incorrect()
        {
            bool didReadEntry = false;

            // incorrect inboxId
            try
            {
                inboxApi.ReadEntry(config.Read("contextId", "Context_1"));
                didReadEntry = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to read entry.\nMessage: {e.Message}");
            }
            Assert.That(didReadEntry, Is.False);
        }

        [Test, Order(13), Description("Read entry by providing correct input data. 1 try")]
        public void ReadEntry_Correct()
        {
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            InboxEntry entry;

            bool didReadEntry = false;

            // after force key generation on inbox
            try
            {
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
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null,
                    1,
                    true,
                    true
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update inbox failed.\nMessage: {e.Message}");
            }

            try
            {
                entry = inboxApi.ReadEntry(config.Read("entryId", "Entry_1"));
                didReadEntry = true;

                Assert.Multiple(() =>
                {
                    Assert.That(entry.EntryId, Is.EqualTo(config.Read("entryId", "Entry_1")));
                    Assert.That(entry.InboxId, Is.EqualTo(config.Read("inboxId", "Entry_1")));
                    Assert.That(ByteArrayToString(entry.Data), Is.EqualTo(config.Read("data_inHex", "Entry_1")));
                    Assert.That(ByteArrayToString(entry.Data), Is.EqualTo(config.Read("uploaded_data_inHex", "Entry_1")));
                    Assert.That(entry.AuthorPubKey, Is.EqualTo(config.Read("authorPubKey", "Entry_1")));
                    Assert.That(entry.CreateDate, Is.EqualTo(StringToInt64(config.Read("createDate", "Entry_1"))));
                    Assert.That(entry.StatusCode, Is.EqualTo(0));
                    Assert.That(entry.Files, Has.Count.EqualTo(2));
                    if(entry.Files.Count >= 1)
                    {
                        File file = entry.Files[0];

                        Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("file_0_info_storeId", "Entry_1")));
                        Assert.That(file.Info.FileId, Is.EqualTo(config.Read("file_0_info_fileId", "Entry_1")));
                        Assert.That(file.Info.CreateDate, Is.EqualTo(StringToInt64(config.Read("file_0_info_createDate", "Entry_1"))));
                        Assert.That(file.Info.Author, Is.EqualTo(config.Read("file_0_info_author", "Entry_1")));
                        Assert.That(file.AuthorPubKey, Is.EqualTo(config.Read("file_0_authorPubKey", "Entry_1")));
                        Assert.That(file.StatusCode, Is.EqualTo(0));
                        Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("file_0_publicMeta_inHex", "Entry_1")));
                        Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("file_0_privateMeta_inHex", "Entry_1")));
                        Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("uploaded_file_0_publicMeta_inHex", "Entry_1")));
                        Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("uploaded_file_0_privateMeta_inHex", "Entry_1")));
                        Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("file_0_size", "Entry_1"))));
                        Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("uploaded_file_0_size", "Entry_1"))));
                    }
                    if (entry.Files.Count >= 2)
                    {
                        File file = entry.Files[1];

                        Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("file_1_info_storeId", "Entry_1")));
                        Assert.That(file.Info.FileId, Is.EqualTo(config.Read("file_1_info_fileId", "Entry_1")));
                        Assert.That(file.Info.CreateDate, Is.EqualTo(StringToInt64(config.Read("file_1_info_createDate", "Entry_1"))));
                        Assert.That(file.Info.Author, Is.EqualTo(config.Read("file_1_info_author", "Entry_1")));
                        Assert.That(file.AuthorPubKey, Is.EqualTo(config.Read("file_1_authorPubKey", "Entry_1")));
                        Assert.That(file.StatusCode, Is.EqualTo(0));
                        Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("file_1_publicMeta_inHex", "Entry_1")));
                        Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("file_1_privateMeta_inHex", "Entry_1")));
                        Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("uploaded_file_1_publicMeta_inHex", "Entry_1")));
                        Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("uploaded_file_1_privateMeta_inHex", "Entry_1")));
                        Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("file_1_size", "Entry_1"))));
                        Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("uploaded_file_1_size", "Entry_1"))));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to read entry.\nMessage: {e.Message}");
            }
            Assert.That(didReadEntry, Is.True);
        }

        [Test, Order(14), Description("List entries by providing incorrect input data. 5 tries")]
        public void ListEntries_IncorrectInputData()
        {
            bool didListEntries_IncorrectInboxtId = false;
            bool diListEntries_LimitLessThan0 = false;
            bool didListEntries_LimitEqual0 = false;
            bool didListEntries_IncorrectSortOrder = false;
            bool didListEntries_IncorrectLastId = false;

            // incorrect inboxId
            try
            {
                inboxApi.ListEntries(config.Read("contextId", "Context_1"), SetPagingQuery(0, 1, "desc"));
                didListEntries_IncorrectInboxtId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list inboxes. Try: incorrect inboxId\nMessage: {e.Message}");
            }
            Assert.That(didListEntries_IncorrectInboxtId, Is.False);

            // limit < 0
            try
            {
                inboxApi.ListEntries(config.Read("inboxId", "Inbox_1"), SetPagingQuery(0, -1, "desc"));
                diListEntries_LimitLessThan0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list inboxes. Try: limit < 0.\nMessage: {e.Message}");
            }
            Assert.That(diListEntries_LimitLessThan0, Is.False);

            // limit == 0
            try
            {
                inboxApi.ListEntries(config.Read("inboxId", "Inbox_1"), SetPagingQuery(0, 0, "desc"));
                didListEntries_LimitEqual0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list inboxes. Try: limit == 0.\nMessage: {e.Message}");
            }
            Assert.That(didListEntries_LimitEqual0, Is.False);

            // incorrect sortOrder
            try
            {
                inboxApi.ListEntries(config.Read("inboxId", "Inbox_1"), SetPagingQuery(0, 0, "BLACH"));
                didListEntries_IncorrectSortOrder = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list inboxes. Try: incorrect sortOrder.\nMessage: {e.Message}");
            }
            Assert.That(didListEntries_IncorrectSortOrder, Is.False);

            // incorrect lastId
            try
            {
                inboxApi.ListEntries(config.Read("inboxId", "Inbox_1"),
                    SetPagingQuery(0, 1, "BLACH", config.Read("contextId", "Context_1")));
                didListEntries_IncorrectLastId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list inboxes. Try: incorrect lastId.\nMessage: {e.Message}");
            }
            Assert.That(didListEntries_IncorrectLastId, Is.False);
        }

        [Test, Order(15), Description("List inboxes by providing correct input data. 3 tries")]
        public void ListEntries_CorrectInputData()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didListMessages_try1 = false;
            bool didListMessages_try2 = false;
            bool didListMessages_try3 = false;

            // {.skip=4, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<InboxEntry> listEntries = inboxApi.ListEntries(config.Read("inboxId", "Inbox_1"), SetPagingQuery(4, 1, "desc"));
                didListMessages_try1 = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list inboxes. Try 1.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages_try1, Is.True);

            // {.skip=1, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<InboxEntry> listEntries = inboxApi.ListEntries(config.Read("inboxId", "Inbox_1"), SetPagingQuery(1, 1, "desc"));
                didListMessages_try2 = true;

                Assert.Multiple(() =>
                {
                    Assert.That(listEntries.TotalAvailable, Is.EqualTo(2));
                    Assert.That(listEntries.ReadItems, Has.Count.EqualTo(1));
                });

                if (listEntries.ReadItems.Count >= 1)
                {
                    InboxEntry entry = listEntries.ReadItems[0];
                    Assert.Multiple(() =>
                    {
                        Assert.That(entry.EntryId, Is.EqualTo(config.Read("entryId", "Entry_1")));
                        Assert.That(entry.InboxId, Is.EqualTo(config.Read("inboxId", "Entry_1")));
                        Assert.That(ByteArrayToString(entry.Data), Is.EqualTo(config.Read("data_inHex", "Entry_1")));
                        Assert.That(ByteArrayToString(entry.Data), Is.EqualTo(config.Read("uploaded_data_inHex", "Entry_1")));
                        Assert.That(entry.AuthorPubKey, Is.EqualTo(config.Read("authorPubKey", "Entry_1")));
                        Assert.That(entry.CreateDate, Is.EqualTo(StringToInt64(config.Read("createDate", "Entry_1"))));
                        Assert.That(entry.StatusCode, Is.EqualTo(0));
                        Assert.That(entry.Files, Has.Count.EqualTo(2));
                        if (entry.Files.Count >= 1)
                        {
                            File file = entry.Files[0];

                            Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("file_0_info_storeId", "Entry_1")));
                            Assert.That(file.Info.FileId, Is.EqualTo(config.Read("file_0_info_fileId", "Entry_1")));
                            Assert.That(file.Info.CreateDate, Is.EqualTo(StringToInt64(config.Read("file_0_info_createDate", "Entry_1"))));
                            Assert.That(file.Info.Author, Is.EqualTo(config.Read("file_0_info_author", "Entry_1")));
                            Assert.That(file.AuthorPubKey, Is.EqualTo(config.Read("file_0_authorPubKey", "Entry_1")));
                            Assert.That(file.StatusCode, Is.EqualTo(0));
                            Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("file_0_publicMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("file_0_privateMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("uploaded_file_0_publicMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("uploaded_file_0_privateMeta_inHex", "Entry_1")));
                            Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("file_0_size", "Entry_1"))));
                            Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("uploaded_file_0_size", "Entry_1"))));
                        }
                        if (entry.Files.Count >= 2)
                        {
                            File file = entry.Files[1];

                            Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("file_1_info_storeId", "Entry_1")));
                            Assert.That(file.Info.FileId, Is.EqualTo(config.Read("file_1_info_fileId", "Entry_1")));
                            Assert.That(file.Info.CreateDate, Is.EqualTo(StringToInt64(config.Read("file_1_info_createDate", "Entry_1"))));
                            Assert.That(file.Info.Author, Is.EqualTo(config.Read("file_1_info_author", "Entry_1")));
                            Assert.That(file.AuthorPubKey, Is.EqualTo(config.Read("file_1_authorPubKey", "Entry_1")));
                            Assert.That(file.StatusCode, Is.EqualTo(0));
                            Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("file_1_publicMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("file_1_privateMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("uploaded_file_1_publicMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("uploaded_file_1_privateMeta_inHex", "Entry_1")));
                            Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("file_1_size", "Entry_1"))));
                            Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("uploaded_file_1_size", "Entry_1"))));
                        }
                    });
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list inboxes. Try 2.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages_try2, Is.True);

            // {.skip=1, .limit=3, .sortOrder="asc"}, after force key generation on inbox
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
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null,
                    1,
                    true,
                    true
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update inbox failed.\nMessage: {e.Message}");
            }

            try
            {
                PagingList<InboxEntry> listEntries = inboxApi.ListEntries(config.Read("inboxId", "Inbox_1"), SetPagingQuery(0, 3, "asc"));
                didListMessages_try3 = true;

                Assert.Multiple(() =>
                {
                    Assert.That(listEntries.TotalAvailable, Is.EqualTo(2));
                    Assert.That(listEntries.ReadItems, Has.Count.EqualTo(2));
                });

                if (listEntries.ReadItems.Count >= 1)
                {
                    InboxEntry entry = listEntries.ReadItems[0];
                    Assert.Multiple(() =>
                    {
                        Assert.That(entry.EntryId, Is.EqualTo(config.Read("entryId", "Entry_1")));
                        Assert.That(entry.InboxId, Is.EqualTo(config.Read("inboxId", "Entry_1")));
                        Assert.That(ByteArrayToString(entry.Data), Is.EqualTo(config.Read("data_inHex", "Entry_1")));
                        Assert.That(ByteArrayToString(entry.Data), Is.EqualTo(config.Read("uploaded_data_inHex", "Entry_1")));
                        Assert.That(entry.AuthorPubKey, Is.EqualTo(config.Read("authorPubKey", "Entry_1")));
                        Assert.That(entry.CreateDate, Is.EqualTo(StringToInt64(config.Read("createDate", "Entry_1"))));
                        Assert.That(entry.StatusCode, Is.EqualTo(0));
                        Assert.That(entry.Files, Has.Count.EqualTo(2));
                        if (entry.Files.Count >= 1)
                        {
                            File file = entry.Files[0];

                            Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("file_0_info_storeId", "Entry_1")));
                            Assert.That(file.Info.FileId, Is.EqualTo(config.Read("file_0_info_fileId", "Entry_1")));
                            Assert.That(file.Info.CreateDate, Is.EqualTo(StringToInt64(config.Read("file_0_info_createDate", "Entry_1"))));
                            Assert.That(file.Info.Author, Is.EqualTo(config.Read("file_0_info_author", "Entry_1")));
                            Assert.That(file.AuthorPubKey, Is.EqualTo(config.Read("file_0_authorPubKey", "Entry_1")));
                            Assert.That(file.StatusCode, Is.EqualTo(0));
                            Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("file_0_publicMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("file_0_privateMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("uploaded_file_0_publicMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("uploaded_file_0_privateMeta_inHex", "Entry_1")));
                            Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("file_0_size", "Entry_1"))));
                            Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("uploaded_file_0_size", "Entry_1"))));
                        }
                        if (entry.Files.Count >= 2)
                        {
                            File file = entry.Files[1];

                            Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("file_1_info_storeId", "Entry_1")));
                            Assert.That(file.Info.FileId, Is.EqualTo(config.Read("file_1_info_fileId", "Entry_1")));
                            Assert.That(file.Info.CreateDate, Is.EqualTo(StringToInt64(config.Read("file_1_info_createDate", "Entry_1"))));
                            Assert.That(file.Info.Author, Is.EqualTo(config.Read("file_1_info_author", "Entry_1")));
                            Assert.That(file.AuthorPubKey, Is.EqualTo(config.Read("file_1_authorPubKey", "Entry_1")));
                            Assert.That(file.StatusCode, Is.EqualTo(0));
                            Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("file_1_publicMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("file_1_privateMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PublicMeta), Is.EqualTo(config.Read("uploaded_file_1_publicMeta_inHex", "Entry_1")));
                            Assert.That(ByteArrayToString(file.PrivateMeta), Is.EqualTo(config.Read("uploaded_file_1_privateMeta_inHex", "Entry_1")));
                            Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("file_1_size", "Entry_1"))));
                            Assert.That(file.Size, Is.EqualTo(StringToInt64(config.Read("uploaded_file_1_size", "Entry_1"))));
                        }
                    });
                }
                if (listEntries.ReadItems.Count >= 2)
                {
                    InboxEntry entry = listEntries.ReadItems[1];
                    Assert.Multiple(() =>
                    {
                        Assert.That(entry.EntryId, Is.EqualTo(config.Read("entryId", "Entry_2")));
                        Assert.That(entry.InboxId, Is.EqualTo(config.Read("inboxId", "Entry_2")));
                        Assert.That(ByteArrayToString(entry.Data), Is.EqualTo(config.Read("data_inHex", "Entry_2")));
                        Assert.That(ByteArrayToString(entry.Data), Is.EqualTo(config.Read("uploaded_data_inHex", "Entry_2")));
                        Assert.That(entry.AuthorPubKey, Is.EqualTo(config.Read("authorPubKey", "Entry_2")));
                        Assert.That(entry.CreateDate, Is.EqualTo(StringToInt64(config.Read("createDate", "Entry_2"))));
                        Assert.That(entry.StatusCode, Is.EqualTo(0));
                        Assert.That(entry.Files, Has.Count.EqualTo(0));
                    });
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list inboxes. Try 3.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages_try3, Is.True);
        }

        [Test, Order(16), Description("Delete entry by providing incorrect input data. 2 tries")]
        public void DeleteEntry_Incorrect()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didDelete_IncorrectEntryId = false;
            bool didDelete_AsUserNotTheirEntry = false;

            // incorrect entryId
            try
            {
                inboxApi.DeleteEntry(config.Read("inboxId", "Inbox_1"));
                didDelete_IncorrectEntryId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete entry. Try: incorrect entryId.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_IncorrectEntryId, Is.False);

            // change privileges
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
                    true,
                    false
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update inbox failed.\nMessage: {e.Message}");
            }

            // as user, not their entry
            CustomDisconnect(ref connection);
            CustomConnect(ref connection, ConnectionType.User2);
            try
            {
                inboxApi.DeleteEntry(config.Read("entryId", "Entry_2"));
                didDelete_AsUserNotTheirEntry = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete entry. Try: as user.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_AsUserNotTheirEntry, Is.False);
        }

        [Test, Order(17), Description("Delete entry by providing correct input data. _ try")]
        public void DeleteEntry_Correct()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didDelete_AsManager = false;

            // change privileges
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
                    2,
                    true,
                    false
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update inbox failed.\nMessage: {e.Message}");
            }

            // as manager no created by me
            try
            {
                inboxApi.DeleteEntry(config.Read("entryId", "Entry_2"));
                didDelete_AsManager = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete entry. Try: as manager.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_AsManager, Is.True);
        }

        [Test, Order(18), Description("Open file - 1 incorrect try")]
        public void OpenFile_Incorrect()
        {
            bool didOpen = false;

            // openFile incorrect fileId
            try
            {
                inboxApi.OpenFile(config.Read("storeId", "Store_1"));
                didOpen = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't open the file.\nMessage: {e.Message}");
            }
            Assert.That(didOpen, Is.False);
        }

        /*Couldn't open the file. code: 4294901760, msg: Invalid request exception*/
        [Test, Order(19), Description("Open file - 1 correct try")]
        public void OpenFile_Correct()
        {
            bool didOpen = false;

            // openFile correct fileId
            try
            {
                inboxApi.OpenFile(config.Read("file_0_info_fileId", "Entry_1"));
                didOpen = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't open the file.\nMessage: {e.Message}");
            }
            Assert.That(didOpen, Is.True);
        }

        [Test, Order(20), Description("Read file - 1 incorrect try")]
        public void ReadFile_Incorrect()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            long handle = 0;

            bool didRead = false;
            bool didRead_IncorrectHandleWrite = false;

            // readFromFile incorrect handle (not exist)
            try
            {
                inboxApi.ReadFromFile(0, 10);
                didRead = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't read from file.\nMessage: {e.Message}");
            }
            Assert.That(didRead, Is.False);

            // createFile correctly
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                handle = inboxApi.CreateFileHandle(
                    publicMeta,
                    privateMeta,
                    10
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't create file.\nMessage: {e.Message}");
            }

            if(handle == 1)
            {
                try
                {
                    inboxApi.ReadFromFile(handle, 10);
                    didRead_IncorrectHandleWrite = true;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Couldn't read from file.\nMessage: {e.Message}");
                }
                Assert.That(didRead_IncorrectHandleWrite, Is.False);
            }
        }

        // open is used - throws Invalid request exception
        [Test, Order(21), Description("Read file - 2 correct tries")]
        public void ReadFile_Correct()
        {
            long handle = 0;
            byte[] data = [];

            bool didRead_LengthHalfTheFileSize = false;

            // open file to get the handle
            try
            {
                handle = inboxApi.OpenFile(config.Read("file_0_info_fileId", "Entry_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't open the file.\nMessage: {e.Message}");
            }

            // readFromFile length == 50% file.size
            try
            {
                inboxApi.ReadFromFile(handle, StringToInt64(config.Read("file_0_size", "Entry_1")) / 2);
                didRead_LengthHalfTheFileSize = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't read from file.\nMessage: {e.Message}");
            }
            Assert.That(didRead_LengthHalfTheFileSize, Is.True);
        }

        // open is used - throws Invalid request exception
        [Test, Order(22), Description("Seek file - 4 incorrect tries")]
        public void SeekFile_Incorrect()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            long handle = 0;

            bool didSeek_IncorrectHandle = false;
            bool didSeek_IncorrectHandleWrite = false;
            bool didSeek_PosLessThan0 = false;
            bool didSeek_PosMoreThanFileSize = false;
            bool didSeek_PosHalfTheFileSize = false;

            // seekInFile incorrect handle (not exist)
            try
            {
                inboxApi.SeekInFile(0, 10);
                didSeek_IncorrectHandle = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't seek in file.\nMessage: {e.Message}");
            }
            Assert.That(didSeek_IncorrectHandle, Is.False);

            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                handle = inboxApi.CreateFileHandle(
                    publicMeta,
                    privateMeta,
                    10
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't create file.\nMessage: {e.Message}");
            }

            if (handle == 1)
            {
                try
                {
                    inboxApi.ReadFromFile(handle, 10);
                    didSeek_IncorrectHandleWrite = true;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Couldn't read from file.\nMessage: {e.Message}");
                }
                Assert.That(didSeek_IncorrectHandleWrite, Is.False);
            }

            // seekInFile pos < 0
            try
            {
                inboxApi.SeekInFile(handle, -1);
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
                inboxApi.SeekInFile(handle, StringToInt64(config.Read("file_0_size", "Entry_1")) + 1);
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
                inboxApi.SeekInFile(handle, StringToInt64(config.Read("file_0_size", "Entry_1")) / 2);
                didSeek_PosHalfTheFileSize = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't seek the file (pos =  1/2 fileSize).\nMessage: {e.Message}");
            }
            Assert.That(didSeek_PosHalfTheFileSize, Is.False);
        }

        // open is used - throws Invalid request exception
        [Test, Order(23), Description("Seek file - 1 correct try")]
        public void SeekFile_Correct()
        {
            long handle = 0;

            bool didSeek_PosEq0 = false;

            // open file to get the handle
            try
            {
                handle = inboxApi.OpenFile(config.Read("file_0_info_fileId", "Entry_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't open the file.\nMessage: {e.Message}");
            }

            // seekInFile pos == 0
            try
            {
                inboxApi.SeekInFile(handle, 0);
                didSeek_PosEq0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't seek in file (pos == 0).\nMessage: {e.Message}");
            }
            Assert.That(didSeek_PosEq0, Is.True);
        }

        [Test, Order(24), Description("Create file - 2 incorrect tries")]
        public void CreateFileHandle_Incorrect()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didCreate_SizeLessThan0 = false;

            // createFileHandle size < 0
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.CreateFileHandle(
                    publicMeta,
                    privateMeta,
                    -1
                );
                didCreate_SizeLessThan0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't create file handle. Try: Incorrect storeId.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_SizeLessThan0, Is.False);
        }

        [Test, Order(25), Description("Create file handle - 2 correct tries")]
        public void CreateFileHandle_Correct()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didCreate = false;

            // createFile correctly
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.CreateFileHandle(
                    publicMeta,
                    privateMeta,
                    10
                );
                didCreate = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't create file.\nMessage: {e.Message}");
            }
            Assert.That(didCreate, Is.True);
        }

        [Test, Order(26), Description("Close file - 1 incorrect try")]
        public void CloseFile_Incorrect()
        {
            bool didClose_IncorrectHandle = false;

            // closeFile incorrect handle
            try
            {
                inboxApi.CloseFile(0);
                didClose_IncorrectHandle = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Couldn't close the file.\nMessage: {e.Message}");
            }
            Assert.That(didClose_IncorrectHandle, Is.False);
        }

        // open is used - throws Invalid request exception
        [Test, Order(27), Description("Close file - 1 correct try")]
        public void CloseFile_Correct()
        {
            long handle = 0;
            bool didClose = false;

            // open file to get the handle
            try
            {
                handle = inboxApi.OpenFile(config.Read("file_0_info_fileId", "Entry_1"));
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

        [Test, Order(28), Description("Test sequence of CreateFileHandle_PrepareEntry_WriteToFile_SendEntry as public user")]
        public void TestSequence_CreateFileHandle_PrepareEntry_WriteToFile_SendEntry_AsPublic()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] data = [];
            long fileHandle_1 = 0;
            long fileHandle_2 = 0;

            CustomDisconnect(ref connection);
            CustomConnect(ref connection, ConnectionType.Public);

            // createFileHandle size = 0
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                fileHandle_1 = inboxApi.CreateFileHandle(
                    publicMeta,
                    privateMeta,
                    0
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't create file handle. Try: Incorrect storeId.\nMessage: {e.Message}");
            }

            // createFileHandle size > 0
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                fileHandle_2 = inboxApi.CreateFileHandle(
                    publicMeta,
                    privateMeta,
                    1024 * 2
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't create file handle. Try: Incorrect storeId.\nMessage: {e.Message}");
            }

            Assert.Multiple(() =>
            {
                Assert.That(fileHandle_1, Is.EqualTo(1));
                Assert.That(fileHandle_2, Is.EqualTo(2));
            });

            if (fileHandle_1 == 1 && fileHandle_2 == 2)
            {
                long inboxHandle = 0;
                string totalDataSent = "";

                for (int i = 0; i < 2; i++)
                {
                    data = new byte[1024];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(data);
                    };
                }

                // prepareEntry with 1, 2
                try
                {
                    privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                    inboxHandle = inboxApi.PrepareEntry(
                        config.Read("inboxId", "Inbox_2"),
                        data,
                        new List<long> { fileHandle_1, fileHandle_2 },
                        config.Read("user_1_privKey", "Login")
                    );
                }
                catch (EndpointNativeException e)
                {
                    Assert.Fail($"Couldn't create file handle. Try: Incorrect storeId.\nMessage: {e.Message}");
                }
                Assert.That(inboxHandle, Is.EqualTo(3));

                if(inboxHandle == 3)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        byte[] randomData = new byte[1024];
                        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                        {
                            rng.GetBytes(randomData);
                        };

                        inboxApi.WriteToFile(inboxHandle, fileHandle_2 ,randomData);
                        totalDataSent += ByteArrayToString(randomData);
                    }

                    try
                    {
                        inboxApi.SendEntry(inboxHandle);
                    }
                    catch (EndpointNativeException e)
                    {
                        Assert.Fail($"Couldn't send entry.\nMessage: {e.Message}");
                    }

                    CustomDisconnect(ref connection);
                    CustomConnect(ref connection, ConnectionType.User1);
                    try
                    {
                        PagingList<InboxEntry> listEntries = inboxApi.ListEntries(
                            config.Read("inboxId", "Inbox_2"), 
                            SetPagingQuery(0, 1, "asc")
                        );

                        Assert.Multiple(() =>
                        {
                            Assert.That(listEntries.TotalAvailable, Is.EqualTo(1));
                            Assert.That(listEntries.ReadItems, Has.Count.EqualTo(1));

                            if (listEntries.ReadItems.Count >= 1)
                            {
                                InboxEntry entry = listEntries.ReadItems[0];
                                Assert.That(entry.InboxId, Is.EqualTo(config.Read("inboxId", "Entry_1")));
                                Assert.That(entry.Data, Is.EqualTo(data));
                                Assert.That(entry.Files, Has.Count.EqualTo(2));
                                if (entry.Files.Count >= 1)
                                {
                                    File file = entry.Files[0];

                                    Assert.That(file.StatusCode, Is.EqualTo(0));
                                    Assert.That(file.PublicMeta, Is.EqualTo(publicMeta));
                                    Assert.That(file.PrivateMeta, Is.EqualTo(privateMeta));
                                    Assert.That(file.Size, Is.EqualTo(0));
                                }
                                if (entry.Files.Count >= 2)
                                {
                                    File file = entry.Files[1];

                                    Assert.That(file.StatusCode, Is.EqualTo(0));
                                    Assert.That(file.PublicMeta, Is.EqualTo(publicMeta));
                                    Assert.That(file.PrivateMeta, Is.EqualTo(privateMeta));
                                    Assert.That(file.Size, Is.EqualTo(1024*2));

                                    long readHandle = 0;
                                    string totalDataRead = "";
                                    try
                                    {
                                        readHandle = inboxApi.OpenFile(file.Info.FileId);
                                    }
                                    catch (EndpointNativeException e)
                                    {
                                        Assert.Fail($"Couldn't open file.\nMessage: {e.Message}");
                                    }

                                    for (int i = 0; i < 2; i++)
                                    {
                                        totalDataRead += inboxApi.ReadFromFile(readHandle, 1024);
                                    }
                                    Assert.That(totalDataSent, Is.EqualTo(totalDataRead));
                                }
                            }
                        });
                    }
                    catch (EndpointNativeException e)
                    {
                        Assert.Fail($"Couldn't send entry.\nMessage: {e.Message}");
                    }
                }
            }
        }

        [Test, Order(29), Description("Test sequence of CreateFileHandle_PrepareEntry_WriteToFile_SendEntry as user")]
        public void TestSequence_CreateFileHandle_PrepareEntry_WriteToFile_SendEntry_AsUser()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] data = [];
            long fileHandle_1 = 0;
            long fileHandle_2 = 0;

            // createFileHandle size = 0
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                fileHandle_1 = inboxApi.CreateFileHandle(
                    publicMeta,
                    privateMeta,
                    0
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't create file handle. Try: Incorrect storeId.\nMessage: {e.Message}");
            }

            // createFileHandle size > 0
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                fileHandle_2 = inboxApi.CreateFileHandle(
                    publicMeta,
                    privateMeta,
                    1024 * 2
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Couldn't create file handle. Try: Incorrect storeId.\nMessage: {e.Message}");
            }

            Assert.Multiple(() =>
            {
                Assert.That(fileHandle_1, Is.EqualTo(1));
                Assert.That(fileHandle_2, Is.EqualTo(2));
            });

            if (fileHandle_1 == 1 && fileHandle_2 == 2)
            {
                long inboxHandle = 0;
                string totalDataSent = "";

                for (int i = 0; i < 2; i++)
                {
                    data = new byte[1024];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(data);
                    };
                }

                // prepareEntry with 1, 2
                try
                {
                    privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                    inboxHandle = inboxApi.PrepareEntry(
                        config.Read("inboxId", "Inbox_2"),
                        data,
                        new List<long> { fileHandle_1, fileHandle_2 },
                        config.Read("user_1_privKey", "Login")
                    );
                }
                catch (EndpointNativeException e)
                {
                    Assert.Fail($"Couldn't create file handle. Try: Incorrect storeId.\nMessage: {e.Message}");
                }
                Assert.That(inboxHandle, Is.EqualTo(3));

                if (inboxHandle == 3)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        byte[] randomData = new byte[1024];
                        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                        {
                            rng.GetBytes(randomData);
                        };

                        inboxApi.WriteToFile(inboxHandle, fileHandle_2, randomData);
                        totalDataSent += ByteArrayToString(randomData);
                    }

                    try
                    {
                        inboxApi.SendEntry(inboxHandle);
                    }
                    catch (EndpointNativeException e)
                    {
                        Assert.Fail($"Couldn't send entry.\nMessage: {e.Message}");
                    }

                    CustomDisconnect(ref connection);
                    CustomConnect(ref connection, ConnectionType.User1);
                    try
                    {
                        PagingList<InboxEntry> listEntries = inboxApi.ListEntries(
                            config.Read("inboxId", "Inbox_2"),
                            SetPagingQuery(0, 1, "asc")
                        );

                        Assert.Multiple(() =>
                        {
                            Assert.That(listEntries.TotalAvailable, Is.EqualTo(1));
                            Assert.That(listEntries.ReadItems, Has.Count.EqualTo(1));

                            if (listEntries.ReadItems.Count >= 1)
                            {
                                InboxEntry entry = listEntries.ReadItems[0];
                                Assert.That(entry.InboxId, Is.EqualTo(config.Read("inboxId", "Entry_1")));
                                Assert.That(entry.Data, Is.EqualTo(data));
                                Assert.That(entry.Files, Has.Count.EqualTo(2));
                                if (entry.Files.Count >= 1)
                                {
                                    File file = entry.Files[0];

                                    Assert.That(file.StatusCode, Is.EqualTo(0));
                                    Assert.That(file.PublicMeta, Is.EqualTo(publicMeta));
                                    Assert.That(file.PrivateMeta, Is.EqualTo(privateMeta));
                                    Assert.That(file.Size, Is.EqualTo(0));
                                }
                                if (entry.Files.Count >= 2)
                                {
                                    File file = entry.Files[1];

                                    Assert.That(file.StatusCode, Is.EqualTo(0));
                                    Assert.That(file.PublicMeta, Is.EqualTo(publicMeta));
                                    Assert.That(file.PrivateMeta, Is.EqualTo(privateMeta));
                                    Assert.That(file.Size, Is.EqualTo(1024 * 2));

                                    long readHandle = 0;
                                    string totalDataRead = "";
                                    try
                                    {
                                        readHandle = inboxApi.OpenFile(file.Info.FileId);
                                    }
                                    catch (EndpointNativeException e)
                                    {
                                        Assert.Fail($"Couldn't open file.\nMessage: {e.Message}");
                                    }

                                    for (int i = 0; i < 2; i++)
                                    {
                                        totalDataRead += inboxApi.ReadFromFile(readHandle, 1024);
                                    }
                                    Assert.That(totalDataSent, Is.EqualTo(totalDataRead));
                                }
                            }
                        });
                    }
                    catch (EndpointNativeException e)
                    {
                        Assert.Fail($"Couldn't send entry.\nMessage: {e.Message}");
                    }
                }
            }
        }

        [Test, Order(30), Description("Try a series of actions on entries and inboxes while not being authorized to do so.")]
        public void AccessDenied_NotInUsersOrManagers()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didGetInbox = false;
            bool didUpdateInbox = false;
            bool didDeleteInbox = false;
            bool didReadEntry = false;
            bool didListEntries = false;

            // connect as user2
            CustomDisconnect(ref connection);
            CustomConnect(ref connection, ConnectionType.User2);

            // getInbox
            try
            {
                inboxApi.GetInbox(config.Read("inboxId", "Inbox_1"));
                didGetInbox = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Getting inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didGetInbox, Is.False);

            // updateInbox
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
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null,
                    1,
                    false,
                    false
                );
                didUpdateInbox = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdateInbox, Is.False);

            // deleteInbox
            try
            {
                inboxApi.DeleteInbox(config.Read("inboxId", "Inbox_1"));
                didDeleteInbox= true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Deleting inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didDeleteInbox, Is.False);

            // readEntry
            try
            {
                inboxApi.ReadEntry(config.Read("entryId", "Entry_1"));
                didReadEntry = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Reading entry failed.\nMessage: {e.Message}");
            }
            Assert.That(didReadEntry, Is.False);

            // listEntries
            try
            {
                inboxApi.ListEntries(config.Read("inboxId", "Inbox_1"), SetPagingQuery(0, 1, "asc"));
                didListEntries = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Listing entries failed.\nMessage: {e.Message}");
            }
            Assert.That(didListEntries, Is.False);
        }

        [Test, Order(30), Description("Try a series of actions on entries and inboxes while not being authorized to do so.")]
        public void AccessDenied_Public()
        {
            byte[] privateMeta = [];
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());

            bool didGetInbox = false;
            bool didListInboxes = false;
            bool didUpdateInbox = false;
            bool didDeleteInbox = false;
            bool didReadEntry = false;
            bool didListEntries = false;

            // connect as user2
            CustomDisconnect(ref connection);
            CustomConnect(ref connection, ConnectionType.Public);

            // getInbox
            try
            {
                inboxApi.GetInbox(config.Read("inboxId", "Inbox_1"));
                didGetInbox = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Getting inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didGetInbox, Is.False);

            // listInboxes
            try
            {
                inboxApi.ListInboxes(config.Read("contextId", "Context_1"), SetPagingQuery(0, 1, "desc"));
                didListInboxes = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Listing inboxes failed.\nMessage: {e.Message}");
            }
            Assert.That(didListInboxes, Is.False);

            // createInbox
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
                            PubKey = config.Read("user_1_pubKey", "Login")
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null
                );
                didUpdateInbox = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdateInbox, Is.False);

            // updateInbox
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
                        }
                    },
                    publicMeta,
                    privateMeta,
                    null,
                    1,
                    false,
                    false
                );
                didUpdateInbox = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdateInbox, Is.False);

            // deleteInbox
            try
            {
                inboxApi.DeleteInbox(config.Read("inboxId", "Inbox_1"));
                didDeleteInbox = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Deleting inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didDeleteInbox, Is.False);

            // readEntry
            try
            {
                inboxApi.ReadEntry(config.Read("entryId", "Entry_1"));
                didReadEntry = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Reading entry failed.\nMessage: {e.Message}");
            }
            Assert.That(didReadEntry, Is.False);

            // listEntries
            try
            {
                inboxApi.ListEntries(config.Read("inboxId", "Inbox_1"), SetPagingQuery(0, 1, "asc"));
                didListEntries = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Listing entries failed.\nMessage: {e.Message}");
            }
            Assert.That(didListEntries, Is.False);
        }

        // UpdaterCanBeRemovedFromManagers err
        [Test, Order(31), Description("Create inbox with policy.")]
        public void CreateInboxWithPolicy()
        {
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            string inboxId = string.Empty;
            Inbox inbox = null;

            bool didCreate = false;
            bool didGetInbox = false;
            bool didGetInbox_User2 = false;

            ContainerPolicy policy = new ContainerPolicy
            {
                Item = new ItemPolicy
                {
                    Get = "owner",
                    ListMy = "owner",
                    ListAll = "owner",
                    Create = "owner",
                    Update = "owner",
                    Delete_ = "owner"
                },
                Get = "owner",
                Update = "owner",
                Delete_ = "owner",
                UpdatePolicy = "owner",
                //UpdaterCanBeRemovedFromManagers = "no",
                OwnerCanBeRemovedFromManagers = "no"
            };

            // create inbox with policy
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
                    policy
                );
                didCreate = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didCreate, Is.True);

            // get inbox and check it
            try
            {
                inbox = inboxApi.GetInbox(inboxId);
                didGetInbox = true;

                Assert.Multiple(() =>
                {
                    Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Store_1")));
                    Assert.That(inbox.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(inbox.PrivateMeta, Is.EqualTo(privateMeta));
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
                    //asserts for policy
                    Assert.That(inbox.Policy.Get, Is.EqualTo(policy.Get));
                    Assert.That(inbox.Policy.Update, Is.EqualTo(policy.Update));
                    Assert.That(inbox.Policy.Delete_, Is.EqualTo(policy.Delete_));
                    Assert.That(inbox.Policy.UpdatePolicy, Is.EqualTo(policy.UpdatePolicy));
                    Assert.That(inbox.Policy.UpdaterCanBeRemovedFromManagers, Is.EqualTo(policy.UpdaterCanBeRemovedFromManagers));
                    Assert.That(inbox.Policy.OwnerCanBeRemovedFromManagers, Is.EqualTo(policy.OwnerCanBeRemovedFromManagers));
                });
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Get inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didGetInbox, Is.True);

            // get inbox as user_2 (wrong)
            CustomDisconnect(ref connection);
            CustomConnect(ref connection, ConnectionType.User2);

            try
            {
                inbox = inboxApi.GetInbox(inboxId);
                didGetInbox = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Get inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didGetInbox_User2, Is.False);
        }

        // UpdaterCanBeRemovedFromManagers err
        [Test, Order(32), Description("Update inbox policy.")]
        public void UpdateInboxPolicy()
        {
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            string inboxId = string.Empty;
            Inbox inbox = null;

            bool didCreate = false;
            bool didGetInbox = false;
            bool didGetInbox_User2 = false;

            ContainerPolicy policy = new ContainerPolicy
            {
                Item = new ItemPolicy
                {
                    Get = "owner",
                    ListMy = "owner",
                    ListAll = "owner",
                    Create = "owner",
                    Update = "owner",
                    Delete_ = "owner"
                },
                Get = "owner",
                Update = "owner",
                Delete_ = "owner",
                UpdatePolicy = "owner",
                //UpdaterCanBeRemovedFromManagers = "no",
                OwnerCanBeRemovedFromManagers = "no"
            };

            // update inbox with policy
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
                inboxApi.UpdateInbox(
                    inboxId,
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
                    1,
                    true,
                    true,
                    policy
                );
                didCreate = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Create inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didCreate, Is.True);

            // get inbox and check it
            try
            {
                inbox = inboxApi.GetInbox(inboxId);
                didGetInbox = true;

                Assert.Multiple(() =>
                {
                    Assert.That(inbox.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(inbox.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(inbox.PrivateMeta, Is.EqualTo(privateMeta));
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
                    //asserts for policy
                    Assert.That(inbox.Policy.Get, Is.EqualTo(policy.Get));
                    Assert.That(inbox.Policy.Update, Is.EqualTo(policy.Update));
                    Assert.That(inbox.Policy.Delete_, Is.EqualTo(policy.Delete_));
                    Assert.That(inbox.Policy.UpdatePolicy, Is.EqualTo(policy.UpdatePolicy));
                    Assert.That(inbox.Policy.UpdaterCanBeRemovedFromManagers, Is.EqualTo(policy.UpdaterCanBeRemovedFromManagers));
                    Assert.That(inbox.Policy.OwnerCanBeRemovedFromManagers, Is.EqualTo(policy.OwnerCanBeRemovedFromManagers));
                });
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Get inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didGetInbox, Is.True);

            // get inbox as user_2 (wrong)
            CustomDisconnect(ref connection);
            CustomConnect(ref connection, ConnectionType.User2);

            try
            {
                inbox = inboxApi.GetInbox(inboxId);
                didGetInbox = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Get inbox failed.\nMessage: {e.Message}");
            }
            Assert.That(didGetInbox_User2, Is.False);
        }
    }
}
