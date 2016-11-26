using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Xml.Linq;

namespace Task2
{
    public class RequestApi
    {
       /// <summary>
       /// Статический конструктор RequestApi, определяющий поля PageSize и StartDateTime
       /// </summary>
        static RequestApi()
        {
            PageSize = ConfigurationManager.AppSettings.Get("PageSize");
            StartDateTime = ConfigurationManager.AppSettings.Get("StartDateTime");
        }

        static private string PageSize;
        static private string StartDateTime;

        /// <summary>
        /// Метод, отвечающий за http запрос.
        /// </summary>
        /// <param name="address">Принимает адрес, к которому необходимо обратиться</param>
        /// <returns>Возвращает xmlreader</returns>
        static public XmlReader HttpRequest(string address) // запрос к апи
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(address);
            request.Accept = "*/*";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            XmlReader reader = XmlReader.Create(stream);
            return reader;
        }

        /// <summary>
        /// Метод, собирающий Id.
        /// </summary>
        /// <param name="address">Адрес документа, из которого нужно вытащить все id</param>
        /// <returns>Возвращает список id</returns>
        static public IEnumerable<string> RequestIdList(string address)
        {
            XmlReader reader = HttpRequest(address);
            XDocument xDoc = XDocument.Load(reader);
            IEnumerable<string> idList = from xEl in xDoc.Element("data").Element("_embedded").Elements("Purchase")
                                         select xEl.Element("id").Value;
            return idList;
        }

        /// <summary>
        /// Метод, отвечающий за формирование запроса к общему документу
        /// </summary>
        /// <returns>Возвращает адрес</returns>
        static public string AddressForm() 
        {
            string endDateTime = CurrentTime(DateTime.Now);
            return "http://api.federal1.ru/api/registry?"+
                $"pageSize={PageSize}&startDateTime={StartDateTime}&endDateTime{endDateTime}";
        }

        /// <summary>
        /// Метод, отвечающий за формирование запроса к отдельному теднеру
        /// </summary>
        /// <param name="id">Принимает id тендера, адрес которого необходимо получить</param>
        /// <returns>Возвращает адрес</returns>
        static public string AddressForm(string id) => $"http://api.federal1.ru/api/registry/{id}";
        /// <summary>
        /// Метод, приобразующий DateTime в строковый вид
        /// </summary>
        /// <param name="date">Принимает время в виде DateTime</param>
        /// <returns>Возвращает строку типа yyyyMMddhhmmss</returns>
        static public string CurrentTime(DateTime date) => string.Format("{0:yyyy''MM''dd''hh''mm''ss}", date);

    }
}
