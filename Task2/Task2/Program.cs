using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using MongoDB.Bson;

namespace Task2
{
    public class Program
    {
        static void Main(string[] args)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(data));
            string address = RequestApi.AddressForm();
            IEnumerable<string> idList = RequestApi.RequestIdList(address);
            Repository DB = new Repository("Task2", "mongodb://localhost/test");

            foreach (string id in idList)
            {
                address = id.AddressForm();
                using (XmlReader reader = RequestApi.HttpRequest(address))
                {
                    data data = dataDeserialize(reader, serializer);
                    Console.WriteLine(data.Hash+" "+ data._embedded.Purchase.Id);
                    DB.AddToDatabase(data);
                }
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
            SetHashString(ref data, reader.ReadOuterXml());
            data._id = new ObjectId(data.Hash).ToString();
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

        static public void SetHashString(ref data data, string inputString)
        {
            StringBuilder sb = new StringBuilder();
            HashAlgorithm algorithm = MD5.Create();
            byte[] byteHash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            foreach (byte b in byteHash)
                sb.Append(b.ToString("X2"));
            data.Hash = sb.ToString();
        }
    }

}
