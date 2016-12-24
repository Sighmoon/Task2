using System;
using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Task2
{
    /// <summary>
    /// Класс отдельного тендера
    /// </summary>
    [Serializable()]
    [System.Xml.Serialization.XmlRoot(elementName:"data")]
    public class data 
    {
        /// <summary>
        /// Пустой конструктор, необходимый для сериализации и десериализации.
        /// </summary>
        public data()
        { }
        /// <summary>
        /// Свойство _embedded, содержит ноду с основой информации о тендере
        /// </summary>
        [XmlElement("_embedded")]
        public _embedded _embedded { get; set; }

        /// <summary>
        /// Свойство _links, сордержит ссылку на сам тендер
        /// </summary>
        [XmlElement("_links")]
        public _links _links { get; set; }
        [BsonId]
        public string _id { get; set; }
        public string Hash { get; set; }




    }

    /// <summary>
    /// Класс ноды тендера, который содержит ноду Purchase
    /// </summary>
    [Serializable()]
    public class _embedded
    {
        public _embedded()
        { }

        /// <summary>
        /// Свойство Purchase содержит всю информацию о тендере
        /// </summary>
        [XmlElement("Purchase")]
        public Purchase Purchase { get; set; }
    }

    /// <summary>
    /// Класс ноды _links, содержищий ноду со ссылкой на сам тендер
    /// </summary>
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

    /// <summary>
    /// Класс Purchase содержит всю информацию о тендере. 
    /// </summary>
    [Serializable()]
    public class Purchase 
    {
        public Purchase()
        { }

        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("statusId")]
        public string StatusId { get; set; }

        [XmlElement("typeTorgs")]
        public string TypeTorgs { get; set; }

        [XmlElement("publicDate")]
        public string PublicDate { get; set; }

        [XmlElement("endPublicationDate")]
        public string EndPublicationDate { get; set; }

        [XmlElement("dateUpdate")]
        public string DateUpdate { get; set; }

        [XmlElement("requestVersions")]
        public int RequestVersions { get; set; }

        [XmlElement("organization")]
        public organizationStruct Organization { get; set; }

        [XmlArray("lots")]
        [XmlArrayItem("lot", typeof(Lot))]
        public Lot[] Lot { get; set; }

    }

    [Serializable()]
    public struct organizationStruct
    {

        [XmlElement("shortName")]
        public string ShortName { get; set; }

        [XmlElement("inn")]
        public string Inn { get; set; }

        [XmlElement("kpp")]
        public string Kpp { get; set; }

        [XmlElement("postAddress")]
        public string PostAddress { get; set; }

        [XmlElement("legalAddress")]
        public string LegalAddress { get; set; }
    }

    [Serializable]
    public class Lot
    {
        [XmlElement("agreementSubject")]
        public string AgreementSubject { get; set; }

        [XmlElement("lotPrice")]
        public string LotPrice { get; set; }

        [XmlElement("unitAmount")]
        public string UnitAmount { get; set; } 

        [XmlElement("nds")]
        public NdsStruct Nds { get; set; }

        [XmlElement("currency")]
        public CurrencyStruct Currency { get; set; }

        [XmlElement("maxUnitPrice")]
        public string MaxUnitPrice { get; set; }

        [XmlElement("offerPriceType")]
        public OfferPriceTypeStruct OfferPriceType { get; set; }

        [XmlElement("delivBasis")]
        public string DelivBasis { get; set; }

        [XmlElement("condSupply")]
        public string CondSupply { get; set; }

        [XmlElement("requirements")]
        public string Requirements { get; set; }

    }

    [Serializable]
    public struct NdsStruct
    {
        [XmlElement("digitalCode")]
        public int DigitalCode { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

    }

    [Serializable]
    public struct CurrencyStruct
    {
        [XmlElement("digitalCode")]
        public int DigitalCode { get; set; }

        [XmlElement("code")]
        public string Code { get; set; }

    }

    [Serializable]
    public struct OfferPriceTypeStruct
    {
        [XmlElement("code")]
        public int Code { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }
    }

}
