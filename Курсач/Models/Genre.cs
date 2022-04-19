using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсач.Models
{
    public class Genre
    {
        public int GENRE_ID { get; set; }

        public string GENRE { get; set; }

        public Genre(string genre)
        {
            GENRE = genre;
        }
    }
}
