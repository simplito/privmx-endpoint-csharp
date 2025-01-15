using EndpointCSharpTests.Utils;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Inbox;
using PrivMX.Endpoint.Store;
using PrivMX.Endpoint.Thread;
using PrivMX.Endpoint.Thread.Models;
using PrivMX.Endpoint.Store.Models;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Inbox.Models;
using EventQueue = PrivMX.Endpoint.Core.EventQueue;
using Event = PrivMX.Endpoint.Core.Models.Event;
using Thread = PrivMX.Endpoint.Thread.Models.Thread;
using File = PrivMX.Endpoint.Store.Models.File;

namespace EndpointCSharpTests
{
    internal class EventsTest : BaseTest
    {
        private Connection connection = null;
        private ThreadApi threadApi = null;
        private StoreApi storeApi = null;
        private InboxApi inboxApi = null;
        private EventQueue eventQueue = null;

        [SetUp]
        public virtual void Setup()
        {
            CustomConnect(ref connection, ConnectionType.User1);
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (connection != null)
            {
                CustomDisconnect(ref connection);
            }
        }

        private void CustomConnect(ref Connection connection, ConnectionType connectionType)
        {
            ConnectAs(ref connection, connectionType);
            threadApi = ThreadApi.Create(connection);
            storeApi = StoreApi.Create(connection);
            inboxApi = InboxApi.Create(connection, threadApi, storeApi);
            eventQueue = EventQueue.GetInstance();
        }

        private void CustomDisconnect(ref Connection connection)
        {
            Disconnect(ref connection);
            threadApi = null;
            storeApi = null;
            inboxApi = null;
            eventQueue = EventQueue.GetInstance();
        }

