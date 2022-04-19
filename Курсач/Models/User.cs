using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсач.Models
{
    public class User
    {
        public int USER_ID { get; set; }
        public string EMAIL { get; set; }
        public string SECOND_NAME { get; set; }
        public string NAME { get; set; }
        public string PASSWORD { get; set; }
        public int ACCESS_LEVEL { get; set; }
        public Nullable<int> CARD { get; set; }
        public Nullable<int> SUBSCRIPTION { get; set; }
        public User(string email, string secondName, string name, string password, int access_level, Nullable<int> card = null, Nullable<int> sub = null)
        {
            EMAIL = email;
            SECOND_NAME = secondName;
            NAME = name;
            PASSWORD = password;
            ACCESS_LEVEL = access_level;
            CARD = card;
            SUBSCRIPTION = sub;
        }
    }
}
