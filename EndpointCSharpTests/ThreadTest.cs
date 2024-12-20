using EndpointCSharpTests.Utils;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Thread;
using Thread = PrivMX.Endpoint.Thread.Models.Thread;
using Message = PrivMX.Endpoint.Thread.Models.Message;
using System.Security.Cryptography;

// Run tests in queue of how the functions are created! Not doing so may change the results (ex: listing might give an exception,
// because the thread count changed in another function (like delete thread)).

namespace EndpointCSharpTests
{
    internal class ThreadTest : BaseTest
    {
        private Connection connection = null;
        private ThreadApi threadApi = null;

        [SetUp]
        public virtual void Setup()
        {
            ConnectAs(ref connection, ConnectionType.User1);
            threadApi = ThreadApi.Create(connection);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Disconnect(ref connection);
        }

        [Test, Description("Gets thread, first by using the incorrect threadId, then correct threadId. Checks values in correct one.")]
        public void GetThread()
        {
            Thread thread = new Thread();
            bool didGetThread_incorrectThreadId = false;
            bool didGetThread_correctThreadId = false;

            // incorrect threadId
            try
            {
                thread = threadApi.GetThread(config.Read("contextId", "Context_1"));
                didGetThread_incorrectThreadId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Getting thread with incorrect id.\nMessage: {e.Message}");
            }
            Assert.That(didGetThread_incorrectThreadId, Is.False);

            // correct threadId
            try
            {
                thread = threadApi.GetThread(config.Read("threadId", "Thread_1"));
                didGetThread_correctThreadId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Getting thread with correct id.\nMessage: {e.Message}");
            }
            Assert.That(didGetThread_correctThreadId, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Thread_1")));
                Assert.That(thread.ThreadId, Is.EqualTo(config.Read("threadId", "Thread_1")));
                Assert.That(thread.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Thread_1"))));
                Assert.That(thread.Creator, Is.EqualTo(config.Read("creator", "Thread_1")));
                Assert.That(thread.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Thread_1"))));
                Assert.That(thread.LastModifier, Is.EqualTo(config.Read("lastModifier", "Thread_1")));
                Assert.That(thread.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Thread_1"))));
                Assert.That(thread.LastMsgDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastMsgDate", "Thread_1"))));
                Assert.That(thread.MessagesCount, Is.EqualTo((long)Convert.ToDouble(config.Read("messagesCount", "Thread_1"))));
                Assert.That(thread.StatusCode, Is.EqualTo(0));
                Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Thread_1")));
                Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Thread_1")));
                Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Thread_1")));
                Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Thread_1")));

                Assert.That(thread.Version, Is.EqualTo(1));
                Assert.That(thread.Creator, Is.EqualTo(config.Read("user_1_id", "Login")));
                Assert.That(thread.LastModifier, Is.EqualTo(config.Read("user_1_id", "Login")));
                Assert.That(thread.Users, Has.Count.EqualTo(1));
                if (thread.Users.Count == 1)
                {
                    Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                }
                Assert.That(thread.Managers, Has.Count.EqualTo(1));
                if (thread.Managers.Count == 1)
                {
                    Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                }
            });
        }

        [Test, Description("List threads for incorrect input data - 5 tries. Expected throws: incorrect contextId, limit < 0, limit == 0, incorrect sortOrder, incorrect lastId")]
        public void ListThreads_IncorrectInput()
        {
            bool didListThreads_IncorrectContextId = false;
            bool didListThreads_LimitLessThan0 = false;
            bool didListThreads_LimitEqual0 = false;
            bool didListThreads_IncorrectSortOrder = false;
            bool didListThreads_IncorrectLastId = false;

            // incorrect contextId
            try
            {
                threadApi.ListThreads(config.Read("threadId", "Thread_1"), SetPagingQuery(0, 1, "desc"));
                didListThreads_IncorrectContextId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Listing threads with incorrect contextId.\nMessage: {e.Message}");
            }
            Assert.That(didListThreads_IncorrectContextId, Is.False);

            // limit < 0
            try
            {
                threadApi.ListThreads(config.Read("contextId", "Context_1"), SetPagingQuery(0, -1, "desc"));
                didListThreads_LimitLessThan0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Listing threads with limit < 0.\nMessage: {e.Message}");
            }
            Assert.That(didListThreads_LimitLessThan0, Is.False);

            // limit == 0
            try
            {
                threadApi.ListThreads(config.Read("contextId", "Context_1"), SetPagingQuery(0, 0, "desc"));
                didListThreads_LimitEqual0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Listing threads with limit == 0.\nMessage: {e.Message}");
            }
            Assert.That(didListThreads_LimitEqual0, Is.False);

            // incorrect sortOrder
            try
            {
                threadApi.ListThreads(config.Read("contextId", "Context_1"), SetPagingQuery(0, 1, "BLACH"));
                didListThreads_IncorrectSortOrder = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Listing threads with incorrect sortOrder.\nMessage: {e.Message}");
            }
            Assert.That(didListThreads_IncorrectSortOrder, Is.False);

            // incorrect lastId
            try
            {
                threadApi.ListThreads(config.Read("contextId", "Context_1"),
                    SetPagingQuery(0, 1, "desc", config.Read("contextId", "Context_1")));
                didListThreads_IncorrectLastId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Listing threads with incorrect sortOrder.\nMessage: {e.Message}");
            }
            Assert.That(didListThreads_IncorrectLastId, Is.False);

        }

        [Test, Description("List threads for correct input data - 3 different tries")]
        public void ListThreadsCorrectInput()
        {
            // {.skip=4, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<Thread> threadList = threadApi.ListThreads(config.Read("contextId", "Context_1"),
                    SetPagingQuery(4, 1, "desc"));
                Assert.Multiple(() =>
                {
                    Assert.That(threadList.TotalAvailable, Is.EqualTo(3));
                    Assert.That(threadList.ReadItems, Has.Count.EqualTo(0));
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Listing threads (try 1) failed.\nMessage: {e.Message}");
            }

            // {.skip=0, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<Thread> threadList = threadApi.ListThreads(config.Read("contextId", "Context_1"),
                    SetPagingQuery(0, 1, "desc"));
                Assert.Multiple(() =>
                {
                    Assert.That(threadList.TotalAvailable, Is.EqualTo(3));
                    Assert.That(threadList.ReadItems, Has.Count.EqualTo(1));
                    if (threadList.ReadItems.Count >= 1)
                    {
                        Thread thread = threadList.ReadItems[0];

                        Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Thread_3")));
                        Assert.That(thread.ThreadId, Is.EqualTo(config.Read("threadId", "Thread_3")));
                        Assert.That(thread.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Thread_3"))));
                        Assert.That(thread.Creator, Is.EqualTo(config.Read("creator", "Thread_3")));
                        Assert.That(thread.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Thread_3"))));
                        Assert.That(thread.LastModifier, Is.EqualTo(config.Read("lastModifier", "Thread_3")));
                        Assert.That(thread.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Thread_3"))));
                        Assert.That(thread.LastMsgDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastMsgDate", "Thread_3"))));
                        Assert.That(thread.MessagesCount, Is.EqualTo((long)Convert.ToDouble(config.Read("messagesCount", "Thread_3"))));
                        Assert.That(thread.StatusCode, Is.EqualTo(0));
                        Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Thread_3")));
                        Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Thread_3")));
                        Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Thread_3")));
                        Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Thread_3")));

                        Assert.That(thread.Version, Is.EqualTo(1));
                        Assert.That(thread.Creator, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.LastModifier, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.Users, Has.Count.EqualTo(2));
                        if (thread.Users.Count == 2)
                        {
                            Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(thread.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                        Assert.That(thread.Managers, Has.Count.EqualTo(1));
                        if (thread.Managers.Count == 1)
                        {
                            Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        }
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Listing threads (try 2) failed.\nMessage: {e.Message}");
            }

            // {.skip=1, .limit=3, .sortOrder="asc"}
            try
            {
                PagingList<Thread> threadList = threadApi.ListThreads(config.Read("contextId", "Context_1"),
                    SetPagingQuery(1, 3, "asc"));
                Assert.Multiple(() =>
                {
                    Assert.That(threadList.TotalAvailable, Is.EqualTo(3));
                    Assert.That(threadList.ReadItems, Has.Count.EqualTo(2));
                    if (threadList.ReadItems.Count >= 1)
                    {
                        Thread thread = threadList.ReadItems[0];

                        Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Thread_2")));
                        Assert.That(thread.ThreadId, Is.EqualTo(config.Read("threadId", "Thread_2")));
                        Assert.That(thread.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Thread_2"))));
                        Assert.That(thread.Creator, Is.EqualTo(config.Read("creator", "Thread_2")));
                        Assert.That(thread.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Thread_2"))));
                        Assert.That(thread.LastModifier, Is.EqualTo(config.Read("lastModifier", "Thread_2")));
                        Assert.That(thread.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Thread_2"))));
                        Assert.That(thread.LastMsgDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastMsgDate", "Thread_2"))));
                        Assert.That(thread.MessagesCount, Is.EqualTo((long)Convert.ToDouble(config.Read("messagesCount", "Thread_2"))));
                        Assert.That(thread.StatusCode, Is.EqualTo(0));
                        Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Thread_2")));
                        Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Thread_2")));
                        Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Thread_2")));
                        Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Thread_2")));

                        Assert.That(thread.Version, Is.EqualTo(1));
                        Assert.That(thread.Creator, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.LastModifier, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.Users, Has.Count.EqualTo(2));
                        if (thread.Users.Count == 2)
                        {
                            Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(thread.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                        Assert.That(thread.Managers, Has.Count.EqualTo(2));
                        if (thread.Managers.Count == 2)
                        {
                            Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(thread.Managers[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                    }
                    if (threadList.ReadItems.Count >= 2)
                    {
                        Thread thread = threadList.ReadItems[1];

                        Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Thread_3")));
                        Assert.That(thread.ThreadId, Is.EqualTo(config.Read("threadId", "Thread_3")));
                        Assert.That(thread.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("createDate", "Thread_3"))));
                        Assert.That(thread.Creator, Is.EqualTo(config.Read("creator", "Thread_3")));
                        Assert.That(thread.LastModificationDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastModificationDate", "Thread_3"))));
                        Assert.That(thread.LastModifier, Is.EqualTo(config.Read("lastModifier", "Thread_3")));
                        Assert.That(thread.Version, Is.EqualTo((long)Convert.ToDouble(config.Read("version", "Thread_3"))));
                        Assert.That(thread.LastMsgDate, Is.EqualTo((long)Convert.ToDouble(config.Read("lastMsgDate", "Thread_3"))));
                        Assert.That(thread.MessagesCount, Is.EqualTo((long)Convert.ToDouble(config.Read("messagesCount", "Thread_3"))));
                        Assert.That(thread.StatusCode, Is.EqualTo(0));
                        Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Thread_3")));
                        Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Thread_3")));
                        Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Thread_3")));
                        Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Thread_3")));

                        Assert.That(thread.Version, Is.EqualTo(1));
                        Assert.That(thread.Creator, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.LastModifier, Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.Users, Has.Count.EqualTo(2));
                        if (thread.Users.Count == 2)
                        {
                            Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                            Assert.That(thread.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                        }
                        Assert.That(thread.Managers, Has.Count.EqualTo(1));
                        if (thread.Managers.Count == 1)
                        {
                            Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        }
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Listing threads (try 3) failed.\nMessage: {e.Message}");
            }
        }

        [Test, Description("Create thread 5 ways - 4 incorrect and 2 correct. Incorrect options: incorrect contextId, incorrect users, incorrect managers, no managers. Correct options: different users and managers, same users and managers")]
        public void CreateThread()
        {
            bool didCreate_IncorrectContextId = false;
            bool didCreate_IncorrectUsers = false;
            bool didCreate_IncorrectManagers = false;
            bool didCreate_NoManagers = false;
            bool didCreate_DifUsersAndManagers = false;
            bool didCreate_SameUsersAndManagers = false;
            string threadId = string.Empty;

            // incorrect contextId
            try
            {
                threadApi.CreateThread(
                    config.Read("threadId", "Thread_1"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()))
                );
                didCreate_IncorrectContextId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"CreateThread failed (incorrect contextId try)\nMessage: {e.Message}");
            }
            Assert.That(didCreate_IncorrectContextId, Is.False);

            // incorrect users
            try
            {
                threadApi.CreateThread(
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()))
                );
                didCreate_IncorrectUsers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"CreateThread failed (incorrect users try)\nMessage: {e.Message}");
            }
            Assert.That(didCreate_IncorrectUsers, Is.False);

            // incorrect managers
            try
            {
                threadApi.CreateThread(
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()))
                );
                didCreate_IncorrectManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"CreateThread failed (incorrect managers try)\nMessage: {e.Message}");
            }
            Assert.That(didCreate_IncorrectManagers, Is.False);

            // no managers
            try
            {
                threadApi.CreateThread(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>(),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()))
                );
                didCreate_NoManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"CreateThread failed (no managers try)\nMessage: {e.Message}");
            }
            Assert.That(didCreate_NoManagers, Is.False);

            // different users and managers - correct  
            try
            {
                threadId = threadApi.CreateThread(
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()))
                );
                didCreate_DifUsersAndManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CreateThread failed (different users and managers try).\nMessage: {e.Message}");
            }
            Assert.That(didCreate_DifUsersAndManagers, Is.True);

            try
            {
                Thread thread = threadApi.GetThread(threadId);
                Assert.Multiple(() =>
                {
                    Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo("public"));
                    Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo("private"));
                    Assert.That(thread.Users, Has.Count.EqualTo(1));
                    if (thread.Users.Count == 1)
                    {
                        Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    Assert.That(thread.Managers, Has.Count.EqualTo(1));
                    if (thread.Managers.Count == 1)
                    {
                        Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting thread failed: Message: {e.Message}");
            }

            // same users and managers - correct
            try
            {
                threadId = threadApi.CreateThread(
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()))
                );
                didCreate_SameUsersAndManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CreateThread failed (same users and managers try)\nMessage: {e.Message}");
            }
            Assert.That(didCreate_SameUsersAndManagers, Is.True);

            try
            {
                Thread thread = threadApi.GetThread(threadId);
                Assert.Multiple(() =>
                {
                    Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo("public"));
                    Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo("private"));
                    Assert.That(thread.Users, Has.Count.EqualTo(1));
                    if (thread.Users.Count == 1)
                    {
                        Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(thread.Managers, Has.Count.EqualTo(1));
                    if (thread.Managers.Count == 1)
                    {
                        Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting thread failed: Message: {e.Message}");
            }
        }

        [Test, Description("Update thread with incorrect data - 5 tries. Expected throws: incorrect threadId, incorrect users, incorrect managers, no managers, ??incorrect version force true??")]
        public void UpdateThread_IncorrectInput()
        {
            bool didUpdate_IncorrectThreadId = false;
            bool didUpdate_IncorrectUsers = false;
            bool didUpdate_IncorrectManagers = false;
            bool didUpdate_NoManagers = false;
            bool didUpdate_IncorrectVersion = false;

            // incorrect threadId
            try
            {
                threadApi.UpdateThread(
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    false,
                    false
                );
                didUpdate_IncorrectThreadId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update thread failed (incorrect threadId).\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectThreadId, Is.False);

            // incorrect users
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    false,
                    false
                );
                didUpdate_IncorrectUsers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update thread failed (incorrect users).\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectUsers, Is.False);

            // incorrect managers
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    false,
                    false
                );
                didUpdate_IncorrectManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update thread failed (incorrect managers).\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectManagers, Is.False);

            // no managers
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_id", "Login"),
                            UserId = config.Read("user_1_pubKey")
                        }
                    },
                    new List<UserWithPubKey>(),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    false,
                    false
                );
                didUpdate_NoManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update thread failed (no managers).\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_NoManagers, Is.False);

            // ??incorrect version force fail??
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    99,
                    false,
                    false
                );
                didUpdate_IncorrectVersion = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update thread failed (incorrect version).\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectVersion, Is.False);
        }

        [Test, Description("Update thread with correct data - 5 tries: new users, new managers, less users, less managers, ??incorrect version force true??")]
        public void UpdateThread_CorrectInput()
        {
            bool didUpdate_NewUsers = false;
            bool didUpdate_NewManagers = false;
            bool didUpdate_LessUsers = false;
            bool didUpdate_LessManagers = false;

            // new users
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    false,
                    false
                );
                didUpdate_NewUsers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update thread failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_NewUsers, Is.True);

