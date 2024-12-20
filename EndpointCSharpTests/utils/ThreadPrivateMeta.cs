using PrivMX.Endpoint.Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EndpointCSharpTests.Utils
{
    internal class ThreadPrivateMeta
    {
        private string storeId;
        private string name;

        public ThreadPrivateMeta(string storeId, string name)
        {
            this.storeId = storeId;
            this.name = name;
        }

        public string StoreId => storeId;

        public string Name => name;
    }
}
