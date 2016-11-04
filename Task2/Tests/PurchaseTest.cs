using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2;

namespace Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class PurchaseTest
    {
        [Test]
        public void XmlDes()
        {
            data data, postData;

            data = data.dataDeserialize("Z-316.xml");
            data.dataSerialize("Z-316(1).xml");
            postData = data.dataDeserialize("Z-316(1).xml");

            Assert.AreEqual(data, postData);
        }
    }
}
