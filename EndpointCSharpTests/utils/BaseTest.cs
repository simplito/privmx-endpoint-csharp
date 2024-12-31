using EndpointCSharpTests.utils;
using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EndpointCSharpTests.Utils
{
    public enum ConnectionType
    {
        User1 = 0,
        User2 = 1,
        Public = 2
    }

    internal class BaseTest
    {
        private string iniFilePath = Directory.GetCurrentDirectory() + @"\..\..\..\tests-ini-files\ServerData.ini";
        private string addressFilePath = Directory.GetCurrentDirectory() + @"\..\..\..\tests-ini-files\TestAddress.ini";
        public string address = @"";
        public IniFileConfig config = null;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern bool SetDllDirectory(string lpPathName);

        [OneTimeSetUp]
        public virtual void OneTimeSetup()
        {
            string dllPath = Directory.GetCurrentDirectory() + @"\libs";
            SetDllDirectory(dllPath);

            try
            {
                if (!File.Exists(iniFilePath))
                {
                    Console.WriteLine("Ini file not found");
                    return;
                }
                config = new IniFileConfig(iniFilePath);

                if (!File.Exists(addressFilePath))
                {
                    Console.WriteLine("Address ini file not found");
                    return;
                }
                address = File.ReadAllText(addressFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Setup failed. Message: {e.Message}");
            }
        }

        protected static PagingQuery SetPagingQuery(int skip, int limit, string sortOrder, string lastId = "")
        {
            PagingQuery query = new PagingQuery
            {
                Skip = skip,
                Limit = limit,
                SortOrder = sortOrder
            };
            if (lastId != "") query.LastId = lastId;

            return query;
        }

        protected static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "").ToLower();
        }

        protected static long StringToInt64(string s)
        {
            return (long)Convert.ToDouble(s);
        }

        protected void ConnectAs(ref Connection connection,  ConnectionType type)
        {
            string userPrivKey_usr1 = config.Read("user_1_privKey", "Login");
            string userPrivKey_usr2 = config.Read("user_2_privKey", "Login");
            string solutionId = config.Read("solutionId", "Login");

            try
            {
                switch(type)
                {
                    case ConnectionType.User1:
                        connection = Connection.Connect(userPrivKey_usr1, solutionId, address);
                        break;
                    case ConnectionType.User2:
                        connection = Connection.Connect(userPrivKey_usr2, solutionId, address);
                        break;
                    case ConnectionType.Public:
                        connection = Connection.ConnectPublic(solutionId, address);
                        break;
                }
                
            }
            catch (EndpointNativeException e)
            {
                Assert.Fail($"Connect failed.\nMessage: {e.Message}");
            }
        }

        protected void Disconnect(ref Connection connection)
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
                    Assert.Fail($"Disconnect failed.\nMessage: {e.Message}");
                }
            }
            else
            {
                Assert.Fail($"Disconnect failed.\nConnection was null");
            }
        }
    }
}
