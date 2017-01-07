using System;
using System.Collections.Generic;
using System.Xml;
using RabbitMQ.Client;
using System.Configuration;
using System.Text;

namespace CollectTenderXml
{
    class Program
    {
        static void Main(string[] args)
        {
            string address = RequestApi.AddressForm();
            IEnumerable<string> idList = RequestApi.RequestIdList(address);

            var connectionParams = new ConnectionFactory
            { HostName = ConfigurationManager.AppSettings.Get("RabbitMQServerIP") };
            using (var connection = connectionParams.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("Xml Queue", true, false, false, null);
                    int i = 0;
                    foreach (string id in idList)
                    {
                        if(i<3)
                        {
                            address = id.AddressForm();
                            XmlDocument xDoc = RequestApi.HttpRequest(address);
                            string xmlString = xDoc.OuterXml;
                            var body = Encoding.UTF8.GetBytes(xmlString);
                            channel.BasicPublish("", "Xml Queue", null, body);
                        }
                        i++;
                    }
                    Console.ReadKey();
                }
            }
        }
    }
}
