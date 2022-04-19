using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсач.Models
{
    public class Advertisment
    {
        public int ADV_ID { get; set; }
        public string IMAGE_SOURCE { get; set; }
        public string LINK { get; set; }

        public Advertisment(string source, string link)
        {
            IMAGE_SOURCE = source;
            LINK = link;
        }

    }
}
