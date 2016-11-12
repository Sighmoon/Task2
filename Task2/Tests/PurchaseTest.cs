using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2;


namespace NUnit.Tests
{
    using System;
    using NUnit.Framework;
    using Task2;
    using System.Xml.Serialization;
    using System.Xml;

    [TestFixture]
    public class SuccessTests
    {
        XmlSerializer serializer;
        XmlReader reader;
        string address;
        data data;

        [SetUp]
        public void Init()
        {
            serializer = new XmlSerializer(typeof(data));
            address = RequestApi.AddressForm("Q-330");
            reader = RequestApi.HttpRequest(address);
            data = Program.dataDeserialize(reader, serializer);
        }

        [Test]
        public void Add()
        {
            data = Program.dataDeserialize(reader, serializer);
            TestActionAttribute.Equals("Q-330", data._embedded.Purchase.Id);
            // ...
        }

        [TearDown]
        public void Dispose()
        {
            
        }
    }
}