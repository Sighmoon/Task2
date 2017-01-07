using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class DBTender
    {
        public DBTender()
        {

        }
        public DBTender(string id, int idStatus, string updateDate)
        {
            Id = id;
            StatusId = idStatus;
            DateUpdate = updateDate;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string TypesTorgs { get; set; }
        public string PublicDate { get; set; }
        public string EndPublicationData { get; set; }
        public string DateUpdate { get; set; }
        public int RequestVersions { get; set; }
        public OrganizationStruct Organization { get; set; }
        public Lot[] Lot { get; set; }
    }

    public struct OrganizationStruct
    {
        public string ShortName { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }
        public string PostAddress { get; set; }
        public string LegalAddress { get; set; }
    }
}