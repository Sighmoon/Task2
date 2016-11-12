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