        [Test, Order(0), Description("Try to get an event. 1 try")]
        public void GetEvent_LibConnected()
        {
            Event privmxEvent = new Event();
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(1), Description("Try to get an event. Different instances try.")]
        public void GetEvent_LibConnectedDifferentInsances()
        {
            Connection connection2 = null;

            // pop libConnected form queue
            try
            {
                Event privmxEvent = new Event();
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // create another connection
            try
            {
                connection2 = Connection.Connect(config.Read("user_2_privKey", "Login"), config.Read("solutionId", "Login"), address);

                // check event
                try
                {
                    Event eventHolder = eventQueue.GetEvent();
                    if (eventHolder != null)
                    {
                        Assert.Multiple(() =>
                        {
                            Assert.That(eventHolder.Type, Is.EqualTo("libConnected"));
                            //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection2.GetInstanceId().ToString()));
                            Assert.That(eventHolder.GetType(), Is.EqualTo(typeof(LibConnectedEvent)));
                        });
                    }
                }
                catch (EndpointNativeException e)
                {
                    Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Connection failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(2), Description("Try to get LibDisconnected event.")]
        public void GetEvent_LibDisconnected()
        {
            // pop libConnected form queue
            try
            {
                Event privmxEvent = new Event();
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // disconnect
            try
            {
                Disconnect(ref connection);
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Disconnect failed.\nMessage: {e.Message}");
            }

            // check event
            try
            {
                Event eventHolder = eventQueue.GetEvent();
                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(eventHolder.Type, Is.EqualTo("libDisconnected"));
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.GetType(), Is.EqualTo(typeof(LibDisconnectedEvent)));
                    });
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(3), Description("Try to get PlatformDisconnected event.")]
        public void GetEvent_LibPlatformDisconnected()
        {
            // pop libConnected form queue
            try
            {
                Event privmxEvent = new Event();
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // disconnect
            try
            {
                Disconnect(ref connection);
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Disconnect failed.\nMessage: {e.Message}");
            }

            // pop libDisconnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // check event
            try
            {
                Event eventHolder = eventQueue.GetEvent();
                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(eventHolder.Type, Is.EqualTo("libPlatformDisconnected"));
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.GetType(), Is.EqualTo(typeof(LibPlatformDisconnectedEvent)));
                    });
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");

            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(4), Description("Try a sequence of: WaitEvent_GetEvent_ThreadCreated and check the returned values.")]
        public void WaitEvent_GetEvent_ThreadCreated_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            string threadId = string.Empty;

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to thread events
            try
            {
                threadApi.SubscribeForThreadEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // create thread
            try
            {
                threadId = threadApi.CreateThread(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                    privateMeta
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CreateThread failed.\nMessage: {e.Message}");
            }

            // check event and thread values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("threadCreated"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("thread"));
                    });

                    if (eventHolder.GetType() == typeof(ThreadCreatedEvent))
                    {
                        ThreadCreatedEvent threadEvent = (ThreadCreatedEvent)eventHolder;
                        Thread thread = threadEvent.Data;

                        Assert.Multiple(() =>
                        {
                            Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                            Assert.That(thread.PublicMeta, Is.EqualTo(publicMeta));
                            Assert.That(thread.PrivateMeta, Is.EqualTo(privateMeta));
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
                    else
                    {
                        Assert.Fail($"Event should be: ThreadCreatedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(5), Description("Try a sequence of: WaitEvent_GetEvent_ThreadCreated and check the returned values.")]
        public void WaitEvent_GetEvent_ThreadCreated_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            string threadId = string.Empty;

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe from thread events
            try
            {
                threadApi.SubscribeForThreadEvents();
                threadApi.UnsubscribeFromThreadEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // create thread
            try
            {
                threadId = threadApi.CreateThread(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                    privateMeta
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CreateThread failed.\nMessage: {e.Message}");
            }

            // check event and thread values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(6), Description("Try a sequence of: WaitEvent_GetEvent_ThreadUpdated and check the returned values.")]
        public void WaitEvent_GetEvent_ThreadUpdated_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to thread events
            try
            {
                threadApi.SubscribeForThreadEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // update thread
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                    1,
                    true,
                    true
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"UpdateThread failed.\nMessage: {e.Message}");
            }

            // check event and thread values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Console.WriteLine("event: " + StringToInt64(eventHolder.InstanceId) + " con: " + connection.GetInstanceId());
                        //Assert.That(StringToInt64(eventHolder.InstanceId), Is.EqualTo(connection.GetInstanceId()));
                        Assert.That(eventHolder.Type, Is.EqualTo("threadUpdated"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("thread"));
                    });

                    if (eventHolder.GetType() == typeof(ThreadUpdatedEvent))
                    {
                        ThreadUpdatedEvent threadEvent = (ThreadUpdatedEvent)eventHolder;
                        Thread thread = threadEvent.Data;

                        Assert.Multiple(() =>
                        {
                            Assert.That(thread.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                            Assert.That(thread.PublicMeta, Is.EqualTo(publicMeta));
                            Assert.That(thread.PrivateMeta, Is.EqualTo(privateMeta));
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
                    else
                    {
                        Assert.Fail($"Event should be: ThreadUpdatedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(7), Description("Try a sequence of: WaitEvent_GetEvent_ThreadUpdated and check the returned values.")]
        public void WaitEvent_GetEvent_ThreadUpdated_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe from thread events
            try
            {
                threadApi.SubscribeForThreadEvents();
                threadApi.UnsubscribeFromThreadEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // update thread
            try
            {
                threadApi.UpdateThread(
                    config.Read("threadId", "Thread_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                    1,
                    true,
                    true
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"UpdateThread failed.\nMessage: {e.Message}");
            }

            // check event and thread values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(114), Description("Try a sequence of: WaitEvent_GetEvent_threadDeleted and check the returned values.")]
        public void WaitEvent_GetEvent_ThreadDeleted_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to thread events
            try
            {
                threadApi.SubscribeForThreadEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete thread
            try
            {
                threadApi.DeleteThread(config.Read("threadId", "Thread_3"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteThread failed.\nMessage: {e.Message}");
            }

            // check event and thread values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("threadDeleted"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("thread"));
                    });

                    if (eventHolder.GetType() == typeof(ThreadDeletedEvent))
                    {
                        ThreadDeletedEvent threadEvent = (ThreadDeletedEvent)eventHolder;
                        ThreadDeletedEventData thread = threadEvent.Data;

                        Assert.That(thread.ThreadId, Is.EqualTo(config.Read("threadId", "Thread_3")));
                    }
                    else
                    {
                        Assert.Fail($"Event should be: ThreadDeletedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(115), Description("Try a sequence of: WaitEvent_GetEvent_threadDeleted and check the returned values.")]
        public void WaitEvent_GetEvent_ThreadDeleted_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe from thread events
            try
            {
                threadApi.SubscribeForThreadEvents();
                threadApi.UnsubscribeFromThreadEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete thread
            try
            {
                threadApi.DeleteThread(config.Read("threadId", "Thread_2"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteThread failed.\nMessage: {e.Message}");
            }

            // check event and thread values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(112), Description("Try a sequence of: WaitEvent_GetEvent_ThreadStatsChanged and check the returned values.")]
        public void WaitEvent_GetEvent_ThreadStatsChanged_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to thread events
            try
            {
                threadApi.SubscribeForThreadEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete message
            try
            {
                threadApi.DeleteMessage(config.Read("info_messageId", "Message_2"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteMessage failed.\nMessage: {e.Message}");
            }

            // check event and message values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("threadStatsChanged"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("thread"));
                    });

                    if (eventHolder.GetType() == typeof(ThreadStatsChangedEvent))
                    {
                        ThreadStatsChangedEvent threadEvent = (ThreadStatsChangedEvent)eventHolder;
                        ThreadStatsEventData thread = threadEvent.Data;

                        Assert.That(thread.ThreadId, Is.EqualTo(config.Read("threadId", "Thread_1")));
                        Assert.That(thread.MessagesCount, Is.EqualTo(1));
                    }
                    else
                    {
                        Assert.Fail($"Event should be: ThreadStatsChangedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(113), Description("Try a sequence of: WaitEvent_GetEvent_ThreadStatsChangedand check the returned values.")]
        public void WaitEvent_GetEvent_ThreadStatsChanged_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe from thread events
            try
            {
                threadApi.SubscribeForThreadEvents();
                threadApi.UnsubscribeFromThreadEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete message
            try
            {
                threadApi.DeleteMessage(config.Read("info_messageId", "Message_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteMessage failed.\nMessage: {e.Message}");
            }

            // check event and message values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(8), Description("Try a sequence of: WaitEvent_GetEvent_SendMessage and check the returned values.")]
        public void WaitEvent_GetEvent_SendMessage_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            byte[] data = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data");

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to message events
            try
            {
                threadApi.SubscribeForMessageEvents(config.Read("threadId", "Thread_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // send message
            try
            {
                threadApi.SendMessage(
                    config.Read("threadId", "Thread_1"),
                    publicMeta,
                    privateMeta,
                    data
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"SendMessage failed.\nMessage: {e.Message}");
            }

            // check event and message values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("threadNewMessage"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("thread/" + config.Read("threadId", "Thread_1") + "/messages"));
                    });

                    if (eventHolder.GetType() == typeof(ThreadNewMessageEvent))
                    {
                        ThreadNewMessageEvent threadEvent = (ThreadNewMessageEvent)eventHolder;
                        Message message = threadEvent.Data;

                        Assert.Multiple(() =>
                        {
                            Assert.That(message.PublicMeta, Is.EqualTo(publicMeta));
                            Assert.That(message.PrivateMeta, Is.EqualTo(privateMeta));
                            Assert.That(message.Data, Is.EqualTo(data));
                            Assert.That(message.Info.ThreadId, Is.EqualTo(config.Read("threadId", "Thread_1")));
                        });
                    }
                    else
                    {
                        Assert.Fail($"Event should be: ThreadNewMessageEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(9), Description("Try a sequence of: WaitEvent_GetEvent_SendMessage and check the returned values.")]
        public void WaitEvent_GetEvent_SendMessage_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            byte[] data = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data");

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe from message events
            try
            {
                threadApi.SubscribeForMessageEvents(config.Read("threadId", "Thread_1"));
                threadApi.UnsubscribeFromMessageEvents(config.Read("threadId", "Thread_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // send message
            try
            {
                threadApi.SendMessage(
                    config.Read("threadId", "Thread_1"),
                    publicMeta,
                    privateMeta,
                    data
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"SendMessage failed.\nMessage: {e.Message}");
            }

            // check event and message values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(10), Description("Try a sequence of: WaitEvent_GetEvent_UpdateMessage and check the returned values.")]
        public void WaitEvent_GetEvent_UpdateMessage_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            byte[] data = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data");

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to message events
            try
            {
                threadApi.SubscribeForMessageEvents(config.Read("threadId", "Thread_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // update message
            try
            {
                threadApi.UpdateMessage(
                    config.Read("info_messageId", "Message_1"),
                    publicMeta,
                    privateMeta,
                    data
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"UpdateMessage failed.\nMessage: {e.Message}");
            }

            // check event and message values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("threadUpdatedMessage"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("thread/" + config.Read("threadId", "Thread_1") + "/messages"));
                    });

                    if (eventHolder.GetType() == typeof(ThreadMessageUpdatedEvent))
                    {
                        ThreadMessageUpdatedEvent threadEvent = (ThreadMessageUpdatedEvent)eventHolder;
                        Message message = threadEvent.Data;

                        Assert.Multiple(() =>
                        {
                            Assert.That(message.Info.MessageId, Is.EqualTo(config.Read("info_messageId", "Message_1")));
                            Assert.That(message.PublicMeta, Is.EqualTo(publicMeta));
                            Assert.That(message.PrivateMeta, Is.EqualTo(privateMeta));
                            Assert.That(message.Data, Is.EqualTo(data));
                            Assert.That(message.Info.ThreadId, Is.EqualTo(config.Read("threadId", "Thread_1")));
                        });
                    }
                    else
                    {
                        Assert.Fail($"Event should be: ThreadMessageUpdatedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(11), Description("Try a sequence of: WaitEvent_GetEvent_UpdateMessage and check the returned values.")]
        public void WaitEvent_GetEvent_UpdateMessage_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            byte[] data = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("data");

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe to message events
            try
            {
                threadApi.SubscribeForMessageEvents(config.Read("threadId", "Thread_1"));
                threadApi.UnsubscribeFromMessageEvents(config.Read("threadId", "Thread_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // update message
            try
            {
                threadApi.UpdateMessage(
                    config.Read("info_messageId", "Message_1"),
                    publicMeta,
                    privateMeta,
                    data
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"UpdateMessage failed.\nMessage: {e.Message}");
            }

            // check event and message values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(110), Description("Try a sequence of: WaitEvent_GetEvent_DeleteMessage and check the returned values.")]
        public void WaitEvent_GetEvent_DeleteMessage_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to message events
            try
            {
                threadApi.SubscribeForMessageEvents(config.Read("threadId", "Thread_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete message
            try
            {
                threadApi.DeleteMessage(config.Read("info_messageId", "Message_2"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteMessage failed.\nMessage: {e.Message}");
            }

            // check event and message values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("threadMessageDeleted"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("thread/" + config.Read("threadId", "Thread_1") + "/messages"));
                    });

                    if (eventHolder.GetType() == typeof(ThreadMessageDeletedEvent))
                    {
                        ThreadMessageDeletedEvent threadEvent = (ThreadMessageDeletedEvent)eventHolder;
                        ThreadDeletedMessageEventData message = threadEvent.Data;

                        Assert.That(message.MessageId, Is.EqualTo(config.Read("info_messageId", "Message_2")));
                        Assert.That(message.ThreadId, Is.EqualTo(config.Read("threadId", "Thread_1")));
                    }
                    else
                    {
                        Assert.Fail($"Event should be: ThreadStatsChangedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        // reset instance
        [Test, Order(111), Description("Try a sequence of: WaitEvent_GetEvent_ThreadStatsChanged and check the returned values.")]
        public void WaitEvent_GetEvent_DeleteMessage_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe from message events
            try
            {
                threadApi.SubscribeForMessageEvents(config.Read("threadId", "Thread_1"));
                threadApi.UnsubscribeFromMessageEvents(config.Read("threadId", "Thread_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete message
            try
            {
                threadApi.DeleteMessage(config.Read("info_messageId", "Message_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteMessage failed.\nMessage: {e.Message}");
            }

            // check event and message values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(12), Description("Try a sequence of: WaitEvent_GetEvent_StoreCreated and check the returned values.")]
        public void WaitEvent_GetEvent_StoreCreated_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            string storeId = string.Empty;

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to store events
            try
            {
                storeApi.SubscribeForStoreEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // create store
            try
            {
                storeId = storeApi.CreateStore(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                    privateMeta
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CreateStore failed.\nMessage: {e.Message}");
            }

            // check event and store values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("storeCreated"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("store"));
                    });

                    if (eventHolder.GetType() == typeof(StoreCreatedEvent))
                    {
                        StoreCreatedEvent storeEvent = (StoreCreatedEvent)eventHolder;
                        Store store = storeEvent.Data;

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
                    else
                    {
                        Assert.Fail($"Event should be: StoreCreatedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(13), Description("Try a sequence of: WaitEvent_GetEvent_StoreCreated and check the returned values.")]
        public void WaitEvent_GetEvent_StoreCreated_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe to store events
            try
            {
                storeApi.SubscribeForStoreEvents();
                storeApi.UnsubscribeFromStoreEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // create store
            try
            {
                storeApi.CreateStore(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                    privateMeta
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CreateStore failed.\nMessage: {e.Message}");
            }

            // check event and store values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(14), Description("Try a sequence of: WaitEvent_GetEvent_StoreUpdated and check the returned values.")]
        public void WaitEvent_GetEvent_StoreUpdated_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to store events
            try
            {
                storeApi.SubscribeForStoreEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // update store
            try
            {
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                    1,
                    true,
                    true
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CreateStore failed.\nMessage: {e.Message}");
            }

            // check event and store values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("storeUpdated"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("store"));
                    });

                    if (eventHolder.GetType() == typeof(StoreUpdatedEvent))
                    {
                        StoreUpdatedEvent storeEvent = (StoreUpdatedEvent)eventHolder;
                        Store store = storeEvent.Data;

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
                    else
                    {
                        Assert.Fail($"Event should be: StoreUpdatedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(15), Description("Try a sequence of: WaitEvent_GetEvent_StoreUpdated and check the returned values.")]
        public void WaitEvent_GetEvent_StoreUpdated_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe to store events
            try
            {
                storeApi.SubscribeForStoreEvents();
                storeApi.UnsubscribeFromStoreEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // update store
            try
            {
                storeApi.UpdateStore(
                    config.Read("storeId", "Store_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                    1,
                    true,
                    true
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"UpdateStore failed.\nMessage: {e.Message}");
            }

            // check event and store values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(108), Description("Try a sequence of: WaitEvent_GetEvent_DeleteStore and check the returned values.")]
        public void WaitEvent_GetEvent_DeleteStore_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to store events
            try
            {
                storeApi.SubscribeForStoreEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete store
            try
            {
                storeApi.DeleteStore(config.Read("storeId", "Store_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteStore failed.\nMessage: {e.Message}");
            }

            // check event and store values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("storeDeleted"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("store"));
                    });

                    if (eventHolder.GetType() == typeof(StoreDeletedEvent))
                    {
                        StoreDeletedEvent storeEvent = (StoreDeletedEvent)eventHolder;
                        StoreDeletedEventData store = storeEvent.Data;

                        Assert.That(store.StoreId, Is.EqualTo(config.Read("storeId", "Store_1")));
                    }
                    else
                    {
                        Assert.Fail($"Event should be: StoreDeletedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(109), Description("Try a sequence of: WaitEvent_GetEvent_DeleteStore and check the returned values.")]
        public void WaitEvent_GetEvent_DeleteStore_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe to store events
            try
            {
                storeApi.SubscribeForStoreEvents();
                storeApi.UnsubscribeFromStoreEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete store
            try
            {
                storeApi.DeleteStore(config.Read("storeId", "Store_2"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteStore failed.\nMessage: {e.Message}");
            }

            // check event and store values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(106), Description("Try a sequence of: WaitEvent_GetEvent_StoreStatsChanged and check the returned values.")]
        public void WaitEvent_GetEvent_StoreStatsChanged_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to store events
            try
            {
                storeApi.SubscribeForStoreEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete file
            try
            {
                storeApi.DeleteFile(config.Read("info_fileId", "File_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteFile failed.\nMessage: {e.Message}");
            }

            // check event and store values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("storeStatsChanged"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("store"));
                    });

                    if (eventHolder.GetType() == typeof(StoreStatsChangedEvent))
                    {
                        StoreStatsChangedEvent storeEvent = (StoreStatsChangedEvent)eventHolder;
                        StoreStatsChangedEventData storeStat = storeEvent.Data;

                        Assert.Multiple(() =>
                        {
                            Assert.That(storeStat.StoreId, Is.EqualTo(config.Read("storeId", "Store_1")));
                            Assert.That(storeStat.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                            Assert.That(storeStat.FilesCount, Is.EqualTo(1));
                        });
                    }
                    else
                    {
                        Assert.Fail($"Event should be: StoreStatsChangedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(107), Description("Try a sequence of: WaitEvent_GetEvent_StoreStatsChanged and check the returned values.")]
        public void WaitEvent_GetEvent_StoreStatsChanged_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe to store events
            try
            {
                storeApi.SubscribeForStoreEvents();
                storeApi.UnsubscribeFromStoreEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete file
            try
            {
                storeApi.DeleteFile(config.Read("info_fileId", "File_2"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteFile failed.\nMessage: {e.Message}");
            }

            // check event and store values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(16), Description("Try a sequence of: WaitEvent_GetEvent_StoreFileCreated and check the returned values.")]
        public void WaitEvent_GetEvent_StoreFileCreated_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            long handle = 0;

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to store events
            try
            {
                storeApi.SubscribeForFileEvents(config.Read("storeId", "Store_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // create file
            try
            {
                handle = storeApi.CreateFile(
                    config.Read("storeId", "Store_1"),
                    publicMeta,
                    privateMeta,
                    0
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CreateFile failed.\nMessage: {e.Message}");
            }

            // close file
            try
            {
                string fileId = string.Empty;
                fileId = storeApi.CloseFile(handle);

                if(fileId == string.Empty) Assert.Fail($"CloseFile failed.");
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CloseFile failed.\nMessage: {e.Message}");
            }

            // check event and file values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("storeFileCreated"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("store/" + config.Read("storeId", "Store_1") + "/files"));
                    });

                    if (eventHolder.GetType() == typeof(StoreFileCreatedEvent))
                    {
                        StoreFileCreatedEvent fileEvent = (StoreFileCreatedEvent)eventHolder;
                        File file = fileEvent.Data;

                        Assert.Multiple(() =>
                        {
                            Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("storeId", "Store_1")));
                            Assert.That(file.Size, Is.EqualTo(0));
                            Assert.That(file.PublicMeta, Is.EqualTo(publicMeta));
                            Assert.That(file.PrivateMeta, Is.EqualTo(privateMeta));
                        });
                    }
                    else
                    {
                        Assert.Fail($"Event should be: StoreFileCreatedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(17), Description("Try a sequence of: WaitEvent_GetEvent_StoreFileCreated and check the returned values.")]
        public void WaitEvent_GetEvent_StoreFileCreated_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            long handle = 0;

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe to file events
            try
            {
                storeApi.SubscribeForFileEvents(config.Read("storeId", "Store_1"));
                storeApi.UnsubscribeFromFileEvents(config.Read("storeId", "Store_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // create file
            try
            {
                handle = storeApi.CreateFile(
                    config.Read("storeId", "Store_1"),
                    publicMeta,
                    privateMeta,
                    0
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CreateFile failed.\nMessage: {e.Message}");
            }

            // close file
            try
            {
                string fileId = string.Empty;
                fileId = storeApi.CloseFile(handle);

                if (fileId == string.Empty) Assert.Fail($"CloseFile failed.");
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CloseFile failed.\nMessage: {e.Message}");
            }

            // check event and store values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(18), Description("Try a sequence of: WaitEvent_GetEvent_StoreFileUpdated and check the returned values.")]
        public void WaitEvent_GetEvent_StoreFileUpdated_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            long handle = 0;

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to store events
            try
            {
                storeApi.SubscribeForFileEvents(config.Read("storeId", "Store_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // update file
            try
            {
                handle = storeApi.UpdateFile(
                    config.Read("info_fileId", "File_1"),
                    publicMeta,
                    privateMeta,
                    0
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"UpdateFile failed.\nMessage: {e.Message}");
            }

            // close file
            try
            {
                string fileId = string.Empty;
                fileId = storeApi.CloseFile(handle);

                if (fileId == string.Empty) Assert.Fail($"CloseFile failed.");
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CloseFile failed.\nMessage: {e.Message}");
            }

            // check event and file values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("storeFileUpdated"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("store/" + config.Read("storeId", "Store_1") + "/files"));
                    });

                    if (eventHolder.GetType() == typeof(StoreFileUpdatedEvent))
                    {
                        StoreFileUpdatedEvent fileEvent = (StoreFileUpdatedEvent)eventHolder;
                        File file = fileEvent.Data;

                        Assert.Multiple(() =>
                        {
                            Assert.That(file.Info.StoreId, Is.EqualTo(config.Read("storeId", "Store_1")));
                            Assert.That(file.Size, Is.EqualTo(0));
                            Assert.That(file.PublicMeta, Is.EqualTo(publicMeta));
                            Assert.That(file.PrivateMeta, Is.EqualTo(privateMeta));
                        });
                    }
                    else
                    {
                        Assert.Fail($"Event should be: StoreFileUpdatedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(19), Description("Try a sequence of: WaitEvent_GetEvent_StoreFileUpdated and check the returned values.")]
        public void WaitEvent_GetEvent_StoreFileUpdated_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            long handle = 0;

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe to file events
            try
            {
                storeApi.SubscribeForFileEvents(config.Read("storeId", "Store_1"));
                storeApi.UnsubscribeFromFileEvents(config.Read("storeId", "Store_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // update file
            try
            {
                handle = storeApi.UpdateFile(
                    config.Read("info_fileId", "File_1"),
                    publicMeta,
                    privateMeta,
                    0
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"UpdateFile failed.\nMessage: {e.Message}");
            }

            // close file
            try
            {
                string fileId = string.Empty;
                fileId = storeApi.CloseFile(handle);

                if (fileId == string.Empty) Assert.Fail($"CloseFile failed.");
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CloseFile failed.\nMessage: {e.Message}");
            }

            // check event and store values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(104), Description("Try a sequence of: WaitEvent_GetEvent_StoreFileDeleted and check the returned values.")]
        public void WaitEvent_GetEvent_StoreFileDeleted_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to file events
            try
            {
                storeApi.SubscribeForFileEvents(config.Read("storeId", "Store_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete file
            try
            {
                storeApi.DeleteFile(config.Read("info_fileId", "File_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteFile failed.\nMessage: {e.Message}");
            }

            // check event and file values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(StringToInt64(eventHolder.InstanceId), Is.EqualTo(connection.GetInstanceId()));
                        Assert.That(eventHolder.Type, Is.EqualTo("storeFileDeleted"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("store/" + config.Read("storeId", "Store_1") + "/files"));
                    });

                    if (eventHolder.GetType() == typeof(StoreFileDeletedEvent))
                    {
                        StoreFileDeletedEvent fileEvent = (StoreFileDeletedEvent)eventHolder;
                        StoreFileDeletedEventData file = fileEvent.Data;

                        Assert.Multiple(() =>
                        {
                            Assert.That(file.FileId, Is.EqualTo(config.Read("info_fileId", "File_1")));
                            Assert.That(file.StoreId, Is.EqualTo(config.Read("storeId", "Store_1")));
                            Assert.That(file.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));

                        });
                    }
                    else
                    {
                        Assert.Fail($"Event should be: StoreFileDeletedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(105), Description("Try a sequence of: WaitEvent_GetEvent_StoreFileDeleted and check the returned values.")]
        public void WaitEvent_GetEvent_StoreFileDeleted_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe to file events
            try
            {
                storeApi.SubscribeForFileEvents(config.Read("storeId", "Store_1"));
                storeApi.UnsubscribeFromFileEvents(config.Read("storeId", "Store_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete file
            try
            {
                storeApi.DeleteFile(config.Read("info_fileId", "File_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteFile failed.\nMessage: {e.Message}");
            }

            // check event and store values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(20), Description("Try a sequence of: WaitEvent_GetEvent_InboxCreated and check the returned values.")]
        public void WaitEvent_GetEvent_InboxCreated_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            string inboxId = string.Empty;

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to events
            try
            {
                inboxApi.SubscribeForInboxEvents();
                threadApi.SubscribeForThreadEvents();
                storeApi.SubscribeForStoreEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // create inbox
            try
            {
                inboxId = inboxApi.CreateInbox(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                Assert.Fail($"CreateInbox failed.\nMessage: {e.Message}");
            }

            // check event and inbox values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("inboxCreated"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("inbox"));
                    });

                    if (eventHolder.GetType() == typeof(InboxCreatedEvent))
                    {
                        InboxCreatedEvent inboxEvent = (InboxCreatedEvent)eventHolder;
                        Inbox inbox = inboxEvent.Data;

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
                    else
                    {
                        Assert.Fail($"Event should be: InboxCreatedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(21), Description("Try a sequence of: WaitEvent_GetEvent_InboxCreated and check the returned values.")]
        public void WaitEvent_GetEvent_InboxCreated_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe from events
            try
            {
                inboxApi.SubscribeForInboxEvents();
                threadApi.SubscribeForThreadEvents();
                storeApi.SubscribeForStoreEvents();
                inboxApi.UnsubscribeFromInboxEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // create inbox
            try
            {
                inboxApi.CreateInbox(
                    config.Read("contextId", "Context_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                Assert.Fail($"CreateInbox failed.\nMessage: {e.Message}");
            }

            // check event and inbox values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(22), Description("Try a sequence of: WaitEvent_GetEvent_InboxUpdated and check the returned values.")]
        public void WaitEvent_GetEvent_InboxUpdated_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to events
            try
            {
                inboxApi.SubscribeForInboxEvents();
                threadApi.SubscribeForThreadEvents();
                storeApi.SubscribeForStoreEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // update inbox
            try
            {
                inboxApi.UpdateInbox(
                    config.Read("inboxId", "Inbox_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                Assert.Fail($"UpdateInbox failed.\nMessage: {e.Message}");
            }

            // check event and inbox values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("inboxUpdated"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("inbox"));
                    });

                    if (eventHolder.GetType() == typeof(InboxUpdatedEvent))
                    {
                        InboxUpdatedEvent inboxEvent = (InboxUpdatedEvent)eventHolder;
                        Inbox inbox = inboxEvent.Data;

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
                    else
                    {
                        Assert.Fail($"Event should be: InboxUpdatedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(23), Description("Try a sequence of: WaitEvent_GetEvent_InboxUpdated and check the returned values.")]
        public void WaitEvent_GetEvent_InboxUpdated_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe from events
            try
            {
                inboxApi.SubscribeForInboxEvents();
                threadApi.SubscribeForThreadEvents();
                storeApi.SubscribeForStoreEvents();
                inboxApi.UnsubscribeFromInboxEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // update inbox
            try
            {
                inboxApi.UpdateInbox(
                    config.Read("inboxId", "Inbox_1"),
                    new List<UserWithPubKey>
                    {
                        new UserWithPubKey
                        {
                            PubKey = config.Read("user_1_pubKey", "Login"),
                            UserId = config.Read("user_1_id", "Login")
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
                Assert.Fail($"UpdateInbox failed.\nMessage: {e.Message}");
            }

            // check event and inbox values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(102), Description("Try a sequence of: WaitEvent_GetEvent_InboxDeleted and check the returned values.")]
        public void WaitEvent_GetEvent_InboxDeleted_Enabled()
        {
            Event privmxEvent = new Event();

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe to events
            try
            {
                inboxApi.SubscribeForInboxEvents();
                threadApi.SubscribeForThreadEvents();
                storeApi.SubscribeForStoreEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete inbox
            try
            {
                inboxApi.DeleteInbox(config.Read("inboxId", "Inbox_3"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteInbox failed.\nMessage: {e.Message}");
            }

            // check event and inbox values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("inboxDeleted"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("inbox"));
                    });

                    if (eventHolder.GetType() == typeof(InboxDeletedEvent))
                    {
                        InboxDeletedEvent inboxEvent = (InboxDeletedEvent)eventHolder;
                        InboxDeletedEventData inbox = inboxEvent.Data;

                        Assert.That(inbox.InboxId, Is.EqualTo(config.Read("inboxId", "Inbox_3")));
                    }
                    else
                    {
                        Assert.Fail($"Event should be: InboxDeletedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(103), Description("Try a sequence of: WaitEvent_GetEvent_InboxDeleted and check the returned values.")]
        public void WaitEvent_GetEvent_InboxDeleted_Disabled()
        {
            Event privmxEvent = new Event();

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe from events
            try
            {
                inboxApi.SubscribeForInboxEvents();
                threadApi.SubscribeForThreadEvents();
                storeApi.SubscribeForStoreEvents();
                inboxApi.UnsubscribeFromInboxEvents();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete inbox
            try
            {
                inboxApi.DeleteInbox(config.Read("inboxId", "Inbox_2"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteInbox failed.\nMessage: {e.Message}");
            }

            // check event and inbox values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(24), Description("Try a sequence of: WaitEvent_GetEvent_InboxEntryCreated and check the returned values.")]
        public void WaitEvent_GetEvent_InboxEntryCreated_Enabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            byte[] data = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("test_inboxSendCommit");
            long fileHandle = 0;
            long inboxHandle = 0;

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe entry to events
            try
            {
                inboxApi.SubscribeForEntryEvents(config.Read("inboxId", "Inbox_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // create file handle
            try
            {
                fileHandle = inboxApi.CreateFileHandle(
                    publicMeta,
                    privateMeta,
                    0
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CreateFileHandle failed.\nMessage: {e.Message}");
            }
            Assert.That(fileHandle, Is.EqualTo(1));

            if(fileHandle == 1)
            {
                // create entry
                try
                {
                    inboxHandle = inboxApi.PrepareEntry(
                        config.Read("inboxId", "Inbox_1"),
                        data,
                        new List<long>{ fileHandle },
                        config.Read("user_1_privKey", "Login")
                    );
                }
                catch (EndpointNativeException e)
                {
                    Assert.Fail($"PrepareEntry failed.\nMessage: {e.Message}");
                }
                Assert.That(inboxHandle, Is.EqualTo(2));

                if(inboxHandle == 2)
                {
                    try
                    {
                        inboxApi.SendEntry(inboxHandle);
                    }
                    catch (EndpointNativeException e)
                    {
                        Assert.Fail($"SendEntry failed.\nMessage: {e.Message}");
                    }
                }
            }

            // check event and inbox, entry values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("inboxEntryCreated"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("inbox/" + config.Read("inboxId", "Inbox_1") + "/entries"));
                    });

                    if (eventHolder.GetType() == typeof(InboxEntryCreatedEvent))
                    {
                        InboxEntryCreatedEvent inboxEvent = (InboxEntryCreatedEvent)eventHolder;
                        InboxEntry inboxEntry = inboxEvent.Data;

                        Assert.Multiple(() =>
                        {
                            Assert.That(inboxEntry.InboxId, Is.EqualTo(config.Read("inboxId", "Inbox_1")));
                            Assert.That(inboxEntry.Data, Is.EqualTo(data));
                            Assert.That(inboxEntry.Files, Has.Count.EqualTo(1));

                            if(inboxEntry.Files.Count == 1)
                            {
                                Assert.That(inboxEntry.Files[0].Size, Is.EqualTo(0));
                                Assert.That(inboxEntry.Files[0].PublicMeta, Is.EqualTo(publicMeta));
                                Assert.That(inboxEntry.Files[0].PrivateMeta, Is.EqualTo(privateMeta));
                            }
                        });
                    }
                    else
                    {
                        Assert.Fail($"Event should be: InboxCreatedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(25), Description("Try a sequence of: WaitEvent_GetEvent_InboxEntryCreated and check the returned values.")]
        public void WaitEvent_GetEvent_InboxEntryCreated_Disabled()
        {
            Event privmxEvent = new Event();
            byte[] publicMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPublicMeta());
            byte[] privateMeta = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ThreadPrivateMeta("text", Guid.NewGuid().ToString()));
            byte[] data = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes("test_inboxSendCommit");
            long fileHandle = 0;
            long inboxHandle = 0;

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe from events
            try
            {
                inboxApi.SubscribeForEntryEvents(config.Read("inboxId", "Inbox_1"));
                inboxApi.UnsubscribeFromEntryEvents(config.Read("inboxId", "Inbox_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // create file handle
            try
            {
                fileHandle = inboxApi.CreateFileHandle(
                    publicMeta,
                    privateMeta,
                    0
                );
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"CreateFileHandle failed.\nMessage: {e.Message}");
            }
            Assert.That(fileHandle, Is.EqualTo(1));

            if (fileHandle == 1)
            {
                // create entry
                try
                {
                    inboxHandle = inboxApi.PrepareEntry(
                        config.Read("inboxId", "Inbox_1"),
                        data,
                        new List<long> { fileHandle },
                        config.Read("user_1_privKey", "Login")
                    );
                }
                catch (EndpointNativeException e)
                {
                    Assert.Fail($"PrepareEntry failed.\nMessage: {e.Message}");
                }
                Assert.That(inboxHandle, Is.EqualTo(2));

                if (inboxHandle == 2)
                {
                    try
                    {
                        inboxApi.SendEntry(inboxHandle);
                    }
                    catch (EndpointNativeException e)
                    {
                        Assert.Fail($"SendEntry failed.\nMessage: {e.Message}");
                    }
                }
            }

            // check event and inbox values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }

        // eventHolder.InstanceId is null
        [Test, Order(100), Description("Try a sequence of: WaitEvent_GetEvent_InboxEntryDeleted and check the returned values.")]
        public void WaitEvent_GetEvent_InboxEntryDeleted_Enabled()
        {
            Event privmxEvent = new Event();

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe entry to events
            try
            {
                inboxApi.SubscribeForEntryEvents(config.Read("inboxId", "Inbox_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete entry handle
            try
            {
                inboxApi.DeleteEntry(config.Read("entryId", "Entry_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteEntry failed.\nMessage: {e.Message}");
            }

            // check event and entry values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Multiple(() =>
                    {
                        //Assert.That(eventHolder.InstanceId, Is.EqualTo(connection.GetInstanceId().ToString()));
                        Assert.That(eventHolder.Type, Is.EqualTo("inboxEntryDeleted"));
                        Assert.That(eventHolder.Channel, Is.EqualTo("inbox/" + config.Read("inboxId", "Inbox_1") + "/entries"));
                    });

                    if (eventHolder.GetType() == typeof(InboxEntryDeletedEvent))
                    {
                        InboxEntryDeletedEvent inboxEvent = (InboxEntryDeletedEvent)eventHolder;
                        InboxEntryDeletedEventData inboxEntry = inboxEvent.Data;

                        Assert.Multiple(() =>
                        {
                            Assert.That(inboxEntry.InboxId, Is.EqualTo(config.Read("inboxId", "Inbox_1")));
                            Assert.That(inboxEntry.EntryId, Is.EqualTo(config.Read("entryId", "Entry_1")));
                        });
                    }
                    else
                    {
                        Assert.Fail($"Event should be: InboxEntryDeletedEvent but was: {eventHolder.Type}");
                    }
                }
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }
        }

        [Test, Order(101), Description("Try a sequence of: WaitEvent_GetEvent_InboxEntryDeleted and check the returned values.")]
        public void WaitEvent_GetEvent_InboxEntryDeleted_Disabled()
        {
            Event privmxEvent = new Event();

            // pop libConnected form queue
            try
            {
                eventQueue.WaitEvent();
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Getting event failed.\nMessage: {e.Message}");
            }

            // subscribe and unsubscribe from events
            try
            {
                inboxApi.SubscribeForEntryEvents(config.Read("inboxId", "Inbox_1"));
                inboxApi.UnsubscribeFromEntryEvents(config.Read("inboxId", "Inbox_1"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Subscribing to events failed.\nMessage: {e.Message}");
            }

            // delete entry handle
            try
            {
                inboxApi.DeleteEntry(config.Read("entryId", "Entry_2"));
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"DeleteEntry failed.\nMessage: {e.Message}");
            }

            // check event and inbox values
            try
            {
                System.Threading.Thread.Sleep(1500);
                var eventHolder = eventQueue.GetEvent();

                if (eventHolder != null)
                {
                    Assert.Fail($"Expected null, but got: {eventHolder.Type}");
                }
            }
            catch (EndpointException e)
            {
                Console.WriteLine($"Getting event failed.\nMessage: {e.Message}");
            }
        }
    }
}