using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using System.IO;
using MongoDB.Bson;
using System.Security.Cryptography;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace IndexTenderXml
{
    class Program
    {
        static XmlSerializer serializer = new XmlSerializer(typeof(data));
        static Repository DB = new Repository(ConfigurationManager.AppSettings.Get("RepositoryName"),
            ConfigurationManager.ConnectionStrings["Task2ConStr"].ConnectionString);

        static void Main(string[] args)
        {
            var connectionParams = new ConnectionFactory
            { HostName = ConfigurationManager.AppSettings.Get("RabbitMQServerIP")};
            using (var connection = connectionParams.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("Xml Queue", true, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += OnReceivedMessage;
                    channel.BasicConsume("Xml Queue", true, consumer);
                }
            }
            Console.ReadKey();
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

        /// <summary>
        /// Метод, обновляющий значение хеша объекта data, принимающий xml-строку тендера
        /// </summary>
        /// <param name="data">Тендер, чей хещ необходимо обновить</param>
        /// <param name="inputString">xml-строка документа тендера</param>
        static public void SetHashString(ref data data, string inputString)
        {
            StringBuilder sb = new StringBuilder();
            HashAlgorithm algorithm = MD5.Create();
            byte[] byteHash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            foreach (byte b in byteHash)
                sb.Append(b.ToString("X2"));
            data.Hash = sb.ToString();
        }

        /// <summary>
        /// Реализация ивента, срабатывающего при получение сообщения с сервера RabbitMQ.
        /// </summary>

        static void OnReceivedMessage(object sender, BasicDeliverEventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            var body = e.Body;
            string XmlString = Encoding.UTF8.GetString(body);
            Console.WriteLine("Пришло: " + XmlString);
            xDoc.LoadXml(XmlString);
            data data = dataDeserialize(xDoc.OuterXml, serializer);
            data._id = data._embedded.Purchase.Id;
            DB.AddToDatabase(data);

        }

    }
}

