using EndpointCsharpTests;
using PrivMX.Endpoint.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EndpointCSharpTests.Utils
{
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
    }
}