            try
            {
                Thread thread = threadApi.GetThread(config.Read("threadId", "Thread_1"));
                Assert.Multiple(() =>
                {
                    Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(thread.Version, Is.EqualTo(2));
                    Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo("public"));
                    Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo("private"));
                    Assert.That(thread.Users, Has.Count.EqualTo(2));
                    if (thread.Users.Count == 2)
                    {
                        Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    Assert.That(thread.Managers, Has.Count.EqualTo(1));
                    if (thread.Managers.Count == 1)
                    {
                        Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting thread failed.\nMessage: {e.Message}");
            }

            // new managers
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    2,
                    false,
                    false
                );
                didUpdate_NewManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update thread failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_NewManagers, Is.True);

            try
            {
                Thread thread = threadApi.GetThread(config.Read("threadId", "Thread_1"));
                Assert.Multiple(() =>
                {
                    Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(thread.Version, Is.EqualTo(3));
                    Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo("public"));
                    Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo("private"));
                    Assert.That(thread.Users, Has.Count.EqualTo(1));
                    if (thread.Users.Count == 1)
                    {
                        Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(thread.Managers, Has.Count.EqualTo(1));
                    if (thread.Managers.Count == 1)
                    {
                        Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting thread failed.\nMessage: {e.Message}");
            }

            // less users
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_2"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    false,
                    false
                );
                didUpdate_LessUsers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update thread failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_LessUsers, Is.True);

