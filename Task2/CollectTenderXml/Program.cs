using System;
using System.Collections.Generic;
using System.Xml;
using RabbitMQ.Client;
using System.Configuration;
using System.Text;
using QueueFunc;

namespace CollectTenderXml
{
    class Program
    {
        static void Main(string[] args)
        {
            QueueParams param = new QueueParams(ConfigurationManager.AppSettings.Get("RabbitMQServerIP"), "Xml-Queue");
            QueueObj Qobj = QueueObj.GetQueueObj(param);

            string address = RequestApi.AddressForm(1);
            int pageAmount = RequestApi.GetPageAmount(address);
            for(int i = 1; i <= pageAmount; i++)
            {

                IEnumerable<string> idList = RequestApi.RequestIdList(address);

                foreach(string id in idList)
                {
                    address = id.AddressForm();
                    XmlDocument xDoc = RequestApi.HttpRequest(address);
                    string xmlString = xDoc.OuterXml;
                    Qobj.PublishMessage<string>(xmlString);
                }
                Console.WriteLine($"Выполнена загрузка документов со страницы {i}");
                address = RequestApi.AddressForm(i+1);
            }
            Console.ReadKey();
        }
    }
}
