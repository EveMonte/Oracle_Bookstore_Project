using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсач.Models
{
    public class Mark
    {
        public int MARK_ID { get; set; }

        public Nullable<int> BOOK_ID { get; set; }

        public Nullable<int> USER_ID { get; set; }

        public Nullable<int> MARK { get; set; }
        public Mark(Nullable<int> book, Nullable<int> user, Nullable<int> mark)
        {
            BOOK_ID = book;
            USER_ID = user;
            MARK = mark;
        }
    }
}
