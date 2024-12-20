using EndpointCSharpTests.Utils;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Core.Models;

namespace EndpointCsharpTests
{
    internal class CoreTest : BaseTest
    {
        [Test, Description("PlatformConnect and disconnect for user_1")]
        public void SingleConnectionAndDisconnectionTest()
        {
            string userPrivKey = config.Read("user_1_privKey", "Login");
            string solutionId = config.Read("solutionId", "Login");
            bool didConnect = false;
            bool didDisconnect = false;

            try
            {
                Connection connection = Connection.Connect(userPrivKey, solutionId, address);
                didConnect = true;
                Console.WriteLine($"Connection instance Id: {connection.GetInstanceId()}");
                Disconnect(connection);
                didDisconnect = true;
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"ConnectionTest failed. Message: {e.Message}");
            }
            Assert.That((didConnect == true), Is.EqualTo(didDisconnect == true));
        }

        [Test, Description("PlatformConnect to user_1 when connected to user_1, then disconnect")]
        public void ConnectWhileUserAlreadyConnected_ThrowExpected()
        {
            string userPrivKey_usr1 = config.Read("user_1_privKey", "Login");
            string solutionId = config.Read("solutionId", "Login");

            bool didConnect2 = false;

            Connection connection1 = null;
            Connection connection2 = null;

            try
            {
                connection1 = Connection.Connect(userPrivKey_usr1, solutionId, address);
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Connection1 failed. Message: {e.Message}");
            }

            try
            {
                connection2 = Connection.Connect(userPrivKey_usr1, solutionId, address);
                didConnect2 = true;
            }
            catch(EndpointNativeException e)
            {
                Console.WriteLine($"Connection2 failed. Message: {e.Message}");
            }
            Assert.That(didConnect2, Is.False);

            Disconnect(connection1);
        }

        [Test, Description("PlatformConnect to user_2 when connected to user_1, then disconnect both")]
        public void ConnectMultipleUsers()
        {
            string userPrivKey_usr1 = config.Read("user_1_privKey", "Login");
            string userPrivKey_usr2 = config.Read("user_2_privKey", "Login");
            string solutionId = config.Read("solutionId", "Login");
            bool isConnected1 = false;
            bool isConnected2 = false;
            Connection connection1 = null;
            Connection connection2 = null;

            try
            {
                connection1 = Connection.Connect(userPrivKey_usr1, solutionId, address);
                isConnected1 = true;
                connection2 = Connection.Connect(userPrivKey_usr2, solutionId, address);
                isConnected2 = true;
            }
            catch (EndpointNativeException ex)
            {
                Assert.Fail($"Connection failed, message: {ex.Message}");
            }
            Assert.That((isConnected1 == true), Is.EqualTo(isConnected2 == true));

            Disconnect(connection1);
            Disconnect(connection2);
        }

        [Test, Description("Checks connection using listContexts for user_1, user_2, disconnects afterwards")]
        public void ListContextForMultipleUsers()
        {
            string userPrivKey_usr1 = config.Read("user_1_privKey", "Login");
            string userPrivKey_usr2 = config.Read("user_2_privKey", "Login");
            string solutionId = config.Read("solutionId", "Login");
            Connection connection1 = null;
            Connection connection2 = null;

            try
            {
                connection1 = Connection.Connect(userPrivKey_usr1, solutionId, address);
                connection2 = Connection.Connect(userPrivKey_usr2, solutionId, address);

                try
                {
                    PagingList<Context> listContextsUsr1 = connection1.ListContexts(
                        SetPagingQuery(0, 1, "desc"));
                    Assert.Multiple(() =>
                    {
                        Assert.That(listContextsUsr1.TotalAvailable, Is.EqualTo(2));
                        Assert.That(listContextsUsr1.ReadItems, Has.Count.EqualTo(1));
                    });
                    if (listContextsUsr1.ReadItems.Count >= 1)
                    {
                        Context context = listContextsUsr1.ReadItems[0];
                        Assert.That(context.ContextId, Is.EqualTo(config.Read("contextId", "Context_2")));
                    }

                    PagingList<Context> listContextsUsr2 = connection2.ListContexts(
                        SetPagingQuery(0, 1, "desc"));
                    Assert.Multiple(() =>
                    {
                        Assert.That(listContextsUsr2.TotalAvailable, Is.EqualTo(2));
                        Assert.That(listContextsUsr2.ReadItems, Has.Count.EqualTo(1));
                    });
                    if (listContextsUsr2.ReadItems.Count >= 1)
                    {
                        Context context = listContextsUsr2.ReadItems[0];
                        Assert.That(context.ContextId, Is.EqualTo(config.Read("contextId", "Context_2")));
                    }
                }
                catch (EndpointNativeException ex)
                {
                    Assert.Fail($"Listing contexts failed, message: {ex.Message}");
                }
            }
            catch (EndpointNativeException ex)
            {
                Assert.Fail($"User connection failed, message: {ex.Message}");
            }

            Disconnect(connection1);
            Disconnect(connection2);
        }

