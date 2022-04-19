using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсач.Models
{
    public class Book
    {
        public int BOOK_ID { get; set; }

        public string TITLE { get; set; }

        public string AUTHOR { get; set; }

        public Nullable<int> GENRE { get; set; }

        public decimal PRICE { get; set; }

        public string COVER { get; set; }

        public int BY_SUBSCRIPTION { get; set; }

        public decimal RATING { get; set; }

        public string DESCRIPTION { get; set; }

        public string LINK { get; set; }
        public decimal MARK { get; set; }

        public Book(string title, string author, Nullable<int> genre, decimal price, string cover, int sub, decimal rating, string description, string link)
        {
            TITLE = title;
            AUTHOR = author;
            GENRE = genre;
            PRICE = price;
            COVER = cover;
            BY_SUBSCRIPTION = sub;
            RATING = rating;
            DESCRIPTION = description;
            LINK = link;
        }

        public Book()
        {
        }
    }
}