            try
            {
                Thread thread = threadApi.GetThread(config.Read("threadId", "Thread_2"));
                Assert.Multiple(() =>
                {
                    Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(thread.Version, Is.EqualTo(2));
                    Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo("public"));
                    Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo("private"));
                    Assert.That(thread.Users, Has.Count.EqualTo(1));
                    if (thread.Users.Count == 1)
                    {
                        Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(thread.Managers, Has.Count.EqualTo(1));
                    if (thread.Managers.Count == 2)
                    {
                        Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.Managers[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting thread failed.\nMessage: {e.Message}");
            }

            // less managers
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_2"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    2,
                    false,
                    false
                );
                didUpdate_LessManagers = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update thread failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_LessManagers, Is.True);

            try
            {
                Thread thread = threadApi.GetThread(config.Read("threadId", "Thread_2"));
                Assert.Multiple(() =>
                {
                    Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(thread.Version, Is.EqualTo(3));
                    Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo("public"));
                    Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo("private"));
                    Assert.That(thread.Users, Has.Count.EqualTo(1));
                    if (thread.Users.Count == 1)
                    {
                        Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(thread.Managers, Has.Count.EqualTo(1));
                    if (thread.Managers.Count == 1)
                    {
                        Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting thread failed.\nMessage: {e.Message}");
            }

            // ??incorrect version force true??
            /*
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_3"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    99,
                    false,
                    false
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Update thread failed. Message: {e.Message}");
            }

            try
            {
                Thread thread = threadApi.GetThread(config.Read("threadId", "Thread_3"));
                Assert.Multiple(() =>
                {
                    Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(thread.Version, Is.EqualTo(2));
                    Assert.That(ByteArrayToString(thread.PublicMeta), Is.EqualTo("public"));
                    Assert.That(ByteArrayToString(thread.PrivateMeta), Is.EqualTo("private"));
                    Assert.That(thread.Users, Has.Count.EqualTo(1));
                    if (thread.Users.Count == 1)
                    {
                        Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                    Assert.That(thread.Managers, Has.Count.EqualTo(1));
                    if (thread.Managers.Count == 1)
                    {
                        Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting thread failed: Message: {e.Message}");
            }
            */
        }

        [Test, Description("Delete thread - 4 tries: 3 incorrect - incorrect threadId, already deleted, as user; 1 correct - as manager")]
        public void DeleteThread()
        {
            bool didDelete_IncorrectThreadId = false;
            bool didDelete_AlreadyDeleted = false;
            bool didDelete_AsUser = false;
            bool didDelete_AsManager = false;

            // incorrect threadId
            try
            {
                threadApi.DeleteThread(config.Read("contextId", "Context_1"));
                didDelete_IncorrectThreadId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete thread failed. Try: Incorrect ThreadId.\nMessage:{e.Message}");
            }
            Assert.That(didDelete_IncorrectThreadId, Is.False);

            // as manager
            try
            {
                threadApi.DeleteThread(config.Read("threadId", "Thread_3"));
                didDelete_AsManager = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Delete thread failed. Try: As manager.\nMessage:{e.Message}");
            }
            Assert.That(didDelete_AsManager, Is.True);

            // as manager - thread already deleted
            try
            {
                threadApi.DeleteThread(config.Read("threadId", "Thread_3"));
                didDelete_AlreadyDeleted = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete thread failed. Try: Already deleted.\nMessage:{e.Message}");
            }
            Assert.That(didDelete_AlreadyDeleted, Is.False);

            // as user
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User2);
            try
            {
                threadApi.DeleteThread(config.Read("threadId", "Thread_3"));
                didDelete_AsUser = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete thread failed. Try: As user.\nMessage:{e.Message}");
            }
            Assert.That(didDelete_AsUser, Is.False);
        }

        [Test, Description("Get message 2 tries: incorrect messageId, correct input, but force key generation on thread")]
        public void GetMessage()
        {
            bool didGetMessage_IncorrectMessageId = false;
            bool didGetMessage_CorrectInput_ForceKeyGen = false;

            // incorrect messageId
            try
            {
                threadApi.GetMessage(config.Read("contextId", "Context_1"));
                didGetMessage_IncorrectMessageId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to get message. Try: incorrect messageId.\nMessage: {e.Message}");
            }
            Assert.That(didGetMessage_IncorrectMessageId, Is.False);

            // correct, after force key generation on thread
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    true,
                    true
                );
                didGetMessage_CorrectInput_ForceKeyGen = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to update thread. Try: after force key generation on thread.\nMessage: {e.Message}");
            }
            Assert.That(didGetMessage_CorrectInput_ForceKeyGen, Is.True);