        [Test, Description("Checks connection using listContexts for user_1, user_2, then disconnects from user_1 and tries to listContexts again (throw). After that, gets the list again from (still connected) user_2.")]
        public void ListContextForMultipleUsers_OneUserDisconnected()
        {
            string userPrivKey_usr1 = config.Read("user_1_privKey", "Login");
            string userPrivKey_usr2 = config.Read("user_2_privKey", "Login");
            string solutionId = config.Read("solutionId", "Login");
            Connection connection1 = null;
            Connection connection2 = null;

            try
            {
                connection1 = Connection.Connect(userPrivKey_usr1, solutionId, address);
                connection2 = Connection.Connect(userPrivKey_usr2, solutionId, address);

                try
                {
                    Assert.Multiple(() =>
                    {
                        PagingList<Context> listContextsUsr1 = connection1.ListContexts(SetPagingQuery(0, 1, "desc"));
                        Assert.That(listContextsUsr1.TotalAvailable, Is.EqualTo(2));
                        Assert.That(listContextsUsr1.ReadItems, Has.Count.EqualTo(1));
                        if (listContextsUsr1.ReadItems.Count >= 1)
                        {
                            Context context = listContextsUsr1.ReadItems[0];
                            Assert.That(context.ContextId, Is.EqualTo(config.Read("contextId", "Context_2")));
                        }

                        PagingList<Context> listContextsUsr2 = connection2.ListContexts(SetPagingQuery(0, 1, "desc"));
                        Assert.That(listContextsUsr2.TotalAvailable, Is.EqualTo(2));
                        Assert.That(listContextsUsr2.ReadItems, Has.Count.EqualTo(1));
                        if (listContextsUsr2.ReadItems.Count >= 1)
                        {
                            Context context = listContextsUsr2.ReadItems[0];
                            Assert.That(context.ContextId, Is.EqualTo(config.Read("contextId", "Context_2")));
                        }
                    });
                }
                catch (EndpointNativeException ex)
                {
                    Assert.Fail($"Listing contexts failed, message: {ex.Message}");
                }

                Disconnect(connection1);
                bool didListContextUsr1 = false;
                bool didListContextUsr2 = false;

                try
                {
                    PagingList<Context> listContextsUsr1 = connection1.ListContexts(
                        SetPagingQuery(0, 1, "desc"));
                    didListContextUsr1 = true;
                }
                catch (EndpointNativeException ex)
                {
                    Console.WriteLine($"Listing contexts failed, message: {ex.Message}");
                }
                Assert.That(didListContextUsr1, Is.False);

                try
                {
                    PagingList<Context> listContextsUsr2 = connection2.ListContexts(
                        SetPagingQuery(0, 1, "desc"));
                    didListContextUsr2 = true;
                }
                catch (EndpointNativeException ex)
                {
                    Console.WriteLine($"Listing contexts failed, message: {ex.Message}");
                }
                Assert.That(didListContextUsr2, Is.True);

                Disconnect(connection2);
            }
            catch (EndpointNativeException ex)
            {
                Assert.Fail($"User connection failed, message: {ex.Message}");
            }
        }

        [Test, Description("PlatformConnect as quest and try to listContexts - expected access denied")]
        public void ConnectsAsPublicUser()
        {
            string solutionId = config.Read("solutionId", "Login");
            bool didListContext = false;

            try
            {
                Connection connection = Connection.ConnectPublic(solutionId, address);

                try
                {
                    PagingList<Context> listContexts = connection.ListContexts(
                        SetPagingQuery(0, 1, "desc"));
                    didListContext = true;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Listing context failed. Message: {e.Message}");
                }
                Assert.That(didListContext, Is.False);

                Disconnect(connection);
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Public connection failed. Message: {e.Message}");
            }
        }

