using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Task2
{
    [Serializable()]
    [System.Xml.Serialization.XmlRoot(elementName:"data")]
    public class data 
    {
        public data()
        { }

        [XmlElement("_embedded")]
        public _embedded _embedded { get; set; }

        [XmlElement("_links")]
        public _links _links { get; set; }

        static public data dataDeserialize(string path)
        {
            data data = null;
            XmlSerializer serializer = new XmlSerializer(typeof(data));
            StreamReader reader = new StreamReader(path);
            data  = (data)serializer.Deserialize(reader);
            reader.Close();
            return data;
        }

        public void dataSerialize(string path)
        {
            data data = this;

            XmlSerializer serializer = new XmlSerializer(typeof(data));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, data);
                Console.WriteLine("Объект сериализован");
            }
        }

    }

    [Serializable()]
    public class _embedded
    {
        public _embedded()
        { }

        [XmlElement("Purchase")]
        public Purchase Purchase { get; set; }
    }

    [Serializable()]
    public class _links
    {
        public _links()
        { }

        [XmlElement("self")]
        public self self { get; set; }

    }

    public class self
    {
        public self()
        { }

        [XmlElement("href")]
        public string href { get; set; }

    }

    [Serializable()]
    public class Purchase 
    {
        public Purchase()
        { }


        [XmlElement("id")]
        public string id { get; set; }

        [XmlElement("name")]
        public string name { get; set; }

        [XmlElement("status")]
        public string status { get; set; }

        [XmlElement("statusId")]
        public string statusId { get; set; }

        [XmlElement("typeTorgs")]
        public string typeTorgs { get; set; }

        [XmlElement("publicDate")]
        public string publicDate { get; set; }

        [XmlElement("endPublicationDate")]
        public string endPublicationDate { get; set; }

        [XmlElement("dateUpdate")]
        public string dateUpdate { get; set; }

        [XmlElement("requestVersions")]
        public int requestVersions { get; set; }

        [XmlElement("organization")]
        public organizationStruct organization { get; set; }

        [XmlArray("lots")]
        [XmlArrayItem("lot", typeof(lot))]
        public lot[] lot { get; set; }

    }

    [Serializable()]
    public struct organizationStruct
    {

        [XmlElement("shortName")]
        public string shortName { get; set; }

        [XmlElement("inn")]
        public string inn { get; set; }

        [XmlElement("kpp")]
        public string kpp { get; set; }

        [XmlElement("postAddress")]
        public string postAddress { get; set; }

        [XmlElement("legalAddress")]
        public string legalAddress { get; set; }
    }

    [Serializable]
    public class lot
    {
        [XmlElement("agreementSubject")]
        public string agreementSubject { get; set; }

        [XmlElement("lotPrice")]
        public decimal lotPrice { get; set; }

        [XmlElement("unitAmount")]
        public string unitAmount { get; set; }

        [XmlElement("nds")]
        public ndsStruct nds { get; set; }

        [XmlElement("currency")]
        public currencyStruct currency { get; set; }

        [XmlElement("maxUnitPrice")]
        public string maxUnitPrice { get; set; }

        [XmlElement("offerPriceType")]
        public offerPriceTypeStruct offerPriceType { get; set; }

        [XmlElement("delivBasis")]
        public string delivBasis { get; set; }

        [XmlElement("condSupply")]
        public string condSupply { get; set; }

        [XmlElement("requirements")]
        public string requirements { get; set; }

    }

    [Serializable]
    public struct ndsStruct
    {
        [XmlElement("digitalCode")]
        public int digitalCode { get; set; }

        [XmlElement("name")]
        public string name { get; set; }

    }

    [Serializable]
    public struct currencyStruct
    {
        [XmlElement("digitalCode")]
        public int digitalCode { get; set; }

        [XmlElement("code")]
        public string code { get; set; }

    }

    [Serializable]
    public struct offerPriceTypeStruct
    {
        [XmlElement("code")]
        public int code { get; set; }

        [XmlElement("name")]
        public string name { get; set; }
    }
}
