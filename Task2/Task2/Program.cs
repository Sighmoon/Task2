using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            data data = data.dataDeserialize("Z-316.xml");
            data.dataSerialize("Z-316(1).xml");
            Console.ReadLine();     

        }
    }
}
