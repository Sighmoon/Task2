using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using QueueFunc;
using RabbitMQ.Client;


namespace QueueFunc.Tests
{

    [TestFixture]
    public class QueueFuncTest
    {
        QueueObj Qobj;
        int count=0;

        [SetUp]
        public void DataInit()
        {
            QueueParams param = new QueueParams("localhost", "Test-Queue");
            Qobj = QueueObj.GetQueueObj(param);
        }

        [Test]
        public void PublishOneShouldWork()
        {
            Qobj.channel.QueuePurge("Test-Queue");
            Qobj.PublishMessage<string>("Test");
            string message = Qobj.ConsumeOneMessage();
            Assert.AreEqual("Test", message);
        }

        [Test]
        public void PublishGroupOfMessage()
        {
            Qobj.channel.QueuePurge("Test-Queue");
            string[] messages = { "Hello", "World!", "This", "is", "the", "test"};
            Qobj.PublishGroupOfMessages(messages);
            Qobj.ConsumeMessages<string>(SomeMethod, false);
            Qobj.channel.Close();
            Assert.AreEqual(messages.Count(), count);
        }
        
        [Test]
        public void ConsumeInEmptyQueue()
        {
            Qobj.channel.QueuePurge("Test-Queue");
            string message = Qobj.ConsumeOneMessage();
            Assert.Null(message);
        }

        [TearDown]
        public void DataDispose()
        {

        }

        public void SomeMethod(string message)
        {
            if (message != null)
            {
                count++;
            }
        }
    }

}

