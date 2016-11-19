using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Task2
{
    public class Program
    {
        static void Main(string[] args)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(data));
            string address;
            XmlReader reader;
            address = RequestApi.AddressForm();
            data data;
            IEnumerable<string> idList = RequestApi.RequestIdList(address);
            foreach (string id in idList)
            {
                address = RequestApi.AddressForm(id);
                reader = RequestApi.HttpRequest(address);
                data = dataDeserialize(reader, serializer);
                Console.WriteLine(data._embedded.Purchase.Id);
                //dataSerialize(data);
                reader.Close();
            }
            Console.ReadLine();

        }

        static public data dataDeserialize(XmlReader reader, XmlSerializer serializer)
        {
            data data = null;
            data = (data)serializer.Deserialize(reader);
            reader.Close();
            return data;
        }


        static public void dataSerialize(data data)
        {
            string path = data._embedded.Purchase.Id + ".xml";
            XmlSerializer serializer = new XmlSerializer(typeof(data));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, data);
                Console.WriteLine("Объект " + data._embedded.Purchase.Id + " сериализован");
            }
        }
    }

}
