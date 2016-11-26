using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2;
using NUnit.Framework;
using System.Xml.Serialization;
using System.Xml;
using MongoDB.Bson;
using MongoDB.Driver;

namespace NUnit.Tests
{

    [TestFixture]
    public class DeserializerTests
    {
        XmlSerializer serializer;

        [SetUp]
        public void DataInit()
        {
            serializer = new XmlSerializer(typeof(data));
        }

        [Test]
        public void PropertyShoudlBeCorrect()
        {
            string address = RequestApi.AddressForm("Q-330");
            XmlReader reader = RequestApi.HttpRequest(address);
            data data = Program.dataDeserialize(reader, serializer);
            TestActionAttribute.Equals("Q-330", data._embedded.Purchase.Id);
        }
        
        public void DataIsNotNull()
        {
            string address = RequestApi.AddressForm("Q-330");
            XmlReader reader = RequestApi.HttpRequest(address);
            data data = Program.dataDeserialize(reader, serializer);
            Assert.IsNotNull(data);
        }

        [Test]
        public void CurrentTimeShouldBeCorrect()
        {
            string currentDate = RequestApi.CurrentTime(new DateTime(2016,11,19,20,15,35));
            Assert.AreEqual("20161119201535", currentDate);
        }

        [Test]
        public void SevenShouldCountAsZeroSeven()
        {
            string currentDate = RequestApi.CurrentTime(new DateTime(2007, 7, 7, 7, 7, 7));
            Assert.AreEqual("20070707070707", currentDate);
        }

        [Test]
        public void CurrentDateLengthShoudlBeEqualFourteen()
        {
            string currentDate = RequestApi.CurrentTime(new DateTime(7, 7, 7, 7, 7, 7));
            Assert.AreEqual(14,currentDate.Length);
        }

        [Test]
        public void AddressFormShouldBeCorrect()
        {
            string address = RequestApi.AddressForm("Q-201");
            Assert.AreEqual("http://api.federal1.ru/api/registry/Q-201", address);
        }

        [Test]
        public void HttpRequestShouldBeSucces()
        {
            string address = RequestApi.AddressForm();
            XmlReader reader = RequestApi.HttpRequest(address);
            Assert.IsNotNull(reader);
        }
        

        [TearDown]
        public void DataDispose()
        {
            
        }
    }

    [TestFixture]
    public class RepositoryTest
    {
        IMongoDatabase database = Repository.GetDatabase("Task2");

        [SetUp]
        public void DatabaseFill()
        {
            Repository.AddToDatabase(new MiniTender("Q-123", 1, "26.11.2016"), database);
            Repository.AddToDatabase(new MiniTender("A-321", 2, "01.01.2016"), database);
            Repository.AddToDatabase(new MiniTender("T-111", 3, "03.05.2016"), database);
        }

        [Test]
        public void IsDocExist()
        {
            Assert.IsTrue(Repository.IsElementExist("Q-123", database));
        }

        [Test]
        public void IsDocDeleted()
        {
            Repository.RemoveFromDatabase("A-321", database);
            Assert.IsFalse((Repository.IsElementExist("Q-321", database)));
        }

        [TearDown]
        public void ClearDB()
        {
            Repository.RemoveFromDatabase("Q-123", database);
            Repository.RemoveFromDatabase("T-111", database);
        }
    }
    
}