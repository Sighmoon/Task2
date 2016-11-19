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
       
        static RequestApi()
        {
            PageSize = ConfigurationManager.AppSettings.Get("PageSize");
            StartDateTime = ConfigurationManager.AppSettings.Get("StartDateTime");
        }

        static private string PageSize;
        static private string StartDateTime;

        static public string CurrentTime(DateTime date) //текущее время возвращается в формате YYYYMMDDhhmmss 
        {
            string timestr = "";
            timestr = string.Format("{0:yyyy''MM''dd''hh''mm''ss}", date);
            return timestr;
        }

        static public XmlReader HttpRequest(string address) // запрос к апи
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(address);
            request.Accept = "*/*";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            XmlReader reader = XmlReader.Create(stream);
            return reader;
        }

        static public IEnumerable<string> RequestIdList(string address)
        {
            XmlReader reader = HttpRequest(address);
            XDocument xDoc = XDocument.Load(reader);
            IEnumerable<string> idList = from xEl in xDoc.Element("data").Element("_embedded").Elements("Purchase")
                                         select xEl.Element("id").Value;
            return idList;
        }

        static public string AddressForm() // формирование адреса большого документа
        {
            string endDateTime = CurrentTime(DateTime.Now);
            return $"http://api.federal1.ru/api/registry?pageSize={PageSize}&startDateTime={StartDateTime}&endDateTime{endDateTime}";
        }

        static public string AddressForm(string id) // формирование адреса документа отдельного торга
        {
            return $"http://api.federal1.ru/api/registry/{id}";
        }

    }
}
