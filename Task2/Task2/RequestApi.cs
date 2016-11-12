using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;


namespace Task2
{
    public class RequestApi
    {
        private const string PageSize = "2000";
        private const string StartDateTime = "0";


        static private string CurrentTime() //текущее время возвращается в формате YYYYMMDDhhmmss 
        {
            string timestr = "";
            DateTime date = DateTime.Now;
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

        static public List<string> RequestIdList(string address)
        {
            List<string> idList = new List<string>();
            XmlReader reader = HttpRequest(address);
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(reader);
            foreach (XmlNode node in xDoc.DocumentElement.SelectNodes("//data/_embedded/Purchase"))
            {
                idList.Add(node.FirstChild.InnerText);
            }
            return idList;
        }

        static public string AddressForm() // формирование адреса большого документа
        {
            string endDateTime = CurrentTime();
            string[] pattern = { "http://api.federal1.ru/api/registry?", "&pageSize=", "&startDateTime=", "&endDateTime=" };
            return pattern[0] + pattern[1] + PageSize.ToString() + pattern[2] + StartDateTime + pattern[3] + endDateTime;
        }

        static public string AddressForm(string id) // формирование адреса документа отдельного торга
        {
            const string pattern = "http://api.federal1.ru/api/registry/";
            return pattern + id; ;
        }

    }
}