            try
            {
                Message message = threadApi.GetMessage(config.Read("info_messageId", "Message_2"));

                Assert.Multiple(() =>
                {
                    Assert.That(message.Info.ThreadId, Is.EqualTo(config.Read("info_threadId", "Message_2")));
                    Assert.That(message.Info.MessageId, Is.EqualTo(config.Read("info_messageId", "Message_2")));
                    Assert.That(message.Info.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("info_createDate", "Message_2"))));
                    Assert.That(message.Info.Author, Is.EqualTo(config.Read("info_author", "Message_2")));
                    Assert.That(ByteArrayToString(message.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Message_2")));
                    Assert.That(ByteArrayToString(message.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Message_2")));
                    Assert.That(ByteArrayToString(message.Data), Is.EqualTo(config.Read("data_inHex", "Message_2")));
                    Assert.That(message.StatusCode, Is.EqualTo(0));
                    //Assert.That(System.Text.Json.JsonSerializer.Serialize(message.Info), Is.EqualTo(config.Read("JSON_data", "Message_2")));
                    Assert.That(ByteArrayToString(message.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Message_2")));
                    Assert.That(ByteArrayToString(message.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Message_2")));
                    Assert.That(ByteArrayToString(message.Data), Is.EqualTo(config.Read("uploaded_data_inHex", "Message_2")));
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to get the message.\nMessage: {e.Message}");
            }

        }

        [Test, Description("List messages for a specified thread with incorrect input data. 5 Tries: incorrect threadId, limit < 0, limit == 0, incorrect sortOrder, incorrect lastId.")]
        public void ListMessages_IncorrectInput()
        {
            bool didListMessage_IncorrectThreadId = false;
            bool didListMessages_LimitLessThan0 = false;
            bool didListMessages_Limit0 = false;
            bool didListMessages_IncorrectSortOrder = false;
            bool didListMessages_IncorrectLastId = false;

            // incorrect threadId
            try
            {
                threadApi.ListMessages(config.Read("info_messageId", "Message_2"), SetPagingQuery(0, 1, "desc"));
                didListMessage_IncorrectThreadId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list thread messages. Try: incorrect threadId.\nMessage: {e.Message}");
            }
            Assert.That(didListMessage_IncorrectThreadId, Is.False);

            // limit < 0
            try
            {
                threadApi.ListMessages(config.Read("threadId", "Thread_1"), SetPagingQuery(0, -1, "desc"));
                didListMessages_LimitLessThan0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list thread messages. Try: limit < 0.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages_LimitLessThan0, Is.False);

            // limit == 0
            try
            {
                threadApi.ListMessages(config.Read("threadId", "Thread_1"), SetPagingQuery(0, 0, "desc"));
                didListMessages_Limit0 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list thread messages. Try: limit == 0.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages_Limit0, Is.False);

            // incorrect sortOrder
            try
            {
                threadApi.ListMessages(config.Read("threadId", "Thread_1"), SetPagingQuery(0, 1, "BLACH"));
                didListMessages_IncorrectSortOrder = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list thread messages. Try: incorrect sortOrder.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages_IncorrectSortOrder, Is.False);

            // incorrect lastId
            try
            {
                threadApi.ListMessages(config.Read("threadId", "Thread_1"), SetPagingQuery(0, 1, "desc", config.Read("threadId", "Thread_1")));
                didListMessages_IncorrectLastId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list thread messages. Try: incorrect lastId.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages_IncorrectLastId, Is.False);
        }

        [Test, Description("List messages for a specified thread with correct input data. 3 tries, one with forced key generation on thread.")]
        public void ListMessages_CorrectInput()
        {
            // {.skip=4, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<Message> listMessages = threadApi.ListMessages(
                    config.Read("threadId", "Thread_1"), SetPagingQuery(4, 1, "desc"));

                Assert.Multiple(() =>
                {
                    Assert.That(listMessages.TotalAvailable, Is.EqualTo(2));
                    Assert.That(listMessages.ReadItems, Has.Count.EqualTo(0));
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list thread messages. Try 1.\nMessage: {e.Message}");
            }

            // {.skip=1, .limit=1, .sortOrder="desc"}
            try
            {
                PagingList<Message> listMessages = threadApi.ListMessages(
                    config.Read("threadId", "Thread_1"), SetPagingQuery(1, 1, "desc"));

                Assert.Multiple(() =>
                {
                    Assert.That(listMessages.TotalAvailable, Is.EqualTo(2));
                    Assert.That(listMessages.ReadItems, Has.Count.EqualTo(1));
                    if (listMessages.ReadItems.Count >= 1)
                    {
                        Message message = listMessages.ReadItems[0];
                        Assert.That(message.Info.ThreadId, Is.EqualTo(config.Read("info_threadId", "Message_1")));
                        Assert.That(message.Info.MessageId, Is.EqualTo(config.Read("info_messageId", "Message_1")));
                        Assert.That(message.Info.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("info_createDate", "Message_1"))));
                        Assert.That(message.Info.Author, Is.EqualTo(config.Read("info_author", "Message_1")));
                        Assert.That(ByteArrayToString(message.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Message_1")));
                        Assert.That(ByteArrayToString(message.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Message_1")));
                        Assert.That(ByteArrayToString(message.Data), Is.EqualTo(config.Read("data_inHex", "Message_1")));
                        Assert.That(message.StatusCode, Is.EqualTo(0));
                        //Assert.That(System.Text.Json.JsonSerializer.Serialize(message), Is.EqualTo(config.Read("JSON_data", "Message_1")));
                        Assert.That(ByteArrayToString(message.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Message_1")));
                        Assert.That(ByteArrayToString(message.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Message_1")));
                        Assert.That(ByteArrayToString(message.Data), Is.EqualTo(config.Read("uploaded_data_inHex", "Message_1")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list thread messages. Try 2.\nMessage: {e.Message}");
            }

            // {.skip=0, .limit=3, .sortOrder="asc"}, after force key generation on thread
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    true,
                    true
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to update thread. Try: 3. Message: {e.Message}");
            }

            try
            {
                PagingList<Message> listMessages = threadApi.ListMessages(
                    config.Read("threadId", "Thread_1"), SetPagingQuery(0, 3, "asc"));

                Assert.Multiple(() =>
                {
                    Assert.That(listMessages.TotalAvailable, Is.EqualTo(2));
                    Assert.That(listMessages.ReadItems, Has.Count.EqualTo(2));
                    if (listMessages.ReadItems.Count >= 1)
                    {
                        Message message = listMessages.ReadItems[0];
                        Assert.That(message.Info.ThreadId, Is.EqualTo(config.Read("info_threadId", "Message_1")));
                        Assert.That(message.Info.MessageId, Is.EqualTo(config.Read("info_messageId", "Message_1")));
                        Assert.That(message.Info.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("info_createDate", "Message_1"))));
                        Assert.That(message.Info.Author, Is.EqualTo(config.Read("info_author", "Message_1")));
                        Assert.That(ByteArrayToString(message.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Message_1")));
                        Assert.That(ByteArrayToString(message.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Message_1")));
                        Assert.That(ByteArrayToString(message.Data), Is.EqualTo(config.Read("data_inHex", "Message_1")));
                        Assert.That(message.StatusCode, Is.EqualTo(0));
                        Assert.That(System.Text.Json.JsonSerializer.Serialize(message), Is.EqualTo(config.Read("JSON_data", "Message_1")));
                        Assert.That(ByteArrayToString(message.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Message_1")));
                        Assert.That(ByteArrayToString(message.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Message_1")));
                        Assert.That(ByteArrayToString(message.Data), Is.EqualTo(config.Read("uploaded_data_inHex", "Message_1")));
                    }

                    if (listMessages.ReadItems.Count >= 2)
                    {
                        Message message = listMessages.ReadItems[1];
                        Assert.That(message.Info.ThreadId, Is.EqualTo(config.Read("info_threadId", "Message_2")));
                        Assert.That(message.Info.MessageId, Is.EqualTo(config.Read("info_messageId", "Message_2")));
                        Assert.That(message.Info.CreateDate, Is.EqualTo((long)Convert.ToDouble(config.Read("info_createDate", "Message_2"))));
                        Assert.That(message.Info.Author, Is.EqualTo(config.Read("info_author", "Message_2")));
                        Assert.That(ByteArrayToString(message.PublicMeta), Is.EqualTo(config.Read("publicMeta_inHex", "Message_2")));
                        Assert.That(ByteArrayToString(message.PrivateMeta), Is.EqualTo(config.Read("privateMeta_inHex", "Message_2")));
                        Assert.That(ByteArrayToString(message.Data), Is.EqualTo(config.Read("data_inHex", "Message_2")));
                        Assert.That(message.StatusCode, Is.EqualTo(0));
                        Assert.That(System.Text.Json.JsonSerializer.Serialize(message), Is.EqualTo(config.Read("JSON_data", "Message_2")));
                        Assert.That(ByteArrayToString(message.PublicMeta), Is.EqualTo(config.Read("uploaded_publicMeta_inHex", "Message_2")));
                        Assert.That(ByteArrayToString(message.PrivateMeta), Is.EqualTo(config.Read("uploaded_privateMeta_inHex", "Message_2")));
                        Assert.That(ByteArrayToString(message.Data), Is.EqualTo(config.Read("uploaded_data_inHex", "Message_2")));
                    }
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to list thread messages. Try 3.\nMessage: {e.Message}");
            }
        }

        [Test, Description("Send message, 3 tries: 2 incorrect, 1 correct.")]
        public void SendMessage()
        {
            string messageId = string.Empty;
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = [];
            byte[] data = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data");

            bool didCreate_IncorrectThreadId = false;
            bool didCreate_TotalDataTooBig = false;
            bool didCreate_CorrectData = false;

            // incorrect_threadId
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

                messageId = threadApi.SendMessage(
                    config.Read("contextId", "Context_1"),
                    publicMeta,
                    privateMeta,
                    data
                );
                didCreate_IncorrectThreadId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to send the message. Try: incorrect threadId.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_IncorrectThreadId, Is.False);

            // msg total data bigger then 1MB
            byte[] randomData = new byte[1024 * 1024];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomData);
            };

            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

                messageId = threadApi.SendMessage(
                    config.Read("threadId", "Thread_1"),
                    randomData,
                    privateMeta,
                    data
                );
                didCreate_TotalDataTooBig = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to send the message. Try: msg total data bigger then 1MB.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_TotalDataTooBig, Is.False);

            // correct data
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

                messageId = threadApi.SendMessage(
                    config.Read("threadId", "Thread_1"),
                    publicMeta,
                    privateMeta,
                    data
                );
                didCreate_CorrectData = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to send the message. Try: correct data.\nMessage: {e.Message}");
            }
            Assert.That(didCreate_CorrectData, Is.True);

            try
            {
                Message message = threadApi.GetMessage(messageId);

                Assert.Multiple(() =>
                {
                    Assert.That(message.Data, Has.Length.EqualTo(data.Length));
                    if (message.Data.Length == data.Length)
                    {
                        Assert.That(ByteArrayToString(message.Data), Is.EqualTo(ByteArrayToString(data)));
                    }
                    Assert.That(ByteArrayToString(message.PublicMeta), Is.EqualTo(ByteArrayToString(publicMeta)));
                    Assert.That(ByteArrayToString(message.PrivateMeta), Is.EqualTo(ByteArrayToString(privateMeta)));
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to get the message. Try: correct data.\nMessage: {e.Message}");
            }
        }

        [Test, Description("Update message, 3 tries: 2 incorrect, 1 correct.")]
        public void UpdateMessage()
        {
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = [];
            byte[] data = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data");
            bool didUpdate_IncorrectMessageId = false;
            bool didUpdate_TotalDataTooBig = false;
            bool didUpdate_CorrectData = false;

            // incorrect messageId
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

                threadApi.UpdateMessage(
                    config.Read("threadId", "Thread_1"),
                    publicMeta,
                    privateMeta,
                    data
                );
                didUpdate_IncorrectMessageId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to update the message. Try: incorrect messageId.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_IncorrectMessageId, Is.False);

            // msg total data bigger then 1MB
            byte[] randomData = new byte[1024 * 1024];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomData);
            };

            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

                threadApi.UpdateMessage(
                    config.Read("info_messageId", "Message_1"),
                    randomData,
                    privateMeta,
                    data
                );
                didUpdate_TotalDataTooBig = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to update the message. Try: msg total data bigger then 1MB.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_TotalDataTooBig, Is.False);

            // correct data
            try
            {
                privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

                threadApi.UpdateMessage(
                    config.Read("info_messageId", "Message_1"),
                    publicMeta,
                    privateMeta,
                    data
                );
                didUpdate_CorrectData = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to update the message. Try: correct data.\nMessage: {e.Message}");
            }
            Assert.That(didUpdate_CorrectData, Is.True);

            try
            {
                Message message = threadApi.GetMessage(config.Read("info_messageId", "Message_1"));

                Assert.Multiple(() =>
                {
                    Assert.That(message.Data, Has.Length.EqualTo(data.Length));
                    if (message.Data.Length == data.Length)
                    {
                        Assert.That(ByteArrayToString(message.Data), Is.EqualTo(ByteArrayToString(data)));
                    }
                    Assert.That(ByteArrayToString(message.PublicMeta), Is.EqualTo(ByteArrayToString(publicMeta)));
                    Assert.That(ByteArrayToString(message.PrivateMeta), Is.EqualTo(ByteArrayToString(privateMeta)));
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to get the message. Try: correct data.\nMessage: {e.Message}");
            }
        }

        [Test, Description("Delete a message, 4 tries: 2 incorrect, 2 correct.")]
        public void DeleteMessage()
        {
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            bool didDelete_IncorrectMessageId = false;
            bool didDelete_AsUser_NotTheirMessage = false;
            bool didDelete_AsUser_TheirMessage = false;
            bool didDelete_AsManager_NotTheirMessage = false;

            // incorrect messageId
            try
            {
                threadApi.DeleteMessage(config.Read("threadId", "Thread_3"));
                didDelete_IncorrectMessageId = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete the message. Try: incorrect messageId.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_IncorrectMessageId, Is.False);

            // change privileges
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    true,
                    false
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to change privlages.\nMessage: {e.Message}");
            }

            // as user not created by me
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User2);
            try
            {
                threadApi.DeleteMessage(config.Read("info_messageId", "Message_2"));
                didDelete_AsUser_NotTheirMessage = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete the message. Try: as user - not their message.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_AsUser_NotTheirMessage, Is.False);

            // change privileges
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User1);
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    true,
                    false
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to change privlages.\nMessage: {e.Message}");
            }
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User2);
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    true,
                    false
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to change privlages.\nMessage: {e.Message}");
            }

            // as user created by me
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User1);
            try
            {
                threadApi.DeleteMessage(config.Read("info_messageId", "Message_2"));
                didDelete_AsUser_TheirMessage = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete the message. Try: as user - their message.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_AsUser_TheirMessage, Is.True);

            // as manager no created by me
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User2);
            try
            {
                threadApi.DeleteMessage(config.Read("info_messageId", "Message_1"));
                didDelete_AsManager_NotTheirMessage = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete the message. Try: as manager - not their message.\nMessage: {e.Message}");
            }
            Assert.That(didDelete_AsManager_NotTheirMessage, Is.True);
        }

        [Test, Description("Run all thread/message actions for user2, that is not in users or managers.")]
        public void AccessDenied_NotInUsersOrManagers()
        {
            bool didGetThread = false;
            bool didUpdateThread = false;
            bool didDeleteThread = false;
            bool didGetMessage = false;
            bool didListMessages = false;
            bool didSendMessage = false;
            bool didUpdateMessage = false;
            bool didDeleteMessage = false;

            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User2);

            // getThread
            try
            {
                Thread thread = threadApi.GetThread(config.Read("threadId", "Thread_1"));
                didGetThread = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Getting thread failed.\nMessage: {e.Message}");
            }
            Assert.That(didGetThread, Is.False);

            // updateThread
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    false,
                    false
                );
                didUpdateThread = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update thread failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdateThread, Is.False);

            // deleteThread
            try
            {
                threadApi.DeleteThread(config.Read("threadId", "Thread_1"));
                didDeleteThread = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete thread failed.\nMessage:{e.Message}");
            }
            Assert.That(didDeleteThread, Is.False);

            // getMessage
            try
            {
                threadApi.GetMessage(config.Read("info_messageId", "Message_1"));
                didGetMessage = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to get message.\nMessage: {e.Message}");
            }
            Assert.That(didGetMessage, Is.False);

            // listMessages
            try
            {
                PagingList<Message> listMessages = threadApi.ListMessages(
                    config.Read("threadId", "Thread_1"), SetPagingQuery(0, 1, "desc"));
                didListMessages = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list thread messages.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages, Is.False);

            // sendMessage
            try
            {
                threadApi.SendMessage(
                    config.Read("threadId", "Thread_1"),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data")
                );
                didSendMessage = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to send the message.\nMessage: {e.Message}");
            }
            Assert.That(didSendMessage, Is.False);

            // updateMessage
            try
            {
                threadApi.UpdateMessage(
                    config.Read("info_messageId", "Message_1"),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data")
                );
                didUpdateMessage = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to update the message.\nMessage: {e.Message}");
            }
            Assert.That(didUpdateMessage, Is.False);

            // deleteMessage
            try
            {
                threadApi.DeleteMessage(config.Read("info_messageId", "Message_1"));
                didDeleteMessage = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete the message.\nMessage: {e.Message}");
            }
            Assert.That(didDeleteMessage, Is.False);
        }

        [Test, Description("Run all thread/message actions for public.")]
        public void AccessDenied_Public()
        {
            bool didGetThread = false;
            bool didListThreads = false;
            bool didCreateThread = false;
            bool didUpdateThread = false;
            bool didDeleteThread = false;
            bool didGetMessage = false;
            bool didListMessages = false;
            bool didSendMessage = false;
            bool didUpdateMessage = false;
            bool didDeleteMessage = false;

            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.Public);

            // getThread
            try
            {
                Thread thread = threadApi.GetThread(config.Read("threadId", "Thread_1"));
                didGetThread = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Getting thread failed.\nMessage: {e.Message}");
            }
            Assert.That(didGetThread, Is.False);

            // listThreads
            try
            {
                threadApi.ListThreads(config.Read("contextId", "Context_1"), SetPagingQuery(0, 1, "desc"));
                didListThreads = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Listing threads failed.\nMessage: {e.Message}");
            }
            Assert.That(didListThreads, Is.False);

            //createThread
            try
            {
                threadApi.CreateThread(
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
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()))
                );
                didCreateThread = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"CreateThread failed (incorrect contextId try)\nMessage: {e.Message}");
            }
            Assert.That(didCreateThread, Is.False);

            // updateThread
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
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
                            PubKey = config.Read("user_2_id", "Login"),
                            UserId = config.Read("user_2_pubKey")
                        }
                    },
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    1,
                    false,
                    false
                );
                didUpdateThread = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Update thread failed.\nMessage: {e.Message}");
            }
            Assert.That(didUpdateThread, Is.False);

            // deleteThread
            try
            {
                threadApi.DeleteThread(config.Read("threadId", "Thread_1"));
                didDeleteThread = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Delete thread failed.\nMessage:{e.Message}");
            }
            Assert.That(didDeleteThread, Is.False);

            // getMessage
            try
            {
                threadApi.GetMessage(config.Read("info_messageId", "Message_1"));
                didGetMessage = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to get message.\nMessage: {e.Message}");
            }
            Assert.That(didGetMessage, Is.False);

            // listMessages
            try
            {
                PagingList<Message> listMessages = threadApi.ListMessages(
                    config.Read("threadId", "Thread_1"), SetPagingQuery(0, 1, "desc"));
                didListMessages = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to list thread messages.\nMessage: {e.Message}");
            }
            Assert.That(didListMessages, Is.False);

            // sendMessage
            try
            {
                threadApi.SendMessage(
                    config.Read("threadId", "Thread_1"),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data")
                );
                didSendMessage = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to send the message.\nMessage: {e.Message}");
            }
            Assert.That(didSendMessage, Is.False);

            // updateMessage
            try
            {
                threadApi.UpdateMessage(
                    config.Read("info_messageId", "Message_1"),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta()),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString())),
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data")
                );
                didUpdateMessage = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to update the message.\nMessage: {e.Message}");
            }
            Assert.That(didUpdateMessage, Is.False);

