using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class MiniTender
    {
        public MiniTender()
        {

        }
        public MiniTender(string id, int idStatus, string updateDate)
        {
            Id = id;
            StatusId = idStatus;
            DateUpdate = updateDate;
        }
        public string Id { get; set; }
        public int StatusId { get; set; }
        public string DateUpdate { get; set; }
    }
}
