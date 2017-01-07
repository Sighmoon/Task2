using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;

namespace Task2
{
    static public class RequestApi
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
        private const string RequestTmp = "http://api.federal1.ru/api/registry";

        /// <summary>
        /// Метод, отвечающий за http запрос.
        /// </summary>
        /// <param name="address">Принимает адрес, к которому необходимо обратиться</param>
        /// <returns>Возвращает xmlreader</returns>
        static public XmlDocument HttpRequest(string address) // запрос к апи
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(address);
            request.Accept = "*/*";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            XmlReader reader = XmlReader.Create(stream);
            XmlDocument xDoc = new XmlDocument();
            
            xDoc.Load(reader);
            return xDoc;
        }

        /// <summary>
        /// Метод, собирающий Id.
        /// </summary>
        /// <param name="address">Адрес документа, из которого нужно вытащить все id</param>
        /// <returns>Возвращает список id</returns>
        static public IEnumerable<string> RequestIdList(string address)
        {
             XmlDocument xDoc = HttpRequest(address);
                IEnumerable<string> idList = from xEl in xDoc.SelectSingleNode("data").SelectSingleNode("_embedded").ChildNodes.Cast<XmlNode>()
                                         select xEl.SelectSingleNode("id").InnerText;
            return idList;
        }

        /// <summary>
        /// Метод, отвечающий за формирование запроса к общему документу
        /// </summary>
        /// <returns>Возвращает адрес</returns>
        static public string AddressForm() 
        {
            string endDateTime = DateTime.Now.CurrentTime();
            return $"{RequestTmp}?pageSize={PageSize}&startDateTime={StartDateTime}&endDateTime{endDateTime}";
        }

        /// <summary>
        /// Метод, отвечающий за формирование запроса к отдельному теднеру
        /// </summary>
        /// <param name="id">Принимает id тендера, адрес которого необходимо получить</param>
        /// <returns>Возвращает адрес</returns>
        static public string AddressForm(this string id) =>  $"{RequestTmp}/{id}";
        /// <summary>
        /// Метод, приобразующий DateTime в строковый вид
        /// </summary>
        /// <param name="date">Принимает время в виде DateTime</param>
        /// <returns>Возвращает строку типа yyyyMMddhhmmss</returns>
        static public string CurrentTime(this DateTime date) => string.Format("{0:yyyy''MM''dd''hh''mm''ss}", date);

    }
}
