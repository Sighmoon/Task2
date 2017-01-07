using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using MongoDB.Bson;
using System.Configuration;

namespace Task2
{
    public class Program
    {
        static void Main(string[] args)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(data));
            string address = RequestApi.AddressForm();
            IEnumerable<string> idList = RequestApi.RequestIdList(address);
            Repository DB = new Repository(ConfigurationManager.AppSettings.Get("RepositoryName"),
                ConfigurationManager.ConnectionStrings["Task2ConStr"].ConnectionString);

            foreach (string id in idList)
            {
                address = id.AddressForm();

                XmlDocument xDoc = RequestApi.HttpRequest(address);
                data data = dataDeserialize(xDoc.OuterXml, serializer);
                Console.WriteLine(data.Hash+" "+ data._embedded.Purchase.Id);
                data._id = data._embedded.Purchase.Id;
                DB.AddToDatabase(data);
            }
            Console.ReadLine();

        }

        /// <summary>
        /// Метод, производящий десериализацию xml документа.
        /// </summary>
        /// <param name="reader">Предоставляет доступ к xml-документу</param>
        /// <param name="serializer"></param>
        /// <returns>Возвращает объект класса data</returns>

        static public data dataDeserialize(string xml, XmlSerializer serializer)
        {
            data data = null;
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                data = (data)serializer.Deserialize(reader);
                SetHashString(ref data, xml);
                data._id = new ObjectId(data.Hash).ToString();
                reader.Close();
            }
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