        [Test, Description("ListContexts for incorrect input data - 4 expected throws - limit < 0, limit == 0, incorrect sortOrder, incorrect lastId")]
        public void ListContextsWithIncorrectInputData()
        {
            string userPrivKey_usr1 = config.Read("user_1_privKey", "Login");
            string solutionId = config.Read("solutionId", "Login");
            bool didListContext_LimitLessThan0 = false;
            bool didListContext_LimitEqual0 = false;
            bool didListContext_IncorrectSortOrder = false;
            bool didListContext_IncorrectLastId = false;

            try
            {
                Connection connection = Connection.Connect(userPrivKey_usr1, solutionId, address);
                // limit < 0
                try
                {
                    PagingList<Context> listContexts = connection.ListContexts(
                        SetPagingQuery(0, -1, "desc"));
                    didListContext_LimitLessThan0 = true;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Listing context with limit < 0. Message: {e.Message}");
                }
                Assert.That(didListContext_LimitLessThan0, Is.False);

                // limit == 0
                try
                {
                    PagingList<Context> listContexts = connection.ListContexts(
                        SetPagingQuery(0, 0, "desc"));
                    didListContext_LimitEqual0 = true;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Listing context with limit == 0. Message: {e.Message}");
                }
                Assert.That(didListContext_LimitEqual0, Is.False);

                // incorrect sortOrder
                try
                {
                    PagingList<Context> listContexts = connection.ListContexts(
                        SetPagingQuery(0, 1, "BLACH"));
                    didListContext_IncorrectSortOrder = true;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Listing context with incorrect sortOrder. Message: {e.Message}");
                }
                Assert.That(didListContext_IncorrectSortOrder, Is.False);

                // incorrect lastId
                try
                {
                    PagingList<Context> listContexts = connection.ListContexts(
                        SetPagingQuery(0, 1, "desc", config.Read("threadId", "Thread_1")));
                    didListContext_IncorrectLastId = true;
                }
                catch (EndpointNativeException e)
                {
                    Console.WriteLine($"Listing context with incorrect lastId. Message: {e.Message}");
                }
                Assert.That(didListContext_IncorrectLastId, Is.False);

                Disconnect(connection);
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Connection failed. Message: {e.Message}");
            }
        }

        [Test, Description("ListContexts for correct input data - 3 different tries")]
        public void ListContextsWithCorrectData()
        {
            string userPrivKey_usr1 = config.Read("user_1_privKey", "Login");
            string solutionId = config.Read("solutionId", "Login");
            bool didListContext_Try1 = false;
            bool didListContext_Try2 = false;
            bool didListContext_Try3 = false;

            try
            {
                Connection connection = Connection.Connect(userPrivKey_usr1, solutionId, address);

                // {.skip=3, .limit=1, .sortOrder="desc"}
                try
                {
                    PagingList<Context> listContexts = connection.ListContexts(
                        SetPagingQuery(3, 1, "desc"));
                    Assert.Multiple(() =>
                    {
                        Assert.That(listContexts.TotalAvailable, Is.EqualTo(2));
                        Assert.That(listContexts.ReadItems, Has.Count.EqualTo(0));
                    });
                    didListContext_Try1 = true;
                }
                catch (EndpointNativeException e)
                {
                   Assert.Fail($"Listing context failed. Try 1. Message: {e.Message}");
                }
                Assert.That(didListContext_Try1 , Is.True);

                // {.skip=1, .limit=1, .sortOrder="desc"}
                try
                {
                    PagingList<Context> listContexts = connection.ListContexts(
                        SetPagingQuery(1, 1, "desc"));
                    Assert.Multiple(() =>
                    {
                        Assert.That(listContexts.TotalAvailable, Is.EqualTo(2));
                        Assert.That(listContexts.ReadItems, Has.Count.EqualTo(1));
                    });
                    if(listContexts.ReadItems.Count >= 1)
                    {
                        Context context = listContexts.ReadItems[0];
                        Assert.That(context.ContextId, Is.EqualTo(config.Read("contextId","Context_1")));
                    }
                    didListContext_Try2 = true;
                }
                catch (EndpointNativeException e)
                {
                    Assert.Fail($"Listing context failed. Try 2. Message: {e.Message}");
                }
                Assert.That(didListContext_Try2, Is.True);

                // {.skip=0, .limit=3, .sortOrder="asc"}
                try
                {
                    PagingList<Context> listContexts = connection.ListContexts(
                        SetPagingQuery(0, 3, "asc"));
                    Assert.Multiple(() =>
                    {
                        Assert.That(listContexts.TotalAvailable, Is.EqualTo(2));
                        Assert.That(listContexts.ReadItems, Has.Count.EqualTo(2));
                    });
                    if (listContexts.ReadItems.Count >= 1)
                    {
                        Context context = listContexts.ReadItems[0];
                        Assert.That(context.ContextId, Is.EqualTo(config.Read("contextId", "Context_1")));
                    }
                    if (listContexts.ReadItems.Count >= 2)
                    {
                        Context context = listContexts.ReadItems[1];
                        Assert.That(context.ContextId, Is.EqualTo(config.Read("contextId", "Context_2")));
                    }
                    didListContext_Try3 = true;
                }
                catch (EndpointNativeException e)
                {
                    Assert.Fail($"Listing context failed. Try 3. Message: {e.Message}");
                }
                Assert.That(didListContext_Try3, Is.True);

                Disconnect(connection);
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Public connection failed. Message: {e.Message}");
            }
        }

        private static void Disconnect(Connection? connection)
        {
            if (connection != null)
            {
                try
                {
                    connection.Disconnect();
                    connection = null;
                }
                catch (EndpointNativeException e)
                { 
                    Assert.Fail($"Disconnect failed. Message: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Disconnect failed. Connection was null");
            }
        }
    }
}