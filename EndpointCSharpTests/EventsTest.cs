using EndpointCSharpTests.Utils;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Inbox;
using PrivMX.Endpoint.Store;
using PrivMX.Endpoint.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventQueue = PrivMX.Endpoint.Core.EventQueue;

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
            CustomDisconnect(ref connection);
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
    }
}
