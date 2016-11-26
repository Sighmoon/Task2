using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Task2
{
    public class Program
    {
        static void Main(string[] args)
        {
             XmlSerializer serializer = new XmlSerializer(typeof(data));
             string address = RequestApi.AddressForm();
             IEnumerable<string> idList = RequestApi.RequestIdList(address);
             foreach (string id in idList)
             {
                 address = RequestApi.AddressForm(id);
                 using (XmlReader reader = RequestApi.HttpRequest(address))
                 {
                     data data = dataDeserialize(reader, serializer);
                     Console.WriteLine(data._embedded.Purchase.Id);
                 }                    
                 //dataSerialize(data);
             }
            Console.ReadLine();

        }

        /// <summary>
        /// Метод, производящий десериализацию xml документа.
        /// </summary>
        /// <param name="reader">Предоставляет доступ к xml-документу</param>
        /// <param name="serializer"></param>
        /// <returns>Возвращает объект класса data</returns>

        static public data dataDeserialize(XmlReader reader, XmlSerializer serializer)
        {
            data data = (data)serializer.Deserialize(reader);
            reader.Close();
            return data;
        }

        /// <summary>
        /// Метод, производящий сериализацию объекта data. Данные сохраняются в xml файле, где именем файла служит его Id.
        /// </summary>
        /// <param name="data">Объект для сериализации</param>
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