            // deleteMessage
            try
            {
                threadApi.DeleteMessage(config.Read("info_messageId", "Message_1"));
                didDeleteMessage = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Failed to delete the message.\nMessage: {e.Message}");
            }
            Assert.That(didDeleteMessage, Is.False);
        }

        [Test, Description("Create thread with policy. Try to get it as User2 afterwards.")]
        public void CreateThread_Policy()
        {
            string threadId = string.Empty;
            Thread thread = null;
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            bool didGetThread_User2 = false;

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
                UpdaterCanBeRemovedFromManagers = "no",
                OwnerCanBeRemovedFromManagers = "no"
            };

            try
            {
                threadId = threadApi.CreateThread(
                    config.Read("threadId", "Thread_1"),
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
                    policy
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to create thread.\nMessage: {e.Message}");
            }

            // cannot test everything - cannot get policy from thread
            try
            {
                thread = threadApi.GetThread(threadId);

                Assert.Multiple(() =>
                {
                    Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(thread.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(thread.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(thread.Users, Has.Count.EqualTo(2));
                    if(thread.Users.Count == 2)
                    {
                        Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    Assert.That(thread.Managers, Has.Count.EqualTo(2));
                    if (thread.Managers.Count == 2)
                    {
                        Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.Managers[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    //asserts for policy
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting thread failed.\nMessage: {e.Message}");
            }

            //get on user2 throws, because thread.Get is set to owner?
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User2);
            try
            {
                thread = threadApi.GetThread(threadId);
                didGetThread_User2 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Getting thread (user2) failed.\nMessage: {e.Message}");
            }
            Assert.That(didGetThread_User2 , Is.False);
        }

        [Test, Description("Update thread policy. Try to get message as User2 afterwards.")]
        public void UpdateThread_Policy()
        {
            string threadId = config.Read("threadId", "Thread_1");
            Thread thread = null;
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            bool didGetMessage_User2 = false;

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
                UpdaterCanBeRemovedFromManagers = "no",
                OwnerCanBeRemovedFromManagers = "no"
            };

            try
            {
                threadApi.UpdateThread(
                    threadId,
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
                    true,
                    policy
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Failed to create thread.\nMessage: {e.Message}");
            }

            // cannot test everything - cannot get policy from thread
            try
            {
                thread = threadApi.GetThread(threadId);

                Assert.Multiple(() =>
                {
                    Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    Assert.That(thread.PublicMeta, Is.EqualTo(publicMeta));
                    Assert.That(thread.PrivateMeta, Is.EqualTo(privateMeta));
                    Assert.That(thread.Users, Has.Count.EqualTo(2));
                    if (thread.Users.Count == 2)
                    {
                        Assert.That(thread.Users[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.Users[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    Assert.That(thread.Managers, Has.Count.EqualTo(2));
                    if (thread.Managers.Count == 2)
                    {
                        Assert.That(thread.Managers[0], Is.EqualTo(config.Read("user_1_id", "Login")));
                        Assert.That(thread.Managers[1], Is.EqualTo(config.Read("user_2_id", "Login")));
                    }
                    //asserts for policy
                });
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting thread failed.\nMessage: {e.Message}");
            }

            //get on user2 throws, because thread.Get is set to owner?
            Disconnect(ref connection);
            ConnectAs(ref connection, ConnectionType.User2);
            try
            {
                threadApi.GetMessage(config.Read("info_messageId", "Message_1"));
                didGetMessage_User2 = true;
            }
            catch (EndpointNativeException e)
            {
                Console.WriteLine($"Getting message (user2) failed.\nMessage: {e.Message}");
            }
            Assert.That(didGetMessage_User2, Is.False);
        }
    }
}
