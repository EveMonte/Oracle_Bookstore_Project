using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсач.Models
{
    public class Shelf
    {
        public int SHELF_ID { get; set; }

        public Nullable<int> USER_ID { get; set; }

        public Nullable<int> BOOK_ID { get; set; }
        public Shelf(Nullable<int> user, Nullable<int> book)
        {
            USER_ID = user;
            BOOK_ID = book;
        }
    }
}
