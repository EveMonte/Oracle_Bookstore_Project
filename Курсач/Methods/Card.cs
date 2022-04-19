using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсач.Methods
{
    public class Card
    {
        public int CARD_ID { get; set; }
        public string CARD_NUMBER { get; set; }
        public string EXPIRATION_DATE { get; set; }
        public int CVV { get; set; }
        public Card(string number, string date, int cvv)
        {
            CARD_NUMBER = number;
            EXPIRATION_DATE = date;
            CVV = cvv;
        }
    }
}
